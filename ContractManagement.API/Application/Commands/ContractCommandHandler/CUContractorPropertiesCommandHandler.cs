using AutoMapper;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using GenericRepository.Configurations;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using ContractManagement.Domain.Commands.OutContractCommand;

namespace ContractManagement.API.Application.Commands.ContractCommandHandler
{
    public class CUContractorPropertiesCommandHandler : IRequestHandler<CUContractorPropertiesCommand, ActionResponse<ContractorPropertiesDTO>>
    {
        private readonly IContractorPropertiesRepository _contractorPropertiesRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IMapper _mapper;

        public CUContractorPropertiesCommandHandler(IContractorPropertiesRepository contractorPropertiesRepository,
            IWrappedConfigAndMapper configAndMapper,
            IMapper mapper)
        {
            _contractorPropertiesRepository = contractorPropertiesRepository;
            _configAndMapper = configAndMapper;
            _mapper = mapper;
        }

        public async Task<ActionResponse<ContractorPropertiesDTO>> Handle(CUContractorPropertiesCommand request, CancellationToken cancellationToken)
        {
            var actionResponse = new ActionResponse<ContractorPropertiesDTO>();
            var createdRsp = await _contractorPropertiesRepository.CreateAndSave(request);
            actionResponse.CombineResponse(createdRsp);
            actionResponse.SetResult(_mapper.Map<ContractorPropertiesDTO>(createdRsp.Result));
            return actionResponse;
        }
    }
}
