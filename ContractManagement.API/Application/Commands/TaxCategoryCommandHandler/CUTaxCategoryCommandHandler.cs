using System.Threading;
using System.Threading.Tasks;
using ContractManagement.Domain.Commands.TaxCategoryCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories.TaxCatagoriesRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.API.Application.Commands.TaxCategoryCommandHandler
{
    public class CUTaxCategoryCommandHandler : IRequestHandler<CUTaxCategoryCommand, ActionResponse<TaxCategoryDTO>>
    {
        private readonly ITaxCategoryRepositoty _taxRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public CUTaxCategoryCommandHandler(ITaxCategoryRepositoty taxRepository, IWrappedConfigAndMapper configAndMapper)
        {
            _taxRepository = taxRepository;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<TaxCategoryDTO>> Handle(CUTaxCategoryCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<TaxCategoryDTO>();

            if (_taxRepository.CheckExistTaxName(request.TaxName, request.Id))
            {
                commandResponse.AddError("Tên loại thuế đã tồn tại", nameof(request.TaxName));
                return commandResponse;
            }
            if (_taxRepository.CheckExistTaxCode(request.TaxCode, request.Id))
            {
                commandResponse.AddError("Mã loại thuế đã tồn tại", nameof(request.TaxCode));
                return commandResponse;
            }

            if (request.Id == 0)
            {
                var insertResponse = await _taxRepository.CreateAndSave(request);
                commandResponse.SetResult(insertResponse.Result.MapTo<TaxCategoryDTO>(_configAndMapper.MapperConfig));
                commandResponse.CombineResponse(insertResponse);
            }
            else
            {
                var updateResponse = await _taxRepository.UpdateAndSave(request);
                commandResponse.SetResult(updateResponse.Result.MapTo<TaxCategoryDTO>(_configAndMapper.MapperConfig));
                commandResponse.CombineResponse(updateResponse);
            }

            return commandResponse;
        }
    }
}
