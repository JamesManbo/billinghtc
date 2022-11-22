using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.RequestModels
{
    public class ArticleFilterModel : RequestFilterModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? ArticleType { get; set; }
        public bool? IsHot { get; set; }
        public string ArticleTypeCode { get; set; }
    }
}
