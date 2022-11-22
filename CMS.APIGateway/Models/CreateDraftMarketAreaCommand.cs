using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.APIGateway.Models
{
    public class CreateDraftMarketAreaCommand
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Level { get; set; }
        public string RegionId { get; set; }
        public string CityId { get; set; }
        public string DistrictId { get; set; }
        public string Commune { get; set; }
    }
}
