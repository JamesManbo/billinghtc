using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Reports
{
    public class ReportEquipmentInProjectDTO
    {
        public ReportEquipmentInProjectDTO()
        {
            EquipmentReports = new List<EquipmentReportDTO>();
        }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string ContractCode { get; set; }
        public int ProjectId { get; set; }       
        public int TotalEquipment { get; set; }
        public int Total { get; set; }
        public List<EquipmentReportDTO> EquipmentReports{ get; set; }
    }

    public class ReportEquipmentInProjectRaw
    {
        public string CustomerName { get; set; }
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public int Total { get; set; }
        public int TotalEquipment { get; set; }
        public int  EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentUom { get; set; }
        public string Activated { get; set; }
        public string Examined { get; set; }
        public string Confirmed { get; set; }
        public string Deployed { get; set; }
        public string Reclaimed { get; set; }
        public string SupporterHolded { get; set; }
        public string Cancelled { get; set; }
        public string Terminate { get; set; }
        public string CustomerCategory { get; set; }
        public string SerialCode { get; set; }
        public string ChannelCid { get; set; }
        public DateTime DeployedDateEquipment { get; set; }
    }

    public class EquipmentReportDTO
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentUom { get; set; }
        public int Examined { get; set; }
        public int Confirmed { get; set; }
        public int Deployed { get; set; }
        public int HasToBeReclaim { get; set; }
        public int Reclaimed { get; set; }
        public int Cancelled { get; set; }
        public int Terminate { get; set; }
        public int SupporterHolded { get; set; }
        public int Activated { get; set; }
    }
}
