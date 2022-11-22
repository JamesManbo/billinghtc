using System;

namespace Business
{
    public class CRHTCGridModel
    {
        public string Id { get; set; }
        public string CrId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string CategoryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Detail { get; set; }
        public string Note { get; set; }
        public int? Inactive { get; set; }
        public string ProjectId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string ImplementationSteps { get; set; }
        public string CrReason { get; set; }
        public string FieldHandler { get; set; }
        public string InfluenceChannel { get; set; }
        public string CrArea { get; set; }
        public string CrProvince { get; set; }
        public DateTime? StartTimeAction { get; set; }
        public DateTime? RestoreTimeService { get; set; }
        public int HourTimeMinus { get; set; }
        public int MinuteTimeMinus { get; set; }
        public int SecondTimeMinus { get; set; }
        public int? OverTimeRegister { get; set; }
        public string Supervisor { get; set; }
        public string TicketAreaId { get; set; }
        public string StatusId { get; set; }
        public string Total { get; set; }
        public string Approver { get; set; }
        public string Progress { get; set; }
    }
}