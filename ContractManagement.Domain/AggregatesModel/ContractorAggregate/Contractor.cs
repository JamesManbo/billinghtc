using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.ContractorAggregate
{
    [Table("Contractors")]
    public class Contractor : Entity
    {
        public static Contractor HTCHeadQuarter = new Contractor()
        {
            Id = 1,
            ContractorFullName = "Công ty cổ phần HTC viễn thông quốc tế",
            ContractorAddress = "Tầng 6 – Lotus Building, số 2, Duy Tân, Phường Dịch Vọng Hậu, Quận Cầu Giấy, Thành phố Hà Nội, Việt Nam",
            ContractorCode = "HTC_ITC",
            ContractorPhone = "(024) 3573 9419",
            ContractorEmail = "info@htc-itc.com.vn",
            CreatedBy = "ADMINISTRATOR",
            IsActive = true,
            IsHTC = true,
            IsEnterprise = true,
            ContractorTaxIdNo = "0102362584",
            IsBuyer = false
        };
        public Contractor()
        {
        }

        public Contractor(string identity, string name) : this()
        {
            IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
            ContractorFullName = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
        }

        [StringLength(68)]
        public string IdentityGuid { get; private set; }
        public string ContractorFullName { get; set; }
        public string ContractorShortName { get; set; }
        public string ContractorUserName { get; set; }
        public string ContractorCode { get; set; }
        public string ContractorPhone { get; set; }
        public string ContractorEmail { get; set; }
        public string ContractorFax { get; set; }
        [StringLength(256)]
        public string AccountingCustomerCode { get; set; }

        public string ContractorCity { get; set; }
        public string ContractorCityId { get; set; }
        public string ContractorDistrict { get; set; }
        public string ContractorDistrictId { get; set; }
        public string ContractorAddress { get; set; }
        public string ContractorIdNo { get; set; }
        public string ContractorTaxIdNo { get; set; }

        public bool IsEnterprise { get; set; }
        public bool IsBuyer { get; set; } // true: buyer, false: seller, provider
        public bool IsPartner { get; set; }
        public bool IsHTC { get; set; }
        public string UserIdentityGuid { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
        public string ParentId { get; set; }

        [StringLength(256)]
        public string Representative { get; set; }
        [StringLength(256)]
        public string Position { get; set; }
        [StringLength(256)]
        public string AuthorizationLetter { get; set; }

        public PaymentMethod VerifyOrAddPaymentMethod(int cardTypeId, string alias, string cardNumber,
            string securityNumber, string cardHolderName, DateTime expiration, int orderId)
        {
            //var existingPayment = _paymentMethods.SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

            //if (existingPayment != null)
            //{
            //    return existingPayment;
            //}

            //var payment  = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);
            //_paymentMethods.Add(payment);
            //return payment;
            return null;
        }
    }
}