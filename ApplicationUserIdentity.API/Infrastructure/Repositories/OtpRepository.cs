using ApplicationUserIdentity.API.Models.Otp;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public class OtpRepository : CrudRepository<OtpEntity, int>, IOtpRepository
    {
        public OtpRepository(ApplicationUserDbContext organizationUnitDbContext, IWrappedConfigAndMapper configAndMapper) : base(organizationUnitDbContext, configAndMapper)
        {
            
        }
    }
}
