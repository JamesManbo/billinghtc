using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GenericRepository.Configurations;
using GenericRepository.Core;
using GenericRepository.Extensions;
using GenericRepository.Models;
using Global.Models.StateChangedResponse;
using Microsoft.EntityFrameworkCore;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using GenericRepository.BulkOperation;
using Global.Models.Auth;

namespace GenericRepository
{
    public class CrudRepository<TEntity, TEType> : BaseBulkOperation<TEntity>, ICrudRepository<TEntity, TEType> where TEntity : class, IEntity<TEType>
    {
        public UserIdentity UserIdentity { get; set; }

        private readonly DbContext _context;
        protected readonly IWrappedConfigAndMapper _configAndMapper;

        /// <inheritdoc />
        public DbContext Context => _context;

        public DbSet<TEntity> DbSet => _context.Set<TEntity>();

        /// <summary>
        /// CrudServices needs the correct DbContext and the AutoMapper config
        /// </summary>
        /// <param name="context"></param>
        /// <param name="configAndMapper"></param>
        public CrudRepository(DbContext context, IWrappedConfigAndMapper configAndMapper) : base(context)
        {
            _context = context ?? throw new ArgumentNullException(
                "The DbContext class is null. Either you haven't registered GenericServices, " +
                "or you are using the multi-DbContext version, in which case you need to use the CrudServices<TContext> and specify which DbContext to use.");
            _configAndMapper = configAndMapper ?? throw new ArgumentException(nameof(configAndMapper));
        }

        public virtual Task<TEntity> GetByIdAsync(object id)
        {
            if (!IsExisted(id)) return Task.FromResult(default(TEntity));

            return _context.Set<TEntity>().FindAsync(id).AsTask();
        }

        /// <inheritdoc />
        public virtual async Task<ActionResponse<TEntity>> CreateAndSave<T>(T entityOrDto)
            where T : class
        {
            var createEntityResp = this.Create(entityOrDto);

            if (!createEntityResp.IsSuccess)
            {
                return createEntityResp;
            }

            _context.Add(createEntityResp.Result);

            await SaveChangeAsync();

            return createEntityResp;
        }

        public virtual ActionResponse<TEntity> Create<T>(T entityOrDto) where T : class
        {
            var actionResponse = CreateEntityHandler(entityOrDto);
            if (actionResponse.IsSuccess)
            {
                _context.Add(actionResponse.Result);
            }

            var validation = ValidateEntity(actionResponse.Result);
            actionResponse.CombineResponse(validation);
            return actionResponse;
        }

        private ActionResponse<TEntity> CreateEntityHandler<T>(T entityOrDto)
            where T : class
        {
            var actionResponse = new ActionResponse<TEntity>();
            TEntity creatingEntity;

            if (typeof(TEntity) == typeof(T))
            {
                creatingEntity = entityOrDto as TEntity;
            }
            else
            {
                creatingEntity = entityOrDto.MapTo<TEntity>(_configAndMapper.MapperConfig);
            }

            if (creatingEntity != null)
            {
                creatingEntity.OrganizationPath =
                    string.IsNullOrEmpty(creatingEntity.OrganizationPath)
                    ? UserIdentity?.OrganizationPaths?.FirstOrDefault()
                    : creatingEntity.OrganizationPath;

                creatingEntity.CreatedBy = string.IsNullOrWhiteSpace(creatingEntity.CreatedBy)
                    ? UserIdentity?.UserName
                    : creatingEntity.CreatedBy;

                creatingEntity.CreatedDate = DateTime.Now;
                creatingEntity.IsDeleted = false;
                actionResponse.SetResult(creatingEntity);
            }

            return actionResponse;
        }

        /// <inheritdoc />
        public virtual async Task<ActionResponse<TEntity>> UpdateAndSave<T>(T entityOrDto) where T : class
        {
            var actionResponse = Update(entityOrDto);
            if (actionResponse.IsSuccess)
            {
                await SaveChangeAsync();
            }

            var entity = actionResponse.Result;

            return new ActionResponse<TEntity>().SetResult(entity);
        }

        public virtual ActionResponse<TEntity> Update<T>(T entityOrDto) where T : class
        {
            TEntity updater;

            var updaterIds = FindPrimaryKeyValues(ConvertToEntity(entityOrDto));
            if (!updaterIds.Any())
            {
                throw new InvalidOperationException(
                    $"The primary key was not set on the entity class {typeof(TEntity).Name}.");
            }

            if (typeof(T) == typeof(TEntity))
            {
                updater = entityOrDto as TEntity;
            }
            else
            {
                var mapper = new Mapper(_configAndMapper.MapperConfig);
                TEntity targetEntity;
                if (updaterIds.Count() > 1)
                {
                    targetEntity = Context.Find<TEntity>(updaterIds);
                }
                else
                {
                    targetEntity = Context.Find<TEntity>(updaterIds.Single());
                }

                updater = mapper.Map(entityOrDto, targetEntity);
            }

            if (updater == null) return default;

            updater.UpdatedDate = DateTime.Now;
            updater.UpdatedBy = string.IsNullOrWhiteSpace(updater.UpdatedBy)
                    ? UserIdentity?.UserName
                    : updater.UpdatedBy;

            return UpdateEntity(updater);
        }

        public virtual ActionResponse<TEntity> UpdateEntity(TEntity entity)
        {
            var actionResponse = new ActionResponse<TEntity>();
            actionResponse.SetResult(entity);

            if (!_context.Entry(entity).IsKeySet)
                throw new InvalidOperationException(
                    $"The primary key was not set on the entity class {typeof(TEntity).Name}. For an update we expect the key(s) to be set (otherwise it does a create).");
            if (_context.Entry(entity).State == EntityState.Detached)
                _context.Update(entity);

            actionResponse.CombineResponse(ValidateEntity(entity));

            return actionResponse;
        }

        protected TEntity ConvertToEntity<T>(T convertibleObject)
        {
            if (typeof(TEntity) == typeof(T))
            {
                return convertibleObject as TEntity;
            }

            return convertibleObject.MapTo<TEntity>(_configAndMapper.MapperConfig);
        }

        private ActionResponse<TEntity> ValidateEntity(TEntity entity)
        {
            var status = new ActionResponse<TEntity>();

            var valProvider = new ValidationDbContextServiceProvider(_context);
            var valContext = new ValidationContext(entity, valProvider, null);
            var entityErrors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(
                entity, valContext, entityErrors, true))
            {
                status.AddValidationResults(entityErrors);
            }

            return status;
        }

        protected IEnumerable<string> FindPrimaryKeyNames()
        {
            return Context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties
                .Select(x => x.Name);
        }

        protected List<object> FindPrimaryKeyValues(TEntity entity)
        {
            var keyNames = FindPrimaryKeyNames();
            return keyNames.Select(k => GetPropertyValue(entity, k)).ToList();
        }

        protected object GetPropertyValue(TEntity entity, string propertyName)
        {
            return entity.GetType().GetProperty(propertyName)?.GetValue(entity, null);
        }

        /// <inheritdoc />
        public virtual ActionResponse DeleteAndSave(params object[] keys)
        {
            var actionResponse = new ActionResponse();
            var entity = _context.Set<TEntity>().Find(keys);

            if (entity == null)
            {
                actionResponse.AddError(
                    $"Sorry, I could not find the {ExtractDisplayHelpers.GetNameForClass<TEntity>()} you wanted to delete.");
                return actionResponse;
            }

            entity.IsDeleted = true;
            Update(entity);

            actionResponse.CombineResponse(SaveChangeWithValidation());

            return actionResponse;
        }

        /// <inheritdoc />
        public virtual ActionResponse DeleteWithActionAndSave(Func<DbContext, TEntity, IActionResponse> runBeforeDelete,
            params object[] keys)
        {
            var actionResponse = new ActionResponse();
            var entity = _context.Set<TEntity>().Find(keys);
            if (entity == null)
            {
                actionResponse.AddError(
                    $"Sorry, I could not find the {ExtractDisplayHelpers.GetNameForClass<TEntity>()} you wanted to delete.");
                return actionResponse;
            }

            actionResponse.CombineResponse(runBeforeDelete(_context, entity));
            if (!actionResponse.IsSuccess) return actionResponse;

            entity.IsDeleted = true;
            Update(entity);

            actionResponse.CombineResponse(SaveChangeWithValidation());

            return actionResponse;
        }

        public virtual bool IsExisted(object id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            return entity != null && !entity.IsDeleted;
        }

        public async Task<ActionResponse<TEntity>> RemoveAndSave<T>(T entityOrDto) where T : class
        {
            var actionResponse = new ActionResponse<TEntity>();
            var createEntityResp = CreateEntityHandler(entityOrDto);

            if (!createEntityResp.IsSuccess)
            {
                actionResponse.CombineResponse(createEntityResp);
                return actionResponse;
            }

            var entity = createEntityResp.Result;

            _context.Remove(entity);

            await SaveChangeAsync();

            if (!actionResponse.IsSuccess) return actionResponse;

            actionResponse.SetResult(entity);

            return actionResponse;
        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public IActionResponse SaveChangeWithValidation(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesWithValidation();
        }

        public async Task<ActionResponse> SaveChangeWithValidationAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesWithValidationAsync(cancellationToken);
        }
    }
}