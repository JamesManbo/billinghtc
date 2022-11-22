using ContractManagement.Domain.AggregatesModel.BaseContract;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ContractFormRepository
{
    public class ContractFormRepository : CrudRepository<ContractForm, int>, IContractFormRepository
    {
        private readonly ContractDbContext _context;
        public ContractFormRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _context = context;
        }

        public bool CheckExits(int id)
        {
            return _context.ContractForms.Any(x => x.IsDeleted != true && x.Id == id);
        }

        public bool CheckExitsName(string name, int id = 0)
        {
            if (string.IsNullOrWhiteSpace(name)) return true;

            return _context.ContractForms.Any(x => x.IsDeleted != true && name.Equals(x.Name, StringComparison.OrdinalIgnoreCase) && x.Id != id);
        }
    }
}
