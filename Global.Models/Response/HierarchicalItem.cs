using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Models.Response
{
    public class HierarchicalItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public int TreeLevel { get; set; }
        public string TreePath { get; set; }
        public object GlobalValue { get; set; }
        public int? DisplayOrder { get; set; }
        public bool HasChildren { get; set; }
        public string Description { get; set; }
    }
}
