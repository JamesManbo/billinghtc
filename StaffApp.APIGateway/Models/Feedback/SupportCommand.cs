using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.Feedback
{
    public class SupportCommand
    {
        public string Id { get; set; }
        public int Status { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerIdentityGuid { get; set; }
        public int ContractorId { get; set; }
        public string CId { get; set; }
        public string ContractId { get; set; }
        public string ContractCode { get; set; }
        public int OutContractServicePackageId { get; set; }
        public string Service { get; set; }
        public string ServicePackage { get; set; }
        public string Address { get; set; }
        public string IPAddress { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public double? CustomerRate { get; set; }
        public string CustomerComment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime StartTime { get; set; }
        public bool ChargeReduction { get; set; }
        public bool Handled { get; set; }
        public string Source { get; set; }
        public string GlobalId { get; set; }
        public string CreatedBy { get; set; }
        public string ChannelText { get; set; }
    }
}
