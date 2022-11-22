using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Infrastructure.Services;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Servers
{
    public class ExchangeRateGrpcService : ExchangeRateGrpc.ExchangeRateGrpcBase
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IMapper _mapper;
        public ExchangeRateGrpcService(IExchangeRateService exchangeRateService, IMapper mapper)
        {
            _exchangeRateService = exchangeRateService;
            _mapper = mapper;
        }

        public override Task<ExchangeRateGrpcResult> ExchangeRate(RequestExchangeRateGrpc request, ServerCallContext context)
        {
            var rs = _exchangeRateService.ExchangeRate(request.FromCode, request.ToCode);

            if (rs.HasValue)
                return Task.FromResult(new ExchangeRateGrpcResult { IsSuccess = rs.HasValue, Value = rs.Value });
            else            
                return Task.FromResult(new ExchangeRateGrpcResult { IsSuccess = rs.HasValue });
        }

        public override Task<ListExchangeRateGrpcDTO> GetExchangeRates(Empty request, ServerCallContext context)
        {
            var lstER = _exchangeRateService.GetAllExchangeRate();
            var result = new ListExchangeRateGrpcDTO();

            for (int i = 0; i < lstER.Count; i++)
            {
                result.Subset.Add(_mapper.Map<ExchangeRateDTOGrpc>(lstER.ElementAt(i)));
            }

            return Task.FromResult(result);
        }

        public override async Task<BoolValue> Synchronize(Empty request, ServerCallContext context)
        {
            return new BoolValue() {
                Value = await _exchangeRateService.SynchronizeExchangeRates()
            };
        }
    }
}
