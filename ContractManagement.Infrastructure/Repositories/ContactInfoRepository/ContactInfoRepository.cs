using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ContractManagement.Infrastructure.Repositories.ContactInfoRepository
{
    public class ContactInfoRepository : CrudRepository<ContactInfo, int>, IContactInfoRepository
    {
        private readonly ContractDbContext _context;
        public ContactInfoRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _context = context;
        }


        public void DeleteContactInfo(int inContractId)
        {
            var lstContactInfo = _context.ContactInfos.Where(x => x.IsDeleted == false && x.InContractId == inContractId).ToList();
            foreach(var i in lstContactInfo)
            {
                _context.ContactInfos.Remove(i);
            }
            _context.SaveChanges();
        }
    }
}
