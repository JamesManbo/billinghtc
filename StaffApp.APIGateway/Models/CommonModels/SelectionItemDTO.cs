using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.CommonModels
{
    public class SelectionItemDTO
    {
        public string Text { get; set; }
        public string Code { get; set; }
        public int Value { get; set; }
        public object GlobalValue { get; set; }
        public int? ParentId { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
