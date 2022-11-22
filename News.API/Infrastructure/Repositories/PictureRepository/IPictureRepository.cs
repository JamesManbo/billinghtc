using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using News.API.Models;
using GenericRepository;

namespace News.API.Infrastructure.Repositories
{
    public interface IPictureRepository : ICrudRepository<Models.Picture, int>
    {
    }
}
