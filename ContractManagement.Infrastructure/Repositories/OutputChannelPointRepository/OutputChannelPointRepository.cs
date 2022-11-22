using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.OutputChannelPointRepository
{
    public interface IOutputChannelPointRepository : ICrudRepository<OutputChannelPoint, int>
    {

    }
    public class OutputChannelPointRepository : CrudRepository<OutputChannelPoint, int>, IOutputChannelPointRepository
    {
        private readonly IWrappedConfigAndMapper _configAndMapper;
        public OutputChannelPointRepository(ContractDbContext context, 
            IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            this._configAndMapper = configAndMapper;
        }
    }
}
