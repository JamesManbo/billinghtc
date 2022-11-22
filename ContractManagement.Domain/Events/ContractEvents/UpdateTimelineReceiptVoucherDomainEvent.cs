using ContractManagement.Domain.AggregatesModel.BaseContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Events.ContractEvents
{
    public class UpdateTimelineReceiptVoucherDomainEvent : INotification
    {
        public int ContractId { get; set; }
        public int OutServicePackageId { get; set; }
        public BillingTimeLine TimeLine { get; set; }
    }
}
