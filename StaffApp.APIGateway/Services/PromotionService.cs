using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.PromotionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IPromotionService
    {
        Task<List<AvailablePromotionDTO>> GetAvailablePromotionForContract(int serviceId, int servicePackageId, int outContractServicePackageId);
    }
    public class PromotionService : GrpcCaller, IPromotionService
    {
        private readonly IMapper _mapper;
        public PromotionService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }
        public async Task<List<AvailablePromotionDTO>> GetAvailablePromotionForContract(int serviceId, int servicePackageId, int outContractServicePackageId)
        {
            return await Call<List<AvailablePromotionDTO>>(async channel =>
            {
                var client = new PromotionsServiceGrpc.PromotionsServiceGrpcClient(channel);
                var request = new  RequestGetPromotionsGrpc{ServiceId = serviceId, ServicePackageId = servicePackageId, OutContractServicePackageId = outContractServicePackageId};

                var lstArticleGrpc = await client.GetPromotionsAsync(request);

                return lstArticleGrpc.Promotions.ToList();
            });
        }
    }
}
