using DebtManagement.API.Grpc.Clients;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Services
{
    public interface IReceiptVoucherService
    {
        Task<string> GenerateReceiptVoucherCode(DateTime issuedDate, int? projectId, int? marketAreaId, bool isEnterprise = false, int? idxByDay = null);
    }

    public class ReceiptVoucherService : IReceiptVoucherService
    {
        private readonly IProjectGrpcService _projectGrpcService;
        private readonly IMarketAreaGrpcService _marketAreaGrpcService;
        private readonly IReceiptVoucherQueries _receiptVoucherQueries;
        private readonly IReceiptVoucherRepository receiptVoucherRepository;

        public ReceiptVoucherService(IProjectGrpcService projectGrpcService,
            IMarketAreaGrpcService marketAreaGrpcService,
            IReceiptVoucherQueries receiptVoucherQueries)
        {
            _projectGrpcService = projectGrpcService;
            _marketAreaGrpcService = marketAreaGrpcService;
            _receiptVoucherQueries = receiptVoucherQueries;
        }

        public async Task<string> GenerateReceiptVoucherCode(DateTime issuedDate, int? projectId, int? marketAreaId, bool isEnterprise, int? idxByDay = null)
        {
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

            return _receiptVoucherQueries.GetReceiptVoucherCode(issuedDate, projectCode, marketAreaCode, isEnterprise, idxByDay);
        }
    }
}
