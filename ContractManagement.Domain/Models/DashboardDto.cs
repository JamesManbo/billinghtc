using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class DashboardDto
    {
        public int ValueMonth1 { get; set; }
        public int ValueMonth2 { get; set; }
        public int ValueMonth3 { get; set; }
        public int ValueMonth4 { get; set; }
        public int ValueMonth5 { get; set; }
        public int ValueMonth6 { get; set; }
        public int ValueMonth7 { get; set; }
        public int ValueMonth8 { get; set; }
        public int ValueMonth9 { get; set; }
        public int ValueMonth10 { get; set; }
        public int ValueMonth11{ get; set; }
        public int ValueMonth12 { get; set; }
        public int Total { get; set; }
    }

    public class EffectedContract
    {
        public string Thang { get; set; }
        public int Quantity { get; set; }
    }
}
