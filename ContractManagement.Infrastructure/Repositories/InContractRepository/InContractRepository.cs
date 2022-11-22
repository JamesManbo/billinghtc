using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories.InContractRepository
{
    public class InContractRepository : CrudRepository<InContract, int>, IInContractRepository
    {
        private readonly ContractDbContext _context;
        public InContractRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _context = context;
        }

        public override Task<InContract> GetByIdAsync(object id)
        {
            return DbSet
                .Include(e => e.Contractor)
                .Include(e => e.ContractContent)
                .Include(e => e.ServicePackages)
                .ThenInclude(s => s.StartPoint)
                .ThenInclude(s => s.Equipments)
                .Include(e => e.ServicePackages)
                .ThenInclude(s => s.EndPoint)
                .ThenInclude(s => s.Equipments)
                .Include(e => e.ServicePackages)
                .ThenInclude(e => e.TaxValues)
                .Include(e => e.ServicePackages)
                .ThenInclude(e => e.ServiceLevelAgreements)
                .Include(e => e.ServicePackages)
                .ThenInclude(e => e.PriceBusTables)
                // Không lấy collection này vì dữ liệu sẽ rất lớn,
                // thay vào đó việc Update/Delete sẽ thực hiện qua repository riêng của entity này
                //.Include(e => e.ContractSharingRevenues)
                .Include(e => e.ContractTotalByCurrencies)
                .Where(e => e.Id == (int) id).FirstOrDefaultAsync();
        }

        public bool DeleteContractSharingRevenuesByIds(int[] contractSharingRevenueIds, string updateBy)
        {
            _context.Database.ExecuteSqlRaw($"CALL DeleteContractSharingRevenuesByIds(@contractSharingRevenueIds, @updateBy)",
                new MySqlParameter("@contractSharingRevenueIds", MySqlDbType.VarChar) { Value = string.Join(",", contractSharingRevenueIds) },
                new MySqlParameter("@updateBy", MySqlDbType.VarChar) { Value = updateBy });
            return true;
        }

        public bool IsExistByCode(string contractCode, int? id = null)
        {
            return _context.InContracts.Any(o => 
                o.ContractCode.Trim().Equals(contractCode.Trim(), StringComparison.OrdinalIgnoreCase)
                && o.Id != id 
                && o.IsDeleted == false);
        }
    }
}
