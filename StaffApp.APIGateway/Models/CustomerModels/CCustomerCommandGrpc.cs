using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.CustomerModels
{
    public class CCustomerCommandGrpc
    {
        public CCustomerCommandGrpc()
        {
           
        }

        public int Id { get; set; }
        public Guid IdentityGuid { get; set; }
        public string CustomerCode { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MobilePhoneNo { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string IdNo { get; set; }
        public string ProvinceIdentityGuid { get; set; }
        public string Province { get; set; }
        public string DistrictIdentityGuid { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public string CountryIdentityGuid { get; set; }
        public DateTime? IdDateOfIssue { get; set; }
        public string IdIssuedBy { get; set; }
        public int? Gender { get; set; }
        public string SecurityStamp { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string CustomerCategoryCode { get; set; }
        public int? CustomerCategoryId { get; set; }
        public int? CustomerTypeId { get; set; }
        public int? CustomerStructureId { get; set; }
        public List<int> IndustryIds { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankBranch { get; set; }
        public int? ClassId { get; set; }
        public string GroupIdsStr { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool isSuccess { get; set; }
        public string Message { get; set; }
    }
}
