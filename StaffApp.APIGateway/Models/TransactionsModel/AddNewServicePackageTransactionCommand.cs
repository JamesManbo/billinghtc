using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class AddNewServicePackageTransactionCommand
    {
        public List<CUTransactionServicePackage> TransactionServicePackages { get; set; }
        public string Note { get; set; }
        public int OutContractId { get; set; }
        public string ContractCode { get; set; }
        public int ContractType { get; set; }
        public int StatusId { get; set; }
        public string Code { get; set; }
        public string HandleUserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int Type { get; set; }
        public int? ContractorId { get; set; }
        public bool? IsAppendix { get; set; }
    }
}
