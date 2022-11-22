using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.SupportModels
{
    public class SupportDTO
    {
        public string Id { get; set; }
        public int Status { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CId { get; set; }
        public string ContractId { get; set; }
        public string ContractCode { get; set; }
        public string Service { get; set; }
        public string ServicePackage { get; set; }
        public string Address { get; set; }
        public string IPAddress { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public bool Handled { get; set; }
        public double? CustomerRate { get; set; }
        public string CustomerComment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string DateCreatedFormat { get { return DateCreated.ToString("dd/MM/yyyy"); } }
        public DateTime DateRequested { get; set; }
        public DateTime StartTime { get; set; }
        public string StartTimeFormat { get { return StartTime.ToString("dd/MM/yyyy"); } }
        public DateTime? StopTime { get; set; }
        public string StopTimeFormat { get { return StopTime.HasValue? StopTime.Value.ToString("dd/MM/yyyy"):""; } }
        public bool ChargeReduction { get; set; }
        public string Source { get; set; }
        public long Duration { get; set; }
        public string GlobalId { get; set; }
        public string ChannelText { get; set; }
    }
}
