using Global.Models.StateChangedResponse;
using MediatR;
using OrganizationUnit.Domain.Models.ConfigurationSettingUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Application.Commands.ConfigurationSettingUser
{
    public class CUConfigurationSettingUserCommand : IRequest<ActionResponse<ConfigurationSettingUserDto>>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool AllowSendEmail { get; set; }
        public bool AllowSendNotification { get; set; }
        public bool AllowSendSMS { get; set; }
    }

    public class CUConfigurationSystemParameterCommand : IRequest<ActionResponse<ConfigurationSystemParameterDto>>
    {
        public int Id { get; set; }
        public int ChangeRecordExportExcel { get; set; }
        public int ChangeRecordExportPdf { get; set; }
        public string OrganizationUnit { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public string BankAccount { get; set; }
        public string TelephoneSwitchboard { get; set; }
        public string RepresentativePersonName { get; set; }
        public string RpPosition { get; set; }
        public string AuthorizationLetterNumber { get; set; }
        public string TradingAddress { get; set; }
        public int NumberDaysBadDebt { get; set; }
        public int NumberDaysOverdue { get; set; }
        public string Website { get; set; }
        public int NotifyChannelExpirationDays { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public decimal CODCollectionFee { get; set; }

        public string PaymentCompany { get; set; }
        public string PaymentBankName { get; set; }
        public string PaymentBankNumber { get; set; }
        public string PaymentBankBranch { get; set; }
    }
}
