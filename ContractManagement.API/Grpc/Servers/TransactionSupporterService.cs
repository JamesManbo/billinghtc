using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Infrastructure.Queries;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Servers
{
    public class TransactionSupporterService : TransactionSupporterGrpc.TransactionSupporterGrpcBase
    {
        private readonly ITransactionQueries _transactionQueries;
        private readonly IMapper _mapper;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        public TransactionSupporterService(ITransactionQueries transactionQueries, IMapper mapper, IWrappedConfigAndMapper configAndMapper)
        {
            _transactionQueries = transactionQueries;
            _mapper = mapper;
            _configAndMapper = configAndMapper;
    }

        public override Task<TransactionSupporterPageListGrpcDTO> GetTransactionSupporterReport(TransactionSupporterFilterGrpc request, ServerCallContext context)
        {
            var result = new TransactionSupporterPageListGrpcDTO();
            var lstService = _transactionQueries.GetTransactionSupporterReport(request.UserIdentity,request.ProjectIds, _mapper.Map<DateTime>(request.StartDate), _mapper.Map < DateTime > (request.EndDate));
            foreach (var item in lstService.ToList())
            {
                result.TransactionSupporter.Add(item.MapTo<TransactionSupporterDataDTOGrpc>(_configAndMapper.MapperConfig));
            }
            
            return Task.FromResult(result);
        }
    }
}

