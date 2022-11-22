using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Models;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ContractManagement.Infrastructure.Repositories.ContractEquipmentRepository
{
    public interface IContractEquipmentRepository : ICrudRepository<ContractEquipment, int>
    {
    }

    public class ContractEquipmentRepository : CrudRepository<ContractEquipment, int>, IContractEquipmentRepository
    {
        public ContractEquipmentRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(
            context, configAndMapper)
        {
        }
    }
}