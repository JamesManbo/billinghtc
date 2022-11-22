using DebtManagement.Domain.Models.ReportModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
    public class ChannelAddressModel
    {
        public string Cid { get; set; }
        public InstallationAddress StartPointAddress { get; set; }
        public InstallationAddress EndPointAddress { get; set; }
    }
}
