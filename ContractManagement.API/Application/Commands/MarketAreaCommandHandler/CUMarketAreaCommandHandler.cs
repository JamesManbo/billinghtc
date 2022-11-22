using System;
using System.Threading;
using System.Threading.Tasks;
using ContractManagement.Domain.Commands.MarketAreaCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories.MarketAreaRespository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.API.Application.Commands.MarketAreaCommandHandler
{
    public class CUMarketAreaCommandHandler : IRequestHandler<CUMarketAreaCommand, ActionResponse<MarketAreaDTO>>
    {
        private readonly IMarketAreaRepository _marketAreaRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public CUMarketAreaCommandHandler(IMarketAreaRepository marketAreaRepository, IWrappedConfigAndMapper configAndMapper)
        {
            _marketAreaRepository = marketAreaRepository;
            _configAndMapper = configAndMapper;
        }
        public async Task<ActionResponse<MarketAreaDTO>> Handle(CUMarketAreaCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<MarketAreaDTO>();
            if (_marketAreaRepository.CheckExistMarketAreaName(request.MarketName, request.Id))
            {
                commandResponse.AddError("Tên vùng thị trường đã tồn tại ", nameof(request.MarketName));
                return commandResponse;
            }

            if (_marketAreaRepository.CheckExistMarketAreaCode(request.MarketCode, request.Id))
            {
                commandResponse.AddError("Mã vùng thị trường đã tồn tại", nameof(request.MarketCode));
                return commandResponse;
            }

            if (!request.ParentId.HasValue || request.ParentId <= 0)
            {
                request.TreeLevel = 0;
                request.TreePath = "";
            }
            else
            {
                var parent = await _marketAreaRepository.GetByIdAsync(request.ParentId);
                request.TreeLevel = parent.TreeLevel + 1;
                request.TreePath = parent.TreePath + "/" + parent.MarketCode;
            }

            if (request.Id == 0)
            {
                request.CreatedDate = DateTime.Now;
                var insertResponse = await _marketAreaRepository.CreateAndSave(request);
                commandResponse.CombineResponse(insertResponse);
                commandResponse.SetResult(insertResponse.Result.MapTo<MarketAreaDTO>(_configAndMapper.MapperConfig));
            }
            else
            {
                request.UpdatedDate = DateTime.Now;
                var updateResponse = await _marketAreaRepository.UpdateAndSave(request);
                commandResponse.CombineResponse(updateResponse);
                commandResponse.SetResult(updateResponse.Result.MapTo<MarketAreaDTO>(_configAndMapper.MapperConfig));
            }

            return commandResponse;
        }
    }
}
