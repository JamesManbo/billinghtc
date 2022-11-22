using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using News.API.Models;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;

namespace News.API.Infrastructure.Repositories
{
    public class PictureRepository: CrudRepository<Models.Picture, int>, IPictureRepository
    {
        public PictureRepository(NewsDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {

        }
    }
}
