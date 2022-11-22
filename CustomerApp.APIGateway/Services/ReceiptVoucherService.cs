using AutoMapper;
using CustomerApp.APIGateway.Models.ReceiptVoucherModels;
using CustomerApp.APIGateway.Models.RequestModels;
using CustomerApp.APIGateway.Services;
using DebtManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Services
{
    public interface IReceiptVoucherService
    {
        Task<IPagedList<ReceiptVoucherGridDTO>> GetList(ReceiptVoucherFilterModel filterModel);
        Task<ReceiptVoucherDTO> GetDetail(string id);
    }
    public class ReceiptVoucherService : GrpcCaller, IReceiptVoucherService
    {
        private readonly IMapper _mapper;
        public ReceiptVoucherService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcDebt)
        {
            _mapper = mapper;
        }

        public async Task<IPagedList<ReceiptVoucherGridDTO>> GetList(ReceiptVoucherFilterModel filterModel)
        {
            return await Call<IPagedList<ReceiptVoucherGridDTO>>(async channel =>
            {
                var client = new ReceiptVoucherGrpc.ReceiptVoucherGrpcClient(channel);
                var request = _mapper.Map<ReceiptVoucherFilterGrpc>(filterModel);

                var lstServiceGrpc = await client.GetReceiptVouchersAsync(request);               

                return _mapper.Map<IPagedList<ReceiptVoucherGridDTO>>(lstServiceGrpc);
            });
        }

        public async Task<ReceiptVoucherDTO> GetDetail(string id)
        {
            return await Call<ReceiptVoucherDTO>(async channel =>
            {
                var client = new ReceiptVoucherGrpc.ReceiptVoucherGrpcClient(channel);                

                return await client.GetReceiptVoucherDetailAsync(new StringValue() { Value = id });
            });
        }
    }
}
