using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ContactInfoRepository
{
    public interface IContactInfoRepository : ICrudRepository<ContactInfo, int>
    {
        void DeleteContactInfo(int inContractId);
    }
}
