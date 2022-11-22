using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.ChangeHistories
{
    public class ContractHistory
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string ActionName { get; set; }
        public string JsonString { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int ContractId { get; set; }
        public bool IsInContract { get; set; }

    }
}
