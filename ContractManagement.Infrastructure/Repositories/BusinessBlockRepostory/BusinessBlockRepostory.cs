using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.BusinessBlockRepostory
{
    public class BusinessBlockRepostory : CrudRepository<ManagementBusinessBlock, int>, IBusinessBlockRepostory
    {
        private readonly ContractDbContext _context;
        public BusinessBlockRepostory(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            _context = context;
        }
    }
}
