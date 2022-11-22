using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using GenericRepository.Configurations;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.API.Application.Commands.ContractCommandHandler
{
    public class CUContractorCommandHandler : IRequestHandler<CUContractorCommand, ActionResponse<ContractorDTO>>
    {
        private readonly IContractorRepository _contractorRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IMapper _mapper;

        public CUContractorCommandHandler(IContractorRepository contractorRepository,
            IWrappedConfigAndMapper configAndMapper,
            IMapper mapper)
        {
            _contractorRepository = contractorRepository;
            _configAndMapper = configAndMapper;
            _mapper = mapper;
        }

        public async Task<ActionResponse<ContractorDTO>> Handle(CUContractorCommand request, CancellationToken cancellationToken)
        {
            var actionResponse = new ActionResponse<ContractorDTO>();
            //request.IdentityGuid = Guid.NewGuid().ToString();
            var createdRsp = await _contractorRepository.CreateAndSave(request);
            actionResponse.CombineResponse(createdRsp);
            actionResponse.SetResult(_mapper.Map<ContractorDTO>(createdRsp.Result));
            return actionResponse;
        }
    }
}
