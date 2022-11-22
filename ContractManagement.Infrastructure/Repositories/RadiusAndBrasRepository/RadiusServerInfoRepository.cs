using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContractManagement.Domain.AggregatesModel.RadiusAggregate;
using ContractManagement.Domain.Models.RadiusAndBras;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.Infrastructure.Repositories.RadiusAndBrasRepository
{
    public interface IRadiusServerInfoRepository : ICrudRepository<RadiusServerInformation, int>
    {
        Task<IEnumerable<RadiusServerInfoDTO>> GetAll();
    }
    public class RadiusServerInfoRepository : CrudRepository<RadiusServerInformation, int>, IRadiusServerInfoRepository
    {
        protected IWrappedConfigAndMapper _configAndMapper;
        public RadiusServerInfoRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper)
            : base(context, configAndMapper)
        {
            this._configAndMapper = configAndMapper;
        }

        public async Task<IEnumerable<RadiusServerInfoDTO>> GetAll()
        {
            return await DbSet.Where(r => !r.IsDeleted && r.IsActive)
                .ProjectTo<RadiusServerInfoDTO>(this._configAndMapper.MapperConfig)
                .ToListAsync();
        }
    }
}
