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

namespace ContractManagement.Infrastructure.Repositories.RadiusAndBrasRepository
{
    public interface IBrasInfoRepository : ICrudRepository<BrasInformation, int>
    {
        IEnumerable<BrasInfoDTO> GetAll();
    }
    public class BrasInfoRepository : CrudRepository<BrasInformation, int>, IBrasInfoRepository
    {
        protected IWrappedConfigAndMapper _configAndMapper;
        public BrasInfoRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) 
            : base(context, configAndMapper)
        {
            this._configAndMapper = configAndMapper;
        }
        public IEnumerable<BrasInfoDTO> GetAll()
        {
            return DbSet.Where(r => !r.IsDeleted && r.IsActive)
                .ProjectTo<BrasInfoDTO>(this._configAndMapper.MapperConfig)
                .ToList();
        }
    }
}
