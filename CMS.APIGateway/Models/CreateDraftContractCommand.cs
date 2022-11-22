using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.APIGateway.Models
{
    public class CreateDraftContractCommand
    {
        public DateTime OrderDate { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public int? ContractorId { get; set; }
        public string ContractorFullName { get; set; }
        public string SalesmanIdentityGuid { get; set; }
        public int? SalesmanId { get; set; }
        public int? PaymentMethodId { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}
