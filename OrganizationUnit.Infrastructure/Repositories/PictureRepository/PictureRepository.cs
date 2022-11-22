using System;
using System.Collections.Generic;
using System.Text;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using OrganizationUnit.Domain.AggregateModels.PictureAggregate;

namespace OrganizationUnit.Infrastructure.Repositories.PictureRepository
{
    public class PictureRepository : CrudRepository<OrganizationUnit.Domain.AggregateModels.PictureAggregate.Picture, int>, IPictureRepository
    {
        public PictureRepository(OrganizationUnitDbContext context, IWrappedConfigAndMapper configAndMapper) : base(
            context, configAndMapper)
        {
        }
    }
}