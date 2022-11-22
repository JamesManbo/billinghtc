using AutoMapper;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using GenericRepository.Configurations;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.ContractCommandHandler
{
    public class CUContractorHTCCommandHandler : IRequestHandler<CUContractorHTCCommand, ActionResponse<ContractorDTO>>
    {
        private readonly IContractorRepository _contractorRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IMapper _mapper;

        public CUContractorHTCCommandHandler(IContractorRepository contractorRepository,
            IWrappedConfigAndMapper configAndMapper,
            IMapper mapper)
        {
            _contractorRepository = contractorRepository;
            _configAndMapper = configAndMapper;
            _mapper = mapper;
        }

        public async Task<ActionResponse<ContractorDTO>> Handle(CUContractorHTCCommand request, CancellationToken cancellationToken)
        {
            var actionResponse = new ActionResponse<ContractorDTO>();
            request.IsHTC = true;
            if (request.Id == 0)
            {
                if (_contractorRepository.CheckExitsCodeHTC(request.ContractorCode))
                {
                    actionResponse.AddError("Mã công ty đã tồn tại", nameof(request.ContractorCode));
                }

                if (_contractorRepository.CheckExitsPhoneHTC(request.ContractorPhone))
                {
                    actionResponse.AddError("Số điện thoại liên hệ đã tồn tại", nameof(request.ContractorPhone));
                }

                if (actionResponse.IsSuccess)
                {
                    var createdRsp = await _contractorRepository.CreateAndSave(request);
                    actionResponse.CombineResponse(createdRsp);
                    actionResponse.SetResult(_mapper.Map<ContractorDTO>(createdRsp.Result));
                }
            }
            else
            {
                if (!_contractorRepository.CheckExitsHTC(request.Id))
                {
                    throw new ContractDomainException("Công ty không tồn tại");
                }

                if (_contractorRepository.CheckExitsCodeHTC(request.ContractorCode, request.Id))
                {
                    actionResponse.AddError("Mã công ty đã tồn tại", nameof(request.ContractorCode));
                }

                if (_contractorRepository.CheckExitsPhoneHTC(request.ContractorPhone, request.Id))
                {
                    actionResponse.AddError("Số điện thoại liên hệ đã tồn tại", nameof(request.ContractorPhone));
                }


                if (actionResponse.IsSuccess)
                {
                    var updatedRsp = await _contractorRepository.UpdateAndSave(request);
                    actionResponse.CombineResponse(updatedRsp);
                    actionResponse.SetResult(_mapper.Map<ContractorDTO>(updatedRsp.Result));
                }
            }
            return actionResponse;
        }
    }
}
