using System;
using System.Threading;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.ProjectAggregate;
using ContractManagement.Domain.Commands.ProjectCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories.ProjectRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.API.Application.Commands.ProjectCommandHandler
{
    public class CUProjectCommandHandler : IRequestHandler<CUProjectCommand, ActionResponse<ProjectDTO>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectTechnicianRepository _projectTechnicianRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public CUProjectCommandHandler(IProjectRepository projectRepository,
                                IWrappedConfigAndMapper configAndMapper, 
                                IProjectTechnicianRepository projectTechnicianRepository)
        {
            _projectRepository = projectRepository;
            _configAndMapper = configAndMapper;
            _projectTechnicianRepository = projectTechnicianRepository;
        }

        public async Task<ActionResponse<ProjectDTO>> Handle(CUProjectCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<ProjectDTO>();

            if (_projectRepository.CheckExitProjectName(request.ProjectName, request.Id))
            {
                commandResponse.AddError("Tên dự án đã tồn tại ", nameof(request.ProjectName));
                return commandResponse;
            }

            if (_projectRepository.CheckExitProjectCode(request.ProjectCode, request.Id))
            {
                commandResponse.AddError("Mã dự án đã tồn tại", nameof(request.ProjectCode));
                return commandResponse;
            }

            if (request.Id == 0)
            {
                request.IsActive = true;
                request.IdentityGuid = Guid.NewGuid().ToString();
                var insertResponse = await _projectRepository.CreateAndSave(request);
                if (request.Technicians.Count > 0)
                {
                    foreach (var tech in request.Technicians)
                    {
                        var projectTechnician = new ProjectTechnician()
                        {
                            UserTechnicianId = tech.Identity,
                            TechnicianName = tech.FullName,
                            ProjectId = insertResponse.Result.Id,
                            IsActive = true,
                            IsDeleted = false
                        };
                        await _projectTechnicianRepository.CreateAndSave(projectTechnician);
                    }                    
                }
                commandResponse.CombineResponse(insertResponse);
                commandResponse.SetResult(insertResponse.Result.MapTo<ProjectDTO>(_configAndMapper.MapperConfig));
            }
            else
            {
                var updateResponse = await _projectRepository.UpdateAndSave(request);
                _projectTechnicianRepository.RemoveOldProjectTechnician(request.Id);
                if (request.Technicians.Count > 0)
                {
                    foreach (var tech in request.Technicians)
                    {                       
                        var projectTechnician = new ProjectTechnician()
                        {
                            UserTechnicianId = tech.Identity,
                            TechnicianName = tech.FullName,
                            ProjectId = request.Id,
                            IsActive = true,
                            IsDeleted = false
                        };
                        await _projectTechnicianRepository.CreateAndSave(projectTechnician);                        
                    }
                }
                commandResponse.CombineResponse(updateResponse);
                commandResponse.SetResult(updateResponse.Result.MapTo<ProjectDTO>(_configAndMapper.MapperConfig));
            }
            return commandResponse;
        }
    }
}
