using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.InOutContractTaxRepository
{
    public class InContractTaxRepository : CrudRepository<Domain.AggregatesModel.ContractOfTaxAggregate.InContractTax, int>, IInContractTaxRepository
    {
        private readonly ContractDbContext _contractDbContext;

        public InContractTaxRepository(ContractDbContext contractDbContext, IWrappedConfigAndMapper configAndMapper) : base(contractDbContext, configAndMapper)
        {
            _contractDbContext = contractDbContext;
        }

        public bool DeleteAllByInContractId(int inContractId)
        {
            var lstContractOfTax = _contractDbContext.InContractTaxes.Where(x => x.InContractId == inContractId);
            if (lstContractOfTax.Count() > 0)
            {
                foreach (var i in lstContractOfTax)
                {
                    _contractDbContext.InContractTaxes.Remove(i);
                    _contractDbContext.SaveChanges();

                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
