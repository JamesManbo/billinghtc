using GenericRepository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenericRepository.BulkOperation
{ 
    public interface IBulkOperation<TEntity>
    {
        /// <summary>
        /// Executes a bulk insert command from the provider.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <param name="entities">Entities to insert.</param>
        Task<int> InsertBulk<TModel>(List<TModel> entities);
    }
}
