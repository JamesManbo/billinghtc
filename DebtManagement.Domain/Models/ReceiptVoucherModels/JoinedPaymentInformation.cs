using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReceiptVoucherModels
{
    public class JoinedPaymentInformation
    {
        private string joinedDomesticBandwidth;
        private string joinedInternationalBandwidth;
        public sbyte NumberOfJoinedChannels { get; set; }
        public DateTime JoinedStartBillingDate { get; set; }
        public string JoinedCids { get; set; }
        public string JoinedServices { get; set; }
        public string JoinedDistinctServices { get; set; }
        public string JoinedBandwidth { get; set; }
        public decimal JoinedSubTotalBeforeTax { get; set; }
        public decimal JoinedTaxAmount { get; set; }
        public decimal JoinedSubTotal { get; set; }
        public decimal JoinedGrandTotalBeforeTax { get; set; }
        public decimal JoinedGrandTotal { get; set; }

        public void NotifyJoinedPropertyChange(string propertyName, object value)
        {
            switch (propertyName)
            {
                case "StartBillingDate":
                    if (value != null)
                    {
                        this.JoinedStartBillingDate = (DateTime)value;
                    }
                    break;
                case "CId":
                    this.JoinedCids = value?.ToString() ?? string.Empty;
                    break;
                case "ServiceName":
                    this.JoinedServices = value?.ToString() ?? string.Empty;
                    this.JoinedDistinctServices = value?.ToString() ?? string.Empty;
                    break;
                case "InternationalBandwidth":
                    if (!string.IsNullOrEmpty(value?.ToString()))
                    {
                        this.joinedInternationalBandwidth = value.ToString();
                        this.JoinedBandwidth = $"{joinedDomesticBandwidth}" +
                            $"{(string.IsNullOrEmpty(this.joinedInternationalBandwidth) ? "" : "/" + this.joinedInternationalBandwidth)}";
                    }
                    break;
                case "DomesticBandwidth":
                    if (!string.IsNullOrEmpty(value?.ToString()))
                    {
                        this.joinedDomesticBandwidth = value.ToString();
                        this.JoinedBandwidth = $"{joinedDomesticBandwidth}" +
                            $"{(string.IsNullOrEmpty(this.joinedInternationalBandwidth) ? "" : "/" + this.joinedInternationalBandwidth)}";
                    }
                    break;
                case "SubTotalBeforeTax":
                    this.JoinedSubTotalBeforeTax += (decimal?)value ?? 0;
                    break;
                case "TaxAmount":
                    this.JoinedTaxAmount += (decimal?)value ?? 0;
                    break;
                case "SubTotal":
                    this.JoinedSubTotal += (decimal?)value ?? 0;
                    break;
                case "GrandTotalBeforeTax":
                    this.JoinedGrandTotalBeforeTax += (decimal?)value ?? 0;
                    break;
                case "GrandTotal":
                    this.JoinedGrandTotal += (decimal?)value ?? 0;
                    break;
                case "JoinedDistinctServices":
                    this.JoinedDistinctServices = value?.ToString();
                    break;
            }
        }
    }
}
