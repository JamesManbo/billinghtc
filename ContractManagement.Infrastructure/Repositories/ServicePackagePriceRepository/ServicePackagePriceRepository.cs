using System;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using GenericRepository;
using GenericRepository.Configurations;
using Global.Models.StateChangedResponse;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace ContractManagement.Infrastructure.Repositories.ServicePackagePriceRepository
{
    public class ServicePackagePriceRepository : CrudRepository<ServicePackagePrice, int>, IServicePackagePriceRepository
    {
        private readonly ContractDbContext _contractDbContext;
        public ServicePackagePriceRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }

        public async Task<ActionResponse> DeleteAndSaveByPackageIdAsync(int packageId)
        {
            var lstExist = _contractDbContext.ServicePackagePrice.Where(p => p.ChannelId == packageId).ToList();
            if (lstExist.Any())
            {
                lstExist.ForEach(e =>
                {
                    e.UpdatedDate = DateTime.Now;
                    e.IsDeleted = true;
                    e.IsActive = false;
                    _contractDbContext.ServicePackagePrice.Update(e);
                });
                await _contractDbContext.SaveChangesAsync();
            }
            var actionResponse = new ActionResponse();
            return actionResponse;
        }
        public async Task<ActionResponse> DeleteWithListId(string listId,int serviceId)
        {
            try
            {
                string sQuery = $"CALL sp_DeleteServicePackage(@listId,@serviceId)";
                _contractDbContext.Database.ExecuteSqlRaw(sQuery,
                    new MySqlParameter("@listId", MySqlDbType.MediumText) { Value = listId },
                    new MySqlParameter("@serviceId", MySqlDbType.Int32) { Value = serviceId });
                var actionResponse = new ActionResponse();
                return actionResponse;
            }
            catch (Exception ex)
            {
                var actionResponse = new ActionResponse();
                return actionResponse;
            }
           
        }
    }
}
