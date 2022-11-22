using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.PromotionModels
{
    public class PromotionDTO
    {
        public int Id { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionName { get; set; }
        public int PromotionType { get; set; }
        public string PromotionTypeString { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public bool IsHot { get; set; }
    }
}
