using ContractManagement.Domain.Commands.CUCurrencyUnitCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories.CurrencyUnitRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.CurrencyUnitCommandHandler
{
    public class CreateCurrencyUnitCommandHandler : IRequestHandler<CreateCurrencyUnitCommand, ActionResponse<CurrencyUnitDTO>>
    {
        private readonly ICurrencyUnitRepository _currencyUnitRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        public CreateCurrencyUnitCommandHandler(ICurrencyUnitRepository currencyUnitRepository, IWrappedConfigAndMapper configAndMapper)
        {
            _currencyUnitRepository = currencyUnitRepository;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<CurrencyUnitDTO>> Handle(CreateCurrencyUnitCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<CurrencyUnitDTO>();
            if (!_currencyUnitRepository.CheckExistCurrencyUnitName(request.CurrencyUnitName.Trim(), 0))
            {
                commandResponse.AddError("Đơn vị tiền tệ đã tồn tại ", nameof(request.CurrencyUnitName));
                return commandResponse;
            }
            if (!_currencyUnitRepository.CheckExistCurrencyUnitCode(request.CurrencyUnitName.Trim(), 0))
            {
                commandResponse.AddError("Mã đơn vị tiền tệ đã tồn tại", nameof(request.CurrencyUnitCode));
                return commandResponse;
            }

            var createCurrencyUnitResp = await _currencyUnitRepository.CreateAndSave(request);
            commandResponse.CombineResponse(createCurrencyUnitResp);

            if (createCurrencyUnitResp.IsSuccess)
            {
                commandResponse.SetResult(createCurrencyUnitResp.Result.MapTo<CurrencyUnitDTO>(_configAndMapper.MapperConfig));
                return commandResponse;
            }

            return commandResponse;
        }
    }

    public class UpdateCurrencyUnitCommandHandler : IRequestHandler<UpdateCurrencyUnitCommand, ActionResponse<CurrencyUnitDTO>>
    {
        private readonly ICurrencyUnitRepository _currencyUnitRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        public UpdateCurrencyUnitCommandHandler(ICurrencyUnitRepository currencyUnitRepository, IWrappedConfigAndMapper configAndMapper)
        {
            _currencyUnitRepository = currencyUnitRepository;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<CurrencyUnitDTO>> Handle(UpdateCurrencyUnitCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<CurrencyUnitDTO>();

            if (!_currencyUnitRepository.CheckExistCurrencyUnitName(request.CurrencyUnitName.Trim(), request.Id))
            {
                commandResponse.AddError("Đơn vị tiền tệ đã tồn tại ", nameof(request.CurrencyUnitName));
                return commandResponse;
            }

            if (!_currencyUnitRepository.CheckExistCurrencyUnitCode(request.CurrencyUnitName.Trim(), request.Id))
            {
                commandResponse.AddError("Mã đơn vị tiền tệ đã tồn tại", nameof(request.CurrencyUnitCode));
                return commandResponse;
            }
            var updateCurrencyUnitResp = await _currencyUnitRepository.UpdateAndSave(request);
            commandResponse.CombineResponse(updateCurrencyUnitResp);

            if (updateCurrencyUnitResp.IsSuccess)
            {
                commandResponse.SetResult(updateCurrencyUnitResp.Result.MapTo<CurrencyUnitDTO>(_configAndMapper.MapperConfig));
                return commandResponse;
            }

            return commandResponse;
        }
    }
}
