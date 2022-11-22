using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StaffApp.APIGateway.Models.TransactionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface ITransactionsService
    {
        Task<IsSuccessDTO> AddNewTransaction(RequestAddNewTransaction request );
        Task<IsSuccessDTO> AcceptanceTransaction(AcceptanceTransactionCommand request);
        Task<TransactionDTO> GetDetail(int id);
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

        public async Task<TransactionDTO> GetDetail(int id)
        {
            return await Call<TransactionDTO>(async channel =>
            {
                var client = new TransactionsGrpc.TransactionsGrpcClient(channel);

                var rs = await client.GetDetailAsync(new Int32Value() { Value = id });
                if (rs.IsSuccess) return JObject.Parse(rs.DataJson).ToObject<TransactionDTO>();
                return null;
            });
        }

        public async Task<IsSuccessDTO> AcceptanceTransaction(AcceptanceTransactionCommand request)
        {
            return await Call<IsSuccessDTO>(async channel =>
            {
                var client = new TransactionsGrpc.TransactionsGrpcClient(channel);
                var isSuccess = await client.AcceptanceTransactionAppAsync(_mapper.Map<AcceptanceTransactionCommandAppGrpc>(request));

                return _mapper.Map<IsSuccessDTO>(isSuccess);
            });
        }
    }
}
