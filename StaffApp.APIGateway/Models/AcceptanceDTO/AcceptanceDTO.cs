using StaffApp.APIGateway.Models.ContractModels;
using StaffApp.APIGateway.Models.TransactionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.AcceptanceDTO
{
    public class AcceptanceDTO: TransactionDTO
    {
        public ContractorDTO Contractor { get; set; }
        public string ContractCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormat { get { return CreatedDate.ToString("dd/MM/yyyy"); } }
    }
}
