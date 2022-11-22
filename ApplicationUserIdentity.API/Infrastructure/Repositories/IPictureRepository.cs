using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Models;
using GenericRepository;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public interface IPictureRepository : ICrudRepository<Models.Picture, int>
    {
    }
}
