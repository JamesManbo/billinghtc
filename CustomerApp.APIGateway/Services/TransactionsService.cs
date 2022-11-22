using AutoMapper;
using ContractManagement.API.Protos;
using CustomerApp.APIGateway.Models.TransactionModels;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Services
{
    public interface ITransactionsService
    {
        Task<IsSuccessDTO> AddNewTransaction(RequestAddNewTransaction request);
    }
    public class TransactionsService : GrpcCaller, ITransactionsService
    {
        private readonly IMapper _mapper;
        public TransactionsService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }
        public async Task<IsSuccessDTO> AddNewTransaction(RequestAddNewTransaction command)
        {
            return await Call<IsSuccessDTO>(async channel =>
            {
                var client = new TransactionsGrpc.TransactionsGrpcClient(channel);
                var request = _mapper.Map<RequestTransactionGRPC>(command);

                var isSuccess = await client.AddNewTransactionAsync(request);

                return _mapper.Map<IsSuccessDTO>(isSuccess);
            });
        }
    }
}
