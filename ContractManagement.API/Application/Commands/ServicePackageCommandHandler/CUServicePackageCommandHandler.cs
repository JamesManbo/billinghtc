using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Commands.ServicePackageCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ServicePackagePriceRepository;
using ContractManagement.Infrastructure.Repositories.ServicePackageRepository;
using GenericRepository.Configurations;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.API.Application.Commands.ServicePackageCommandHandler
{
    public class CUServicePackageCommandHandler : IRequestHandler<CUServicePackageCommand, ActionResponse<ServicePackageDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IServicePackagesRepository _packageRepository;
        private readonly IServicePackagePriceRepository _packagePriceRepository;


        public CUServicePackageCommandHandler(IServicePackagesRepository packageRepository,
            IServicePackagePriceRepository packagePriceRepository,
            IMapper mapper)
        {
            _packageRepository = packageRepository;
            _packagePriceRepository = packagePriceRepository;
            this._mapper = mapper;
        }
        public async Task<ActionResponse<ServicePackageDTO>> Handle(CUServicePackageCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<ServicePackageDTO>();

            if (_packageRepository.CheckExitServicePackageCode(request.PackageCode, request.Id))
            {
                commandResponse.AddError("Mã gói cước đã tồn tại", nameof(request.PackageCode));
                return commandResponse;
            }

            //if (_packageRepository.CheckExitServicePackageName(request.PackageName, request.Id))
            //{
            //    commandResponse.AddError("Tên gói cước đã tồn tại ", nameof(request.PackageName));
            //    return commandResponse;
            //}

            if (request.Id == 0)
            {
                if (request.ParentId.HasValue && request.ParentId > 0)
                {
                    var parentPackageEntity = await _packageRepository.GetByIdAsync(request.ParentId);
                    if (parentPackageEntity.ParentId.HasValue)
                    {
                        commandResponse.AddError("Gói cước gốc đã là biến thể", nameof(request.ParentId));
                        return commandResponse;
                    }
                }
                var createdResponse = await _packageRepository.CreateAndSave(request);

                if (request.ListProjectPrice != null
                    && request.ListProjectPrice.Count > 0)
                {
                    int count = request.ListProjectPrice.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var p = request.ListProjectPrice.ElementAt(i);
                        var priceModel = new ServicePackagePrice
                        {
                            CreatedBy = request.CreateBy,
                            ChannelId = createdResponse.Result.Id,
                            PriceValue = p.PriceValue,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            IsActive = true,
                            CurrencyUnitCode = p.CurrencyUnitCode
                        };
                        await _packagePriceRepository.CreateAndSave(priceModel);
                    }
                }

                if (request.ListPriceByCurrencyUnit != null
                    && request.ListPriceByCurrencyUnit.Count > 0)
                {
                    int count = request.ListPriceByCurrencyUnit.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var p = request.ListPriceByCurrencyUnit.ElementAt(i);
                        var priceByCurrency = new ServicePackagePrice
                        {
                            CreatedBy = request.CreateBy,
                            ChannelId = createdResponse.Result.Id,
                            PriceValue = p.PriceValue,
                            CurrencyUnitCode = p.CurrencyUnitCode,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            IsActive = true,
                            IsDeleted = false
                        };
                        await _packagePriceRepository.CreateAndSave(priceByCurrency);
                    }
                }
                commandResponse.CombineResponse(createdResponse);
                commandResponse.SetResult(createdResponse.IsSuccess ? _mapper.Map<ServicePackageDTO>(createdResponse.Result) : default);
            }
            else
            {
                var srvPackageEntity = await _packageRepository.GetByIdAsync(request.Id);
                srvPackageEntity.Update(request);

                if (request.ListProjectPrice != null)
                {
                    foreach (var projectPrice in request.ListProjectPrice)
                    {
                        if (projectPrice.PriceValue == 0) continue;
                        projectPrice.ChannelId = request.Id;
                        projectPrice.IsActive = true;
                        if (projectPrice.Id == 0)
                        {
                            await _packagePriceRepository.CreateAndSave(projectPrice);
                        }
                        else
                        {
                            await _packagePriceRepository.UpdateAndSave(projectPrice);
                        }
                    }
                }

                if (request.ListDelProjectPrices != null)
                {
                    foreach (var projectPrice in request.ListDelProjectPrices)
                    {
                        await _packagePriceRepository.RemoveAndSave(projectPrice);
                    }
                }

                var updateResponse = await _packageRepository.UpdateAndSave(srvPackageEntity);
                commandResponse.CombineResponse(updateResponse);
                commandResponse.SetResult(updateResponse.IsSuccess ? _mapper.Map<ServicePackageDTO>(updateResponse.Result) : default);
            }

            return commandResponse;
        }
    }
}
