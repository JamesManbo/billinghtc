using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.RadiusAggregate
{
    [Table("RadiusServerInformation")]
    public class RadiusServerInformation : Entity
    {
        public string IP { get; set; }
        public int? MarketAreaId { get; set; }
        public int DatabasePort { get; set; }
        public string ServerName { get; set; }
        public string SSHUserName { get; set; }
        public string SSHPassword { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }
        public string Description { get; set; }
        public string GetConnectionString()
        {
            return $"server={this.IP};database=radius;user={this.DatabaseUserName};password={this.DatabasePassword};persistsecurityinfo=True;allowloadlocalinfile=True;allowuservariables=True;Convert Zero Datetime=True;TreatTinyAsBoolean=False";
        }
    }
}
