using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Location.API.Model
{
    public class LocationSelectionItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public object GlobalValue { get; set; }
        public string ParentId { get; set; }
        public float DisplayOrder { get; set; }
        public string Path { get; set; }
        public string Code { get; set; }
        public int Level { get; set; }
    }
}
