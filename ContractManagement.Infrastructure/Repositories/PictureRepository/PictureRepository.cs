using System;
using System.Collections.Generic;
using System.Text;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using ContractManagement.Domain.AggregateModels.PictureAggregate;

namespace ContractManagement.Infrastructure.Repositories.PictureRepository
{
    public class PictureRepository : CrudRepository<Picture, int>, IPictureRepository
    {
        public PictureRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) : base(
            context, configAndMapper)
        {
        }
    }
}