using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Commands.ServicePackageCommand;
using ContractManagement.Domain.Commands.ServicePackagePriceCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories;
using ContractManagement.Infrastructure.Repositories.ServicePackagePriceRepository;
using GenericRepository.Configurations;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.API.Application.Commands.ServicePackageCommandHandler
{
    public class CUServicePackagePriceCommandHandler : IRequestHandler<CUServicePackagePriceCommand, ActionResponse<ServicePackagePriceDTO>>
    {
        private readonly IServicePackagePriceQueries _servicePackagePriceQueries;
        private readonly IServicePackagePriceRepository _packagePriceRepository;

        public CUServicePackagePriceCommandHandler(IServicePackagePriceRepository packageRepository,
            IServicePackagePriceQueries servicePackagePriceQueries)
        {
            _servicePackagePriceQueries = servicePackagePriceQueries;
            _packagePriceRepository = packageRepository;
        }
        public async Task<ActionResponse<ServicePackagePriceDTO>> Handle(CUServicePackagePriceCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<ServicePackagePriceDTO>();
            var priceModel = new ServicePackagePrice
            {
                ChannelId = request.ServicePackageId,
                PriceValue = request.PriceValue,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CurrencyUnitCode = request.CurrencyUnitCode
            };

            if (request.Id != 0)
            {
                priceModel.UpdatedBy = request.UpdatedBy;
                priceModel.UpdatedDate = DateTime.Now;
                commandResponse.CombineResponse(await _packagePriceRepository.UpdateAndSave(priceModel));
            }
            else
            {
                priceModel.CreatedBy = request.CreatedBy;
                commandResponse.CombineResponse(await _packagePriceRepository.CreateAndSave(priceModel));
            }
            return commandResponse;


        }
    }
}
