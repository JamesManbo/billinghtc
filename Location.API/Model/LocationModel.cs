using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Location.API.Model
{
    public class LocationModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = "";
        public string LocationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Path { get; set; }
        public int Level { get; set; }
        public string ParentId { get; set; }
        public string ParentName { get; set; }
        public bool Active { get; set; }
        public bool IsShow { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateUpdated { get; set; }
        public string CountryCode { get; set; }
        public float DisplayOrder { get; set; }
    }
}
