using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.RadiusAndBras
{
    public class RadiusServerInfoDTO : BaseDTO
    {
        public string IP { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public int DatabasePort { get; set; }
        public string ServerName { get; set; }
        public string SSHUserName { get; set; }
        public string SSHPassword { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string BuildConnectionString()
        {
            return $"server={this.IP};database=radius;user={this.DatabaseUserName};password={this.DatabasePassword};persistsecurityinfo=True;allowloadlocalinfile=True;allowuservariables=True;Convert Zero Datetime=True;TreatTinyAsBoolean=False";
        }
    }
}
