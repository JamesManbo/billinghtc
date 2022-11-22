using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ProjectRepository
{

    public interface IProjectTechnicianRepository : ICrudRepository<ProjectTechnician, int>
    {
        void RemoveOldProjectTechnician(int id);
    }
    public class ProjectTechnicianRepository : CrudRepository<ProjectTechnician, int>, IProjectTechnicianRepository
    {
        private readonly ContractDbContext _contractDbContext;
        public ProjectTechnicianRepository(ContractDbContext contractDbContext, IWrappedConfigAndMapper configAndMapper) : base(contractDbContext, configAndMapper)
        {
            _contractDbContext = contractDbContext;
        }
        public void RemoveOldProjectTechnician(int id)
        {
            var projects = _contractDbContext.ProjectTechnicians.Where(e => e.ProjectId == id);
            _contractDbContext.ProjectTechnicians.RemoveRange(projects);
           
        }

    }
}
