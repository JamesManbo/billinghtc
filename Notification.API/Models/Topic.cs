using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API.Models
{
    [BsonIgnoreExtraElements]
    public class Topic : BaseBsonEntity
    {
        public string Name { get; set; }
    }
}
