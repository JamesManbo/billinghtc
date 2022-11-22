using AutoMapper;
using DebtManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StaffApp.APIGateway.Models.CuReceiptVoucherCommands;
using StaffApp.APIGateway.Models.ReceiptVoucherModels;
using StaffApp.APIGateway.Models.RequestModels;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IReceiptVoucherService
    {
        Task<IPagedList<ReceiptVoucherGridDTO>> GetList(ReceiptVoucherFilterModel filterModel);
        Task<ReceiptVoucherDTO> GetDetail(string id);
        Task<ActionResponse> Update(CuReceiptVoucherCommand receiptVoucher);
        Task<string> GetReceiveStatuses();
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
                //if (filterModel.IsOutOfDate) request.StatusIds = "8,9";//Nợ xấu, Quá hạn
                //else request.StatusIds = "1,2,3,4,5,6,7";

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

        public async Task<ActionResponse> Update(CuReceiptVoucherCommand updateReceiptVchrCommand)
        {
            return await Call<ActionResponse>(async channel =>
            {
                var actionResponse = new ActionResponse();
                var client = new ReceiptVoucherGrpc.ReceiptVoucherGrpcClient(channel);
                var grpcReceiptVchrCommand = JsonConvert.SerializeObject(updateReceiptVchrCommand);
                var response = await client.UpdateAsync(new StringValue() {
                    Value = grpcReceiptVchrCommand
                });

                if (response.IsSuccess.HasValue && response.IsSuccess.Value)
                {
                    actionResponse.Message = response.Message;
                }
                else
                {
                    foreach (var error in response.Errors)
                    {
                        actionResponse.AddError(error.ErrorMessage, error.MemberName);
                    }
                }

                return actionResponse;
            });
        }

        public async Task<string> GetReceiveStatuses()
        {
            return await Call<string>(async channel =>
            {
                var client = new ReceiptVoucherGrpc.ReceiptVoucherGrpcClient(channel);

                var resGrpc = await client.GetReceiptStatusAsync(new Empty());

                return resGrpc.Value;
            });
        }
    }
}
