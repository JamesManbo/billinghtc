using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using GenericRepository.BulkOperation;
using GenericRepository.Core;
using GenericRepository.Models;
using Global.Models.StateChangedResponse;
using Microsoft.EntityFrameworkCore;

namespace GenericRepository
{
    public interface ICrudRepository<TEntity, TEType> : IBulkOperation<TEntity> where TEntity : class, IEntity<TEType>
    {
        DbContext Context { get; }

        /// <summary>
        /// This will create a new entity in the database. If you provide class which is an entity class (i.e. in your EF Core database) then
        /// the method will add, and then call SaveChanges. If the class you provide is a CrudServices DTO which has a <see cref="ILinkToEntity{TEntity}"/> interface
        /// it will use that to create the entity by matching the DTOs properties to either, a) a public static method, b) a public ctor, or 
        /// c) by setting the properties with public setters in the entity (using AutoMapper)
        /// </summary>
        /// <typeparam name="T">This type is found from the input instance</typeparam>
        /// <param name="entityOrDto">This should either be an instance of a entity class or a CrudServices DTO which has a <see cref="ILinkToEntity{TEntity}"/> interface</param>
        /// <param name="ctorOrStaticMethodName">Optional: you can tell GenericServices which static method, ctor or CrudValues.UseAutoMapper to use</param>
        /// <returns>It returns the class you provided. It will contain the primary key defined after the database. 
        /// If its a DTO then GenericServices will have copied the keys from the entity added back into the DTO</returns>
        Task<TEntity> GetByIdAsync(object id);
        Task<ActionResponse<TEntity>> CreateAndSave<T>(T entityOrDto) where T : class;

        ActionResponse<TEntity> Create<T>(T entityOrDto) where T : class;

        /// <summary>
        /// This will update the entity referred to by the keys in the given class instance.
        /// For a entity class instance it will check the state of the instance. If its detached it will call Update, otherwise it assumes its tracked and calls SaveChanges
        /// For a CrudServices DTO it will: 
        /// a) load the existing entity class using the primary key(s) in the DTO
        /// b) This it will look for a public method that match the DTO's properties to do the update, or if no method is found it will try to use AutoMapper to copy the data over to the e
        /// c) finally it will call SaveChanges
        /// </summary>
        /// <typeparam name="T">This type is found from the input instance</typeparam>
        /// <param name="entityOrDto">This should either be an instance of a entity class or a CrudServices DTO which has a <see cref="ILinkToEntity{TEntity}"/> interface</param>
        /// <param name="methodName">Optional: you can give the method name to be used for the update, or CrudValues.UseAutoMapper to make it use AutoMapper to update the entity.</param>
        Task<ActionResponse<TEntity>> UpdateAndSave<T>(T entityOrDto) where T : class;
        Task<ActionResponse<TEntity>> RemoveAndSave<T>(T entityOrDto) where T : class;
        ActionResponse<TEntity> Update<T>(T entityOrDto) where T : class;
        /// <summary>
        /// This will delete the entity class with the given primary key
        /// </summary>
        /// <typeparam name="TEntity">The entity class you want to delete. It should be an entity in the DbContext you are referring to.</typeparam>
        /// <param name="keys">The key(s) value. If there are multiple keys they must be in the correct order as defined by EF Core</param>
        ActionResponse DeleteAndSave(params object[] keys);

        /// <summary>
        /// This will find entity class with the given primary key, then call the method you provide before calling the Remove method + SaveChanges.
        /// Your method has access to the database and can handle any relationships, and returns an <see cref="IStatusGeneric"/>. The Remove will 
        /// only go ahead if the status your method returns is Valid, i.e. no errors
        /// NOTE: This method ignore any query filters when deleting. If you are working in a multi-tenant system you should include a test
        /// that the entity you are deleting has the correct TenantId
        /// </summary>
        /// <typeparam name="TEntity">The entity class you want to delete. It should be an entity in the DbContext you are referring to.</typeparam>
        /// <param name="runBeforeDelete">You provide a method, which is called after the entity to delete has been loaded, but before the Remove method is called.
        /// Your method has access to the database and can handle any relationships, and returns an <see cref="IStatusGeneric"/>. The Remove will 
        /// only go ahead if the status your method returns is Valid, i.e. no errors</param>
        /// <param name="keys">The key(s) value. If there are multiple keys they must be in the correct order as defined by EF Core</param>
        ActionResponse DeleteWithActionAndSave(Func<DbContext, TEntity, IActionResponse> runBeforeDelete,
            params object[] keys);

        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
        Task<ActionResponse> SaveChangeWithValidationAsync(CancellationToken cancellationToken = default);
        bool IsExisted(object id);

    }
}
