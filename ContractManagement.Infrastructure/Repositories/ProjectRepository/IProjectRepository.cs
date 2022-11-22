using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using ContractManagement.Domain.Models.OutContracts;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Infrastructure.Repositories.ProjectRepository
{
    public interface IProjectRepository : ICrudRepository<Project, int>
    {
        bool CheckExitProjectName(string nameProject, int id = 0);
        bool CheckExitProjectCode(string codeName, int id = 0);
        bool CheckExitProjectName(string nameProject);
        int GetProjectId(string nameProject);
    }
}
