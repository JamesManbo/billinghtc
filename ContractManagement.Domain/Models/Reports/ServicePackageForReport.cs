using ContractManagement.Domain.AggregatesModel.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Reports
{
    public class ServicePackageForMasterReport
    {
        public ServicePackageForMasterReport()
        {
            StartPoint = new OutputChannelPointDTO();
            EndPoint = new OutputChannelPointDTO();
        }
        public bool HasStartAndEndPoint { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal PackagePrice { get; set; }
        public OutputChannelPointDTO StartPoint { get; set; }
        public OutputChannelPointDTO EndPoint { get; set; }
        public string ServiceName { get; set; }
        public string ServicePackageName { get; set; }
        public string Bandwidth { get; set; }

        public int TimeLinePaymentPeriod { get; set; } 
        public int TimeLinePrepayPeriod { get; set; } 
        public string TimeLineEffective { get; set; } 

    }
}
