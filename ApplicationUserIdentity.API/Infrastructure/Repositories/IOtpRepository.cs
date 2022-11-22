using ApplicationUserIdentity.API.Models.Otp;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public interface IOtpRepository : ICrudRepository<OtpEntity, int>
    {
    }
}
