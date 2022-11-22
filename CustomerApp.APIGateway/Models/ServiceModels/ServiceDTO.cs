using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public PictureViewModel Avatar { get; set; }
    }
}
