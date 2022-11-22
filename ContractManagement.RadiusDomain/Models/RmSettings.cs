using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmSettings
    {
        public string Currency { get; set; }
        public sbyte Unixacc { get; set; }
        public sbyte Diskquota { get; set; }
        public string Quotatpl { get; set; }
        public int Paymentopt { get; set; }
        public sbyte Changesrv { get; set; }
        public decimal Vatpercent { get; set; }
        public sbyte Disablenotpaid { get; set; }
        public sbyte Disableexpcont { get; set; }
        public sbyte Resetctr { get; set; }
        public sbyte Newnasallsrv { get; set; }
        public sbyte Newmanallsrv { get; set; }
        public sbyte Disconnmethod { get; set; }
        public long Warndl { get; set; }
        public int Warndlpercent { get; set; }
        public long Warnul { get; set; }
        public int Warnulpercent { get; set; }
        public long Warncomb { get; set; }
        public int Warncombpercent { get; set; }
        public long Warnuptime { get; set; }
        public int Warnuptimepercent { get; set; }
        public int Warnexpiry { get; set; }
        public sbyte Emailselfregman { get; set; }
        public sbyte Emailwelcome { get; set; }
        public sbyte Emailnewsrv { get; set; }
        public sbyte Emailrenew { get; set; }
        public sbyte Emailexpiry { get; set; }
        public sbyte Smswelcome { get; set; }
        public sbyte Smsexpiry { get; set; }
        public sbyte Warnmode { get; set; }
        public sbyte Selfreg { get; set; }
        public sbyte Edituserdata { get; set; }
        public sbyte Hidelimits { get; set; }
        public sbyte PmInternal { get; set; }
        public sbyte PmPaypalstd { get; set; }
        public sbyte PmPaypalpro { get; set; }
        public sbyte PmPaypalexp { get; set; }
        public sbyte PmSagepay { get; set; }
        public sbyte PmAuthorizenet { get; set; }
        public sbyte PmDps { get; set; }
        public sbyte Pm2co { get; set; }
        public sbyte PmPayfast { get; set; }
        public sbyte Unixhost { get; set; }
        public string Remotehostname { get; set; }
        public sbyte Maclock { get; set; }
        public sbyte Billingstart { get; set; }
        public sbyte Renewday { get; set; }
        public sbyte Changepswucp { get; set; }
        public sbyte Redeemucp { get; set; }
        public sbyte Buycreditsucp { get; set; }
        public sbyte SelfregFirstname { get; set; }
        public sbyte SelfregLastname { get; set; }
        public sbyte SelfregAddress { get; set; }
        public sbyte SelfregCity { get; set; }
        public sbyte SelfregZip { get; set; }
        public sbyte SelfregCountry { get; set; }
        public sbyte SelfregState { get; set; }
        public sbyte SelfregPhone { get; set; }
        public sbyte SelfregMobile { get; set; }
        public sbyte SelfregEmail { get; set; }
        public sbyte SelfregMobactsms { get; set; }
        public sbyte SelfregNameactemail { get; set; }
        public sbyte SelfregNameactsms { get; set; }
        public sbyte SelfregEndupemail { get; set; }
        public sbyte SelfregEndupmobile { get; set; }
        public sbyte SelfregVatid { get; set; }
        public sbyte IasEmail { get; set; }
        public sbyte IasMobile { get; set; }
        public sbyte IasVerify { get; set; }
        public sbyte IasEndupemail { get; set; }
        public sbyte IasEndupmobile { get; set; }
        public int Simuseselfreg { get; set; }
    }
}
