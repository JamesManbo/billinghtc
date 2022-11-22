using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace CMS.APIGateway.Models.FeedbackAndRequest
{
    public class CUFeedbackAndRequest
    {
        public string Guid { get; set; }
        public string  GlobalId { get; set; }
        public string  RequestCode { get; set; }
        public int  Status { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerCode { get; set; }
        public string CId { get; set; }
        public string ContractId { get; set; }
        public string ContractCode { get; set; }
        public string Service { get; set; }
        public string ServicePackage { get; set; }
        public string Address { get; set; }
        public string IPAddress { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public long Duration { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public string UpdateFrom { get; set; }
    }
}
