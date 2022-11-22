using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ContractManagement.Infrastructure.Repositories.OutContractRepository
{
    public class OutContractRepository : CrudRepository<OutContract, int>, IOutContractRepository
    {
        private readonly ContractDbContext _contractDbContext;
        public OutContractRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }

        public async Task<int> AutoRenewExpirationContract()
        {
            var localDate = DateTime.UtcNow.AddHours(7);
            var expiredContracts = this.DbSet.Where(
                c => c.TimeLine.Expiration.HasValue &&
                    EF.Functions.DateDiffDay(localDate, c.TimeLine.Expiration) <= 0 &&
                    c.AutoRenew == true
                );

            foreach (var contract in expiredContracts)
            {
                contract.TimeLine.Expiration = contract.TimeLine.Expiration.Value.AddMonths(
                    contract.TimeLine.RenewPeriod
                );

                UpdateEntity(contract);
            }

            return await SaveChangeAsync();
        }

        public override Task<OutContract> GetByIdAsync(object id)
        {
            return DbSet.Include(e => e.ContactInfos)
                .Include(e => e.Contractor)
                //.Include(e => e.OutContractOfTaxes)
                //.Include(e => e.ContractSharingRevenues)
                .Include(e => e.ContractTotalByCurrencies)
                .Include(e => e.ServicePackages)
                .ThenInclude(s => s.StartPoint)
                .ThenInclude(s => s.Equipments)
                .Include(e => e.ServicePackages)
                .ThenInclude(s => s.EndPoint)
                .ThenInclude(s => s.Equipments)
                .Include(e => e.ServicePackages)
                .ThenInclude(e => e.AppliedPromotions)
                .Include(e => e.ServicePackages)
                .ThenInclude(e => e.TaxValues)
                .Include(e => e.ContractContent)
                .Include(e => e.ServicePackages)
                .ThenInclude(e => e.ServiceLevelAgreements)
                .Include(e => e.ServicePackages)
                .ThenInclude(e => e.PaymentTarget)
                .Include(e => e.ServicePackages)
                .ThenInclude(e => e.PriceBusTables)
                .Where(e => e.Id == (int)id).FirstOrDefaultAsync();
        }

        public Task<List<OutContract>> GetByTransactionIdsAsync(int[] ids)
        {
            return DbSet
                .Include(e => e.ServicePackages)
                .ThenInclude(s => s.StartPoint)
                .ThenInclude(s => s.Equipments)
                .Include(e => e.ServicePackages)
                .ThenInclude(s => s.EndPoint)
                .ThenInclude(s => s.Equipments)
                .Where(e => e.Transactions.Any(a => ids.Contains(a.Id))).ToListAsync();
        }
    }
}