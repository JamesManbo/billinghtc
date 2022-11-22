using OrganizationUnit.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OrganizationUnit.Domain.AggregateModels.ConfigurationUserAggregate
{
    [Table("ConfigurationSystemParameters")]
    public class ConfigurationSystemParameter : Entity
    {
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
        public string Website { get; set; }
        public int NumberDaysBadDebt { get; set; }
        public int NumberDaysOverdue { get; set; }
        public int NotifyChannelExpirationDays { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public decimal CODCollectionFee { get; set; }

        public string PaymentCompany { get; set; }
        public string PaymentBankName { get; set; }
        public string PaymentBankNumber { get; set; }
        public string PaymentBankBranch { get; set; }

        public ConfigurationSystemParameter()
        {

        }

        public ConfigurationSystemParameter(int changeRecordExportExcel, int changeRecordExportPdf)
        {
            ChangeRecordExportExcel = changeRecordExportExcel;
            ChangeRecordExportPdf = changeRecordExportPdf;
        }
    }
}
