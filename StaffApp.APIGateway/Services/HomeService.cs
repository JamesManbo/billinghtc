using AutoMapper;
using DebtManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.ReceiptVoucherModels;
using StaffApp.APIGateway.Models.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IHomeService
    {
        Task<List<CollectedVoucherDTO>> GetList(CollectedVoucherFilterModel filterModel);
    }

    public class HomeService : GrpcCaller, IHomeService
    {
        private readonly IMapper _mapper;
        public HomeService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcDebt)
        {
            _mapper = mapper;
        }

        public async Task<List<CollectedVoucherDTO>> GetList(CollectedVoucherFilterModel filterModel)
        {
            return await Call<List<CollectedVoucherDTO>>(async channel =>
            {
                var client = new CollectedVoucherGrpc.CollectedVoucherGrpcClient(channel);
                var request = _mapper.Map<CollectedVoucherFilterGrpc>(filterModel);

                var lstServiceGrpc = await client.GetCollectedAndUnCollectedVoucherByMonthAsync(request);

                return lstServiceGrpc.CollectedVoucher;
            });
        }
    }
}
