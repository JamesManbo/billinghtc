using GenericRepository;
using GenericRepository.Configurations;
using OrganizationUnit.Domain.AggregateModels.OTPAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Repositories.OtpRepository
{
    public class OtpRepository : CrudRepository<OtpEntity, int>, IOtpRepository
    {
        private readonly OrganizationUnitDbContext _organizationUnitDbContext;

        public OtpRepository(OrganizationUnitDbContext organizationUnitDbContext, IWrappedConfigAndMapper configAndMapper) : base(organizationUnitDbContext, configAndMapper)
        {
            _organizationUnitDbContext = organizationUnitDbContext;
        }
    }
}
