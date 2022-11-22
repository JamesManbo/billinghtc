using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.Commands.ContractorCommands;
using AutoMapper;
using GenericRepository;
using GenericRepository.Configurations;
using GenericRepository.Models;
using Global.Models.StateChangedResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public class ApplicationUserBaseRepository<TEntity, TKey> : CrudRepository<TEntity, TKey>
        where TEntity : Entity, IEntity<TKey>
    {
        protected readonly ApplicationUserDbContext ApplicationUserDbContext;
        public ApplicationUserBaseRepository(ApplicationUserDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            this.ApplicationUserDbContext = context;
        }

        public override ActionResponse DeleteAndSave(params object[] keys)
        {
            bool isHasActiveTransaction = ApplicationUserDbContext.HasActiveTransaction;
            if (!isHasActiveTransaction)
            {
                ApplicationUserDbContext.BeginTransactionAsync().Wait();
            }

            var actionResponse = new ActionResponse<TEntity>();
            var entity = DbSet.Find(keys);

            if (entity == null)
            {
                actionResponse.AddError(
                    $"Sorry, I could not find the {typeof(TEntity).Name} you wanted to delete.");
                return actionResponse;
            }

            entity.IsDeleted = true;
            Update(entity);

            var removePropertyEvent = new RemoveContractorPropertyDomainEvent();

            if (typeof(TEntity) == typeof(CustomerStructure))
            {
                removePropertyEvent.ContractorStructureId = entity.Id;
            }

            if (typeof(TEntity) == typeof(CustomerCategory))
            {
                removePropertyEvent.ContractorCategoryId = entity.Id;
            }

            if (typeof(TEntity) == typeof(ApplicationUserGroup))
            {
                removePropertyEvent.ContractorGroupId = entity.Id;
                removePropertyEvent.ContractorGroupName = (entity as ApplicationUserGroup).GroupName;
            }

            if (typeof(TEntity) == typeof(ApplicationUserClass))
            {
                removePropertyEvent.ContractorClassId = entity.Id;
            }

            if (typeof(TEntity) == typeof(CustomerType))
            {
                removePropertyEvent.ContractorTypeId = entity.Id;
            }

            if (typeof(TEntity) == typeof(Industry))
            {
                removePropertyEvent.ContractorIndustryId = entity.Id;
            }

            removePropertyEvent.TransactionId = ApplicationUserDbContext
                .GetCurrentTransaction().TransactionId;
            entity.AddDomainEvent(removePropertyEvent);

            if (!isHasActiveTransaction)
            {
                ApplicationUserDbContext
                    .CommitTransactionAsync(ApplicationUserDbContext.GetCurrentTransaction())
                    .Wait();
            }

            return actionResponse;
        }

        public async override Task<ActionResponse<TEntity>> UpdateAndSave<T>(T entityOrDto)
        {
            bool isHasActiveTransaction = ApplicationUserDbContext.HasActiveTransaction;
            if (!isHasActiveTransaction)
            {
                await ApplicationUserDbContext.BeginTransactionAsync();
            }

            var updateContractorEvent = new UpdateContractorPropertyDomainEvent();
            TEntity targetEntity;

            var updaterIds = FindPrimaryKeyValues(ConvertToEntity(entityOrDto));
            if (!updaterIds.Any())
            {
                throw new InvalidOperationException(
                    $"The primary key was not set on the entity class {typeof(TEntity).Name}.");
            }

            var mapper = new Mapper(_configAndMapper.MapperConfig);
            if (updaterIds.Count() > 1)
            {
                targetEntity = Context.Find<TEntity>(updaterIds);
            }
            else
            {
                targetEntity = Context.Find<TEntity>(updaterIds.Single());
            }

            if (targetEntity is ApplicationUserGroup originalGroupObject)
            {
                updateContractorEvent.OldContractorGroupName = originalGroupObject.GroupName;
            }

            if (targetEntity is Industry originalIndustryObject)
            {
                updateContractorEvent.OldContractorIndustryName = originalIndustryObject.Name;
            }

            /// Mapping properties of destination object to source
            mapper.Map(entityOrDto, targetEntity);

            if (targetEntity == null) return default;

            targetEntity.UpdatedDate = DateTime.Now;
            targetEntity.CreatedBy = UserIdentity?.UserName;
            var actionResponse = base.UpdateEntity(targetEntity);

            if (actionResponse.IsSuccess)
            {

                if (actionResponse.Result is CustomerStructure customerStructure)
                {
                    updateContractorEvent.ContractorStructureId = customerStructure.Id;
                    updateContractorEvent.ContractorStructureName = customerStructure.Name;
                }

                if (actionResponse.Result is CustomerCategory customerCategory)
                {
                    updateContractorEvent.ContractorCategoryId = customerCategory.Id;
                    updateContractorEvent.ContractorCategoryName = customerCategory.Name;
                }

                if (actionResponse.Result is ApplicationUserGroup customerGroup)
                {
                    updateContractorEvent.ContractorGroupId = customerGroup.Id.ToString();
                    updateContractorEvent.NewContractorGroupName = customerGroup.GroupName;
                }

                if (actionResponse.Result is ApplicationUserClass customerClass)
                {
                    updateContractorEvent.ContractorClassId = customerClass.Id;
                    updateContractorEvent.ContractorClassName = customerClass.ClassName;
                }

                if (actionResponse.Result is CustomerType customerType)
                {
                    updateContractorEvent.ContractorTypeId = customerType.Id;
                    updateContractorEvent.ContractorTypeName = customerType.Name;
                }

                if (actionResponse.Result is Industry applicationUserIndustry)
                {
                    updateContractorEvent.ContractorIndustryId = applicationUserIndustry.Id.ToString();
                    updateContractorEvent.NewContractorIndustryName = applicationUserIndustry.Name;
                }

                updateContractorEvent.TransactionId = ApplicationUserDbContext.GetCurrentTransaction().TransactionId;
                targetEntity.AddDomainEvent(updateContractorEvent);

                if (!isHasActiveTransaction)
                {
                    await ApplicationUserDbContext.CommitTransactionAsync(
                        ApplicationUserDbContext.GetCurrentTransaction()
                        );
                }
            }

            return actionResponse;
        }
    }
}
