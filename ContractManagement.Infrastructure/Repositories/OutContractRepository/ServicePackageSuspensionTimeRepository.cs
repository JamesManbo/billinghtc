using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories.OutContractRepository
{
    public interface IServicePackageSuspensionTimeRepository : ICrudRepository<ServicePackageSuspensionTime, int>
    {
        Task<List<ServicePackageSuspensionTime>> GetByIdsAsync(int[] ids, bool? isActive = null);
        Task<List<ServicePackageSuspensionTime>> GetByContractIdsAsync(int[] ids);
        Task<List<ServicePackageSuspensionTime>> GetByChannelIdsAsync(int[] ids);
    }

    public class ServicePackageSuspensionTimeRepository : CrudRepository<ServicePackageSuspensionTime, int>, IServicePackageSuspensionTimeRepository
    {
        public ServicePackageSuspensionTimeRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
        }

        public async Task<List<ServicePackageSuspensionTime>> GetByIdsAsync(int[] ids, bool? isActive = null)
        {
            return await DbSet
                .Include(c => c.Channel)
                .Where(o => ids.Contains(o.Id) && (!isActive.HasValue || o.IsActive == isActive))
                .ToListAsync();
        }

        public async Task<List<ServicePackageSuspensionTime>> GetByContractIdsAsync(int[] ids)
        {
            return await DbSet
                .Include(c => c.Channel)
                .Where(o => o.Channel.OutContractId.HasValue
                    && ids.Contains(o.Channel.OutContractId.Value)
                    && o.IsActive == true)
                .ToListAsync();
        }

        public async Task<List<ServicePackageSuspensionTime>> GetByChannelIdsAsync(int[] ids)
        {
            return await DbSet
                .Include(c => c.Channel)
                .Where(o => ids.Contains(o.OutContractServicePackageId)
                    && o.IsActive == true)
                .ToListAsync();
        }
    }
}
