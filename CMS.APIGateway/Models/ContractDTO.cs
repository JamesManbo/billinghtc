using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.APIGateway.Models
{
    public class ContractDTO
    {
        public DateTime SignedDate { get; set; }

        public AddressDTO Address { get; set; }
        public int ContractStatus { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public int? ContractorId { get; set; }
        public string ContractorFullName { get; set; }
        public string SalesmanIdentityGuid { get; set; }
        public int? SalesmanId { get; set; }
        public int? PaymentMethodId { get; set; }
        public string Description { get; set; }
    }
}
