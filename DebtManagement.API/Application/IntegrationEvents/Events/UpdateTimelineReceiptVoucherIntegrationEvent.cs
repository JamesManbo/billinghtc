using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.Models;
using EventBus.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.API.Application.IntegrationEvents.Events
{
    public class UpdateTimelineReceiptVoucherIntegrationEvent : IntegrationEvent
    {
        public int ContractId { get; set; }
        public int OutServicePackageId { get; set; }
        public BillingTimeLine TimeLine { get; set; }
    }
}
