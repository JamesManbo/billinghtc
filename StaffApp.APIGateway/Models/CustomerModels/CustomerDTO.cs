using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.CustomerModels
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public Guid IdentityGuid { get; set; }
        public string AccountingCustomerCode { get; set; }
        public string CustomerCode { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int? Gender { get; set; }
        public string MobilePhoneNo { get; set; }
        public string FaxNo { get; set; }
        public string SecurityStamp { get; set; }
        public string IdNo { get; set; }
        public DateTime? IdDateOfIssue { get; set; }
        public string IdIssuedBy { get; set; }
        public string TaxIdNo { get; set; }
        public string RepresentativePersonName { get; set; }
        public string RpPhoneNo { get; set; }
        public DateTime? RpDateOfBirth { get; set; }
        public string RpJobPosition { get; set; }
        public string BusinessRegCertificate { get; set; }
        public DateTime? BrcDateOfIssue { get; set; }
        public string BrcIssuedBy { get; set; }
        public string Address { get; set; }
        public int? CustomerCategoryId { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? CustomerStructureId { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankBranch { get; set; }
        public bool IsEnterprise { get; set; }
        public IEnumerable<string> ContractCodes { get; set; }
    }
}
