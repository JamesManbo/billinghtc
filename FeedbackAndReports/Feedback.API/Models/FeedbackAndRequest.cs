using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Feedback.API.Models
{
    [BsonIgnoreExtraElements]
    public class FeedbackAndRequest
    {

        private string _cId = string.Empty;
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string RequestCode { get; set; }
        public int Status { get; set; }
        public int ReceiptLineId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerIdentityGuid { get; set; }
        public int ConstractorId { get; set; }
        public string CId
        {
            get
            {
                return this._cId;
            }
            set
            {
                this._cId = string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public string ContractId { get; set; }
        public string ContractCode { get; set; }
        public int? OutContractServicePackageId { get; set; }
        public string Service { get; set; }
        public string ServicePackage { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }
        public string City { get; set; }
        public string CityId { get; set; }
        public string IPAddress { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public bool ChargeReduction { get; set; }
        public bool Handled { get; set; }
        public string Source { get; set; }
        public double? CustomerRate { get; set; }
        public string CustomerComment { get; set; }
        public string GlobalId { get; set; }
        public long Duration { get; set; }
        public string UpdateFrom { get; set; }
        public string CreatedBy { get; set; }
        public string ChannelText { get; set; }
    }

    public class CreateFeedbackCommand
    {
        public string ContractId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string PhoneNumber { get; set; }
        public bool ChargeReduction { get; set; }
        public string GlobalId { get; set; }
        public string CreatedBy { get; set; }
        public string CustomerIdentityGuid { get; set; }
        public int ConstractorId { get; set; }
        public string ChannelText { get; set; }
    }

    public class UpdateFeedbackCommand
    {
        public string Id { get; set; }
        public string ContractId { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string PhoneNumber { get; set; }
        public bool ChargeReduction { get; set; }
        public string GlobalId { get; set; }

    }
}
