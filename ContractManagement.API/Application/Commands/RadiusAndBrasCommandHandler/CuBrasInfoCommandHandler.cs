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
    public class CuBrasInfoCommandHandler : IRequestHandler<CuBrasInfoCommand, ActionResponse<BrasInfoDTO>>
    {
        private readonly IBrasInfoRepository _brasInfoRepository;
        private readonly IBrasInfoQueries _brasInfoQueries;
        private readonly IWrappedConfigAndMapper _wrapperConfigAndMapper;

        public CuBrasInfoCommandHandler(IBrasInfoRepository brasInfoRepository, 
            IBrasInfoQueries brasInfoQueries, 
            IWrappedConfigAndMapper wrapperConfigAndMapper)
        {
            _brasInfoRepository = brasInfoRepository;
            _brasInfoQueries = brasInfoQueries;
            _wrapperConfigAndMapper = wrapperConfigAndMapper;
        }

        public async Task<ActionResponse<BrasInfoDTO>> Handle(CuBrasInfoCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<BrasInfoDTO>();

            if (request.Id == 0)
            {
                var insertResponse = await _brasInfoRepository.CreateAndSave(request);
                commandResponse.CombineResponse(insertResponse);
                commandResponse.SetResult(insertResponse.Result.MapTo<BrasInfoDTO>(_wrapperConfigAndMapper.MapperConfig));
            }
            else
            {
                var updateResponse = await _brasInfoRepository.UpdateAndSave(request);
                commandResponse.CombineResponse(updateResponse);
                commandResponse.SetResult(updateResponse.Result.MapTo<BrasInfoDTO>(_wrapperConfigAndMapper.MapperConfig));
            }

            return commandResponse;
        }
    }
}
