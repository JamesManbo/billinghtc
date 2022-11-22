using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Servers
{
    public class PromotionService : PromotionsServiceGrpc.PromotionsServiceGrpcBase
    {
        private readonly IPromotionQueries _promotionQueries;
        private readonly IMapper _mapper;
        public PromotionService(IPromotionQueries promotionQueries, IMapper mapper)
        {
            _promotionQueries = promotionQueries;
            _mapper = mapper;
        }
        public override Task<ListPromotionDTOGrpc> GetPromotions(RequestGetPromotionsGrpc request, ServerCallContext context)
        {
            var rs = new ListPromotionDTOGrpc();
            if (!request.ServiceId.HasValue || !request.ServicePackageId.HasValue || !request.OutContractServicePackageId.HasValue) {
                return Task.FromResult(rs);
            }
            // var promotions = _promotionQueries.GetAvailablePromotionForContract(request.ServiceId.Value, request.ServicePackageId.Value, request.OutContractServicePackageId.Value);
            var promotionFiler = new AvailablePromotionModelFilter();
            promotionFiler.ServiceIds.Append(request.ServiceId.Value);
            promotionFiler.ServicePackageIds.Append(request.ServicePackageId.Value);
            promotionFiler.OutContractServicePackageIds.Append(request.OutContractServicePackageId.Value);
            var promotions = _promotionQueries.GetAvailablePromotionForContract(promotionFiler);
            
            for (int i = 0; i < promotions.Count(); i++)
            {
                rs.Promotions.Add(_mapper.Map<PromotionDTOGrpc>(promotions.ElementAt(i)));
            }

            return Task.FromResult(rs);
        }
    }
}
