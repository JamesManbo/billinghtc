using GenericRepository;
using OrganizationUnit.Domain.AggregateModels.OTPAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrganizationUnit.Infrastructure.Repositories.OtpRepository
{
    public interface IOtpRepository : ICrudRepository<OtpEntity, int>
    {
    }
}
