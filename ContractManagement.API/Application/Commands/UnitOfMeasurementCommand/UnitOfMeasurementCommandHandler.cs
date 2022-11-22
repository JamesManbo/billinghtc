using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories.UnitOfMeasurementRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.UnitOfMeasurementCommand
{
    public class UnitOfMeasurementCommandHandler : IRequestHandler<CUUnitOfMeasurementCommand, ActionResponse<UnitOfMeasurementDTO>>
    {
        private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public UnitOfMeasurementCommandHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository, IWrappedConfigAndMapper configAndMapper)
        {
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<UnitOfMeasurementDTO>> Handle(CUUnitOfMeasurementCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<UnitOfMeasurementDTO>();
            UnitOfMeasurementDTO responseResult;
            
            if (_unitOfMeasurementRepository.CheckExitDescription(request.Description, request.Id))
            {
                commandResponse.AddError("Tên đơn vị đo lường đã tồn tại", nameof(request.Description));
                return commandResponse;
            }

            if ( _unitOfMeasurementRepository.CheckExitLabel(request.Label, request.Id))
            {
                commandResponse.AddError("Tên ký hiệu đơn vị đo lường đã tồn tại", nameof(request.Label));
                return commandResponse;
            }

            if (request.Id == 0)
            {
                var createResponse = await _unitOfMeasurementRepository.CreateAndSave(request);
                commandResponse.CombineResponse(createResponse);
                responseResult = createResponse.Result.MapTo<UnitOfMeasurementDTO>(this._configAndMapper.MapperConfig);
            }
            else
            {
                var updateResponse = await _unitOfMeasurementRepository.UpdateAndSave(request);
                commandResponse.CombineResponse(updateResponse);
                responseResult = updateResponse.Result.MapTo<UnitOfMeasurementDTO>(this._configAndMapper.MapperConfig);
            }

            commandResponse.SetResult(responseResult);

            return commandResponse;
        }
    }
}
