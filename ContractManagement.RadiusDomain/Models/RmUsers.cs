using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmUsers
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int Groupid { get; set; }
        public sbyte Enableuser { get; set; }
        public long Uplimit { get; set; }
        public long Downlimit { get; set; }
        public long Comblimit { get; set; }
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string Company { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Mobile { get; set; } = "";
        public string Address { get; set; } = "";
        public string City { get; set; } = "";
        public string Zip { get; set; } = "";
        public string Country { get; set; } = "";
        public string State { get; set; } = "";
        public string Comment { get; set; } = "";
        public decimal Gpslat { get; set; }
        public decimal Gpslong { get; set; }
        public string Mac { get; set; } = "";
        public sbyte Usemacauth { get; set; }
        public DateTime Expiration { get; set; }
        public long Uptimelimit { get; set; }
        public int Srvid { get; set; }
        public string Staticipcm { get; set; } = "";
        public string Staticipcpe { get; set; } = "";
        public sbyte Ipmodecm { get; set; }
        public sbyte Ipmodecpe { get; set; }
        public int Poolidcm { get; set; }
        public int Poolidcpe { get; set; }
        public DateTime Createdon { get; set; }
        public sbyte Acctype { get; set; }
        public decimal Credits { get; set; }
        public sbyte Cardfails { get; set; }
        public string Createdby { get; set; } = "";
        public string Owner { get; set; } = "";
        public string Taxid { get; set; } = "";
        public string Email { get; set; } = "";
        public string Maccm { get; set; } = "";
        public string Custattr { get; set; } = "";
        public sbyte Warningsent { get; set; }
        public string Verifycode { get; set; } = "";
        public sbyte Verified { get; set; }
        public sbyte Selfreg { get; set; }
        public sbyte Verifyfails { get; set; }
        public sbyte Verifysentnum { get; set; }
        public string Verifymobile { get; set; } = "";
        public string Contractid { get; set; } = "";
        public DateTime Contractvalid { get; set; }
        public string Actcode { get; set; } = "";
        public sbyte Pswactsmsnum { get; set; }
        public sbyte Alertemail { get; set; }
        public sbyte Alertsms { get; set; }
        public string Lang { get; set; } = "";
        public DateTime? Lastlogoff { get; set; }
    }
}
