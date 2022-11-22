using DebtManagement.API.Grpc.Clients;
using DebtManagement.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Services
{
    public interface IPaymentVoucherService
    {
        Task<string> GeneratePaymentVoucherCode(int? projectId, int? marketAreaId);
    }
    public class PaymentVoucherService : IPaymentVoucherService
    {
        private readonly IProjectGrpcService _projectGrpcService;
        private readonly IMarketAreaGrpcService _marketAreaGrpcService;
        private readonly IPaymentVoucherQueries _paymentVoucherQueries;

        public PaymentVoucherService(IProjectGrpcService projectGrpcService,
            IMarketAreaGrpcService marketAreaGrpcService,
            IPaymentVoucherQueries paymentVoucherQueries)
        {
            _projectGrpcService = projectGrpcService;
            _marketAreaGrpcService = marketAreaGrpcService;
            _paymentVoucherQueries = paymentVoucherQueries;
        }

        public async Task<string> GeneratePaymentVoucherCode(int? projectId, int? marketAreaId)
        {
            var dateTimeLocal = DateTime.UtcNow.AddHours(7);
            int voucherLatestIndex = _paymentVoucherQueries.GetOrderNumberByNow();
            string indexAsString = voucherLatestIndex.ToString().PadLeft(2, '0');

            string projectCode = string.Empty;
            if (projectId.HasValue)
            {
                projectCode = await _projectGrpcService.GetProjectCode(projectId.Value);
            }

            string marketAreaCode = string.Empty;
            if (marketAreaId.HasValue)
            {
                marketAreaCode = await _marketAreaGrpcService.GetMarketAreaCode(marketAreaId.Value);
            }

            string projectPart = !string.IsNullOrEmpty(projectCode) ? projectCode + "/" : "";
            string marketAreaPart = !string.IsNullOrEmpty(marketAreaCode) ? marketAreaCode + "/" : "";

            return $"DNTT-{ marketAreaPart.ToUpper()}{ projectPart.ToUpper()}{dateTimeLocal.ToString("yyMMdd")}/{ indexAsString}";
        }
    }
}
