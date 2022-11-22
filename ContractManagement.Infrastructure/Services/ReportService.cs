using ContractManagement.Domain.FilterModels.ReportsModel;
using ContractManagement.Domain.Models.Reports;
using ContractManagement.Infrastructure.Queries;
using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Services
{
    public interface IReportService
    {
        List<OutContractInfoForReport> GetOutContractInfos(ReportFilterBase reportFilter);

    }
    public class ReportService : IReportService
    {
        private readonly IReportsQueries _reportsQueries;
        public ReportService(IReportsQueries reportsQueries)
        {
            _reportsQueries = reportsQueries;
        }
        public List<OutContractInfoForReport> GetOutContractInfos(ReportFilterBase reportFilter)
        {
            var outContractInfo = _reportsQueries.GetOutContractInfos(reportFilter);
            var contractCodes = outContractInfo.Select(e => e.Id);
            
            return new List<OutContractInfoForReport>();
        }
    }
}