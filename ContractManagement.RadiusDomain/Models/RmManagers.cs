using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmManagers
    {
        public string Managername { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Comment { get; set; }
        public string Company { get; set; }
        public string Vatid { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
        public sbyte PermListusers { get; set; }
        public sbyte PermCreateusers { get; set; }
        public sbyte PermEditusers { get; set; }
        public sbyte PermEdituserspriv { get; set; }
        public sbyte PermDeleteusers { get; set; }
        public sbyte PermListmanagers { get; set; }
        public sbyte PermCreatemanagers { get; set; }
        public sbyte PermEditmanagers { get; set; }
        public sbyte PermDeletemanagers { get; set; }
        public sbyte PermListservices { get; set; }
        public sbyte PermCreateservices { get; set; }
        public sbyte PermEditservices { get; set; }
        public sbyte PermDeleteservices { get; set; }
        public sbyte PermListonlineusers { get; set; }
        public sbyte PermListinvoices { get; set; }
        public sbyte PermTrafficreport { get; set; }
        public sbyte PermAddcredits { get; set; }
        public sbyte PermNegbalance { get; set; }
        public sbyte PermListallinvoices { get; set; }
        public sbyte PermShowinvtotals { get; set; }
        public sbyte PermLogout { get; set; }
        public sbyte PermCardsys { get; set; }
        public sbyte PermEditinvoice { get; set; }
        public sbyte PermAllusers { get; set; }
        public sbyte PermAllowdiscount { get; set; }
        public sbyte PermEnwriteoff { get; set; }
        public sbyte PermAccessap { get; set; }
        public sbyte PermCts { get; set; }
        public sbyte Enablemanager { get; set; }
        public string Lang { get; set; }
    }
}
