using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace CMS.APIGateway.Models.FeedbackAndRequest
{
    public class FeedbackAndRequestDtoGrpc
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public int  Status { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
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
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

    }
}
