using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Models;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationUserIdentity.API.Infrastructure.Repositories
{
    public class PictureRepository: CrudRepository<Models.Picture, int>, IPictureRepository
    {
        public PictureRepository(ApplicationUserDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {

        }
    }
}
