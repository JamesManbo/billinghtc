using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.Models.OutContracts;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ContractManagement.Infrastructure.Repositories.OutContractRepository
{
    public interface IContractorRepository : ICrudRepository<Contractor, int>
    {
        Task<Contractor> FindContractorByIdentityGuid(string identityGuid);
        bool CheckExitsContractor(string fullName, string address, string mobile);
        bool CheckExitsCodeHTC(string code, int id = 0);
        bool CheckExitsPhoneHTC(string phone, int id = 0);
        bool CheckExitsHTC(int id);
    }

    public class ContractorRepository : CrudRepository<Contractor, int>, IContractorRepository
    {
        public readonly ContractDbContext _contractDbContext;
        public ContractorRepository(ContractDbContext context,
            IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _contractDbContext = context;
        }

        public Task<Contractor> FindContractorByIdentityGuid(string identityGuid)
        {
            return DbSet.FirstOrDefaultAsync(c => c.IdentityGuid.Equals(identityGuid));
        }

        public bool CheckExitsContractor(string fullName, string address, string mobile)
        {
            return _contractDbContext.Contractors.Any(x => x.ContractorFullName.Contains(fullName) && x.ContractorAddress.Contains(address) && x.ContractorPhone.Contains(mobile));
        }

        public bool CheckExitsHTC(int id)
        {
            return _contractDbContext.Contractors.Any(x => x.IsHTC == true && x.Id == id);
        }

        public bool CheckExitsCodeHTC(string code, int id = 0)
        {
            return _contractDbContext.Contractors.Any(x => x.ContractorCode == code && x.IsHTC == true && x.Id != id);
        }

        public bool CheckExitsPhoneHTC(string phone, int id = 0)
        {
            return _contractDbContext.Contractors.Any(x => x.ContractorCode == phone && x.IsHTC == true && x.Id != id);
        }
    }
}