using ContractManagement.Domain.Commands.RadiusAndBrasCommand;
using ContractManagement.Domain.Models.RadiusAndBras;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.RadiusAndBrasRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.RadiusAndBrasCommandHandler
{
    public class CuRadiusServerInfoCommandHandler : IRequestHandler<CuRadiusServerInfoCommand, ActionResponse<RadiusServerInfoDTO>>
    {
        private readonly IRadiusServerInfoRepository _radiusServerInfoRepository;
        private readonly IRadiusServerInfoQueries _radiusServerInfoQueries;
        private readonly IWrappedConfigAndMapper _wrapperConfigAndMapper;

        public CuRadiusServerInfoCommandHandler(IRadiusServerInfoRepository radiusServerInfoRepository, 
            IRadiusServerInfoQueries radiusServerInfoQueries, 
            IWrappedConfigAndMapper wrapperConfigAndMapper)
        {
            _radiusServerInfoRepository = radiusServerInfoRepository;
            _radiusServerInfoQueries = radiusServerInfoQueries;
            _wrapperConfigAndMapper = wrapperConfigAndMapper;
        }

        public async Task<ActionResponse<RadiusServerInfoDTO>> Handle(CuRadiusServerInfoCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<RadiusServerInfoDTO>();

            if (request.Id == 0)
            {
                var insertResponse = await _radiusServerInfoRepository.CreateAndSave(request);
                commandResponse.CombineResponse(insertResponse);
                commandResponse.SetResult(insertResponse.Result.MapTo<RadiusServerInfoDTO>(_wrapperConfigAndMapper.MapperConfig));
            }
            else
            {
                var updateResponse = await _radiusServerInfoRepository.UpdateAndSave(request);
                commandResponse.CombineResponse(updateResponse);
                commandResponse.SetResult(updateResponse.Result.MapTo<RadiusServerInfoDTO>(_wrapperConfigAndMapper.MapperConfig));
            }

            return commandResponse;
        }
    }
}
