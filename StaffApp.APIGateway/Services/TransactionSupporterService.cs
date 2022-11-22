using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Models.TransactionsModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface ITransactionSupporterService
    {
        Task<IEnumerable<TransactionSupporterDTO>> GetTransactionSupporterReport(TransactionSupporterFilterModel filterModel);
    }
    public class TransactionSupporterService : GrpcCaller, ITransactionSupporterService
    {
        private readonly IMapper _mapper;
        public TransactionSupporterService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }
        public async Task<IEnumerable<TransactionSupporterDTO>> GetTransactionSupporterReport(TransactionSupporterFilterModel filterModel)
        {
            return await Call<List<TransactionSupporterDTO>>(async channel =>
            {
                var client = new TransactionSupporterGrpc.TransactionSupporterGrpcClient(channel);
                var request = _mapper.Map<TransactionSupporterFilterGrpc>(filterModel);

                var lstServiceGrpc = await client.GetTransactionSupporterReportAsync(request);

                return lstServiceGrpc.TransactionSupporter;
            });
        }
    }
}
