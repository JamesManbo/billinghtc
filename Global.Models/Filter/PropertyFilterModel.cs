using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Models.Filter
{
    public class PropertyFilterModel
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object FilterValue { get; set; }
    }
}
