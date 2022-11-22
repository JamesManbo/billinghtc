using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Models;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace ApplicationUserIdentity.API.Infrastructure.Queries
{
    public class PictureQueries : QueryRepository<Models.Picture, int>, IPictureQueries
    {
        public PictureQueries(ApplicationUserDbContext context) : base(context)
        {
        }
    }
}
