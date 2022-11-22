using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ServicesModels
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public bool HasPackages { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        public PictureViewDTO Avatar { get; set; }
    }
}
