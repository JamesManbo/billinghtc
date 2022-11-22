using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class TransactionPromotionForContact
    {
        public int TransactionServicePackageId { get; set; }
        public int? OutContractServicePackageId { get; set; }
        public int PromotionDetailId { get; set; }
        public int PromotionType { get; set; }
        public int PromotionValue { get; set; }
        public bool IsApplied { get; set; }
    }
}
