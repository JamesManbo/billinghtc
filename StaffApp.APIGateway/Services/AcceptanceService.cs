using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StaffApp.APIGateway.Models.AcceptanceDTO;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Models.TransactionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface IAcceptanceService
    {
        Task<IPagedList<AcceptanceDTO>> GetList(AcceptanceFilterModel filterModel);
        Task<TransactionDTO> GetDetail(int id);
    }
    public class AcceptanceService : GrpcCaller, IAcceptanceService
    {
        private readonly IMapper _mapper;
        public AcceptanceService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<IPagedList<AcceptanceDTO>> GetList(AcceptanceFilterModel filterModel)
        {

            return await Call<IPagedList<AcceptanceDTO>>(async channel =>
            {
                var client = new AcceptanceGrpc.AcceptanceGrpcClient(channel);
                var request = _mapper.Map<RequestGetAcceptancesGrpc>(filterModel);

                var lstArticleGrpc = await client.GetAcceptancesAsync(request);

                return _mapper.Map<IPagedList<AcceptanceDTO>>(lstArticleGrpc);
            });
        }

        public async Task<TransactionDTO> GetDetail(int id)
        {
            return await Call<TransactionDTO>(async channel =>
            {
                var client = new AcceptanceGrpc.AcceptanceGrpcClient(channel);

                var transactionGrpc = await client.GetDetailAsync(new Int32Value() { Value = id});
                return _mapper.Map<TransactionDTO>(transactionGrpc);
            });
        }
    }
}
