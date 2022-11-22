using AutoMapper;
using ContractManagement.API.Grpc.StaticResource;
using ContractManagement.Domain.Commands.ContractFormCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories.ContractFormRepository;
using ContractManagement.Infrastructure.Repositories.PictureRepository;
using GenericRepository.Configurations;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.ContractFormCommandHandler
{
    public class CUContractFormCommandHandler : IRequestHandler<CUContractFormCommand, ActionResponse<ContractFormDTO>>
    {
        private readonly IContractFormRepository _contractFormRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IMapper _mapper;
        private readonly IStaticResourceService _staticResourceService;
        private readonly IPictureRepository _pictureRepository;

        public CUContractFormCommandHandler(IContractFormRepository contractFormRepository,
            IWrappedConfigAndMapper configAndMapper,
            IMapper mapper,
            IStaticResourceService staticResourceService, 
            IPictureRepository pictureRepository)
        {
            _contractFormRepository = contractFormRepository;
            _configAndMapper = configAndMapper;
            _mapper = mapper;
            _staticResourceService = staticResourceService;
            _pictureRepository = pictureRepository;
        }

        public async Task<ActionResponse<ContractFormDTO>> Handle(CUContractFormCommand request, CancellationToken cancellationToken)
        {
            var actionResponse = new ActionResponse<ContractFormDTO>();

            if (_contractFormRepository.CheckExitsName(request.Name, request.Id))
            {
                actionResponse.AddError("Mã mẫu hợp đồng đã tồn tại", nameof(request.Name));
                return actionResponse;
            }

            if (request.Id == 0)
            {
                if (request.DigitalSignature != null)
                {
                    var storedDigitalSignatureItem =
                        await _staticResourceService.PersistentImage(request.DigitalSignature.TemporaryUrl);
                    var addDigitalSignatureResponse = await _pictureRepository.CreateAndSave(storedDigitalSignatureItem);
                    if (!addDigitalSignatureResponse.IsSuccess)
                        throw new ContractDomainException(addDigitalSignatureResponse.Message);

                    request.DigitalSignatureId = addDigitalSignatureResponse.Result.Id;
                }

                var createdRsp = await _contractFormRepository.CreateAndSave(request);
                actionResponse.CombineResponse(createdRsp);
                actionResponse.SetResult(_mapper.Map<ContractFormDTO>(createdRsp.Result));
            }
            else
            {
                if (!_contractFormRepository.CheckExits(request.Id))
                {
                    throw new ContractDomainException("Mẫu hợp đồng không tồn tại");
                }

                if (request.DigitalSignature != null && !string.IsNullOrWhiteSpace(request.DigitalSignature.TemporaryUrl))
                {
                    var storedDigitalSignatureItem =
                        await _staticResourceService.PersistentImage(request.DigitalSignature.TemporaryUrl);
                    var addDigitalSignatureResponse = await _pictureRepository.CreateAndSave(storedDigitalSignatureItem);
                    if (!addDigitalSignatureResponse.IsSuccess)
                        throw new ContractDomainException(addDigitalSignatureResponse.Message);

                    request.DigitalSignatureId = addDigitalSignatureResponse.Result.Id;
                }

                var updatedRsp = await _contractFormRepository.UpdateAndSave(request);
                actionResponse.CombineResponse(updatedRsp);
                actionResponse.SetResult(_mapper.Map<ContractFormDTO>(updatedRsp.Result));
            }

            return actionResponse;
        }
    }
}
