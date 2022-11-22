using ApplicationUserIdentity.API.Grpc.Clients;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.ContractDomainModels;
using AutoMapper;
using ContractManagement.API.Protos;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Services.GRPC.Clients
{
    public interface IContractorGrpcService
    {
        Task<IEnumerable<ContractorDTO>> GetAllByIds(int[] ids);
        Task<IEnumerable<ContractorDTO>> GetFromId(int id);
        Task<IPagedList<ContractorDTO>> GetListByMarketIdsProjectIds(RequestGetByMarketAreaIdsProjectIds req);
    }
    public class ContractorGrpcService : GrpcCaller, IContractorGrpcService
    {
        private readonly IMapper _mapper;
        public ContractorGrpcService(ILogger<ContractorGrpcService> logger,
            IMapper mapper) : base(logger, MicroserviceRouterConfig.GrpcContract)
        {
            this._mapper = mapper;
        }

        public async Task<IEnumerable<ContractorDTO>> GetAllByIds(int[] ids)
        {
            var grpcResponse = await CallAsync(async channel =>
            {
                var client = new ContractorGrpc.ContractorGrpcClient(channel);
                var contractors = await client.GetAllByIdsAsync(new StringValue()
                {
                    Value = string.Join(",", ids)
                });

                return contractors;
            });

            if (grpcResponse == null || grpcResponse.Contractors == null) return default;

            var result = new List<ContractorDTO>();
            foreach (var item in grpcResponse.Contractors)
            {
                result.Add(_mapper.Map<ContractorDTO>(item));
            }

            return result;
        }

        public async Task<IEnumerable<ContractorDTO>> GetFromId(int id)
        {
            var grpcResponse = await CallAsync(async channel =>
            {
                var client = new ContractorGrpc.ContractorGrpcClient(channel);
                var contractors = await client.GetFromIdAsync(new Int32Value()
                {
                    Value = id
                });

                return contractors;
            });

            if (grpcResponse == null || grpcResponse.Contractors == null) return default;

            return grpcResponse.Contractors.Select(_mapper.Map<ContractorDTO>);
        }

        public async Task<IPagedList<ContractorDTO>> GetListByMarketIdsProjectIds(RequestGetByMarketAreaIdsProjectIds req)
        {
            return await CallAsync<IPagedList<ContractorDTO>>(async channel =>
            {
                var client = new ContractorGrpc.ContractorGrpcClient(channel);
                var request = _mapper.Map<RequestGetByMarketAreaIdsProjectIdsGrpc>(req);

                var lstContractFormGrpc = await client.GetByMarketAreaIdsProjectIdsAsync(request);

                return _mapper.Map<IPagedList<ContractorDTO>>(lstContractFormGrpc);
            });
        }
    }
}
