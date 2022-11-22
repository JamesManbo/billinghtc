using AutoMapper;
using DebtManagement.API.Protos;
using DebtManagement.Domain.Models.FilterModels;
using DebtManagement.Infrastructure.Queries;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Grpc.Servers
{
    public interface ICollectedVoucherGrpcService
    {
        Task<CollectedVoucherPageListGrpcDTO> GetCollectedAndUnCollectedVoucherByMonth(CollectedVoucherFilterGrpc request, ServerCallContext context);
    }
    public class CollectedVoucherGrpcService : CollectedVoucherGrpc.CollectedVoucherGrpcBase, ICollectedVoucherGrpcService
    {
        private readonly IReceiptVoucherQueries _receiptVoucherQueries;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IMapper _mapper;
        public CollectedVoucherGrpcService(IReceiptVoucherQueries receiptVoucherQueries, IMapper mapper, IWrappedConfigAndMapper configAndMapper)
        {
            _receiptVoucherQueries = receiptVoucherQueries;
            _mapper = mapper;
            _configAndMapper = configAndMapper;
        }
        public override Task<CollectedVoucherPageListGrpcDTO> GetCollectedAndUnCollectedVoucherByMonth(CollectedVoucherFilterGrpc request, ServerCallContext context)
        {
            var result = new CollectedVoucherPageListGrpcDTO();
            var lstService = _receiptVoucherQueries.GetCollectedAndUnCollectedVoucherByMonth(_mapper.Map<CollectedVoucherFilter>(request));
            foreach (var item in lstService.ToList())
            {
                result.CollectedVoucher.Add(item.MapTo<CollectedVoucherDataDTOGrpc>(_configAndMapper.MapperConfig));
            }
            return Task.FromResult(result);  
        }
    }
   
}
