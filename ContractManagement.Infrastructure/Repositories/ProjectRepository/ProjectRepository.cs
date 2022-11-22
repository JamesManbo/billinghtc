using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using ContractManagement.Domain.Models.OutContracts;
using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ProjectRepository
{
    public class ProjectRepository : CrudRepository<Project, int>, IProjectRepository
    {
        private readonly ContractDbContext _contractDbContext;
        public ProjectRepository(ContractDbContext contractDbContext, IWrappedConfigAndMapper configAndMapper) : base(contractDbContext, configAndMapper)
        {
            _contractDbContext = contractDbContext;
        }

        public bool CheckExitProjectName(string nameProject, int id)
        {
            return _contractDbContext.Projects
                .Any(x => x.ProjectName == nameProject.Trim() && x.IsDeleted == false && x.Id != id);
        }

        public bool CheckExitProjectCode(string codeName, int id)
        {
            return _contractDbContext.Projects
                .Any(x => x.ProjectCode == codeName.Trim() && x.IsDeleted == false && x.Id != id);
        }

        public bool CheckExitProjectName(string nameProject)
        {
            return _contractDbContext.Projects.Any(x => x.ProjectName == nameProject.Trim() && x.IsDeleted == false);
        }

        public int GetProjectId(string nameProject)
        {
            return _contractDbContext.Projects
                .Where(x => x.ProjectName.Contains(nameProject) && x.IsDeleted == false && x.IsActive == true).Select(y => y.Id)
                .FirstOrDefault();
        }
    }
}
