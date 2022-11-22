using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Events.InContractEvents;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Models;
using GenericRepository.Extensions;

namespace DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate
{
    [Table("PaymentVouchers")]
    public class PaymentVoucher : VoucherAbstraction
    {
        public int InContractId { get; set; }
        public decimal FixedGrandTotal { get; set; }
        public string PaymentApprovalUserId { get; set; } // Nhân viên(kế toán) xác nhận chi tiền
        public string CashierUserId { get; set; } // Nhân viên thanh toán

        private List<PaymentVoucherDetail> _paymentVoucherDetails;
        public IReadOnlyCollection<PaymentVoucherDetail> PaymentVoucherDetails => _paymentVoucherDetails;

        private List<PaymentVoucherTax> _paymentVoucherTaxes;
        public IReadOnlyCollection<PaymentVoucherTax> PaymentVoucherTaxes => _paymentVoucherTaxes;

        private List<PaymentVoucherPaymentDetail> _paymentDetails;
        public IReadOnlyCollection<PaymentVoucherPaymentDetail> PaymentDetails => _paymentDetails;

        // Draft contracts have this set to true. Currently we don't check anywhere the draft status of an Contract, but we could do it if needed
        public PaymentVoucher()
        {
            Initialize();
        }

        public PaymentVoucher(CreatePaymentVoucherCommand createCommand)
        {
            Initialize();
            CreatedDate = createCommand.CreatedDate;
            CreatedBy = createCommand.CreatedBy;
            this.Binding(createCommand);
        }

        private void Initialize()
        {
            NumberDaysOverdue = 0;
            NumberBillingLimitDays = 60;
            IdentityGuid = Guid.NewGuid().ToString();
            _paymentDetails = new List<PaymentVoucherPaymentDetail>();
            _paymentVoucherDetails = new List<PaymentVoucherDetail>();
            _paymentVoucherTaxes = new List<PaymentVoucherTax>();
            Discount = new Discount();
            ReductionFreeTotal = 0;
            PromotionTotalAmount = 0;
            SubTotal = 0;
            ClearingTotal = 0;
            CashTotal = 0;
            OtherFee = 0;
            GrandTotalBeforeTax = 0;
            GrandTotal = 0;
            OpeningDebtAmount = 0;
            OpeningDebtPaidAmount = 0;
            GrandTotalIncludeDebt = 0;
            PaidTotal = 0;
            RemainingTotal = 0;
            TaxAmount = 0;
            InstallationFee = 0;
            EquipmentTotalAmount = 0;
            CurrencyUnitId = CurrencyUnit.VND.Id;
            CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
        }

        public void Update(UpdatePaymentVoucherCommand updateCommand)
        {
            UpdatedBy = updateCommand.UpdatedBy;
            UpdatedDate = updateCommand.UpdatedDate;

            //FixedGrandTotal = updateCommand.FixedGrandTotal;
            ReductionFreeTotal = 0;
            CashTotal = 0;
            SubTotal = 0;
            GrandTotal = 0;
            RemainingTotal = 0;

            this.Binding(updateCommand);
        }

        public void UpdateFromSentToAccountantStatus(UpdatePaymentVoucherCommand updateCommand)
        {
            UpdatedBy = updateCommand.UpdatedBy;
            UpdatedDate = updateCommand.UpdatedDate;

            Description = updateCommand.Description;
            Payment = updateCommand.Payment;
            PaymentDate = updateCommand.PaymentDate;
            AccountingCode = updateCommand.AccountingCode;
            InvoiceCode = updateCommand.InvoiceCode;
            InvoiceDate = updateCommand.InvoiceDate;
            InvoiceReceivedDate = updateCommand.InvoiceReceivedDate;
            Description = updateCommand.Description;

            SetStatusId(updateCommand.StatusId);
        }

        public void UpdateInvoice(UpdatePaymentVoucherCommand updateCommand)
        {
            UpdatedBy = updateCommand.UpdatedBy;
            UpdatedDate = updateCommand.UpdatedDate;

            if (string.IsNullOrEmpty(InvoiceCode))
            {
                InvoiceCode = updateCommand.InvoiceCode;
            }

            if (InvoiceDate == null)
            {
                InvoiceDate = updateCommand.InvoiceDate;
            }
            InvoiceReceivedDate = updateCommand.InvoiceReceivedDate;
            Description = updateCommand.Description;
        }

        private void Binding(CUVoucherBaseCommand command)
        {
            CancellationReason = command.CancellationReason;
            AccountingCode = command.AccountingCode;
            ContractCode = command.ContractCode;
            InContractId = command.InContractId;
            CreatedUserId = command.CreatedUserId;
            Content = command.Content;
            Description = command.Description;
            InvoiceCode = command.InvoiceCode;

            InvoiceDate = command.InvoiceDate;
            InvoiceReceivedDate = command.InvoiceReceivedDate;
            PaymentDate = command.PaymentDate;

            UpdatedBy = command.UpdatedBy;
            UpdatedDate = command.UpdatedDate;
            NumberBillingLimitDays = command.NumberBillingLimitDays;
            IsEnterprise = command.Target.IsEnterprise;

            CurrencyUnitId = command.CurrencyUnitId;
            CurrencyUnitCode = command.CurrencyUnitCode;
            CashierUserId = command.CashierUserId;
            MarketAreaId = command.MarketAreaId;
            MarketAreaName = command.MarketAreaName;
            ProjectId = command.ProjectId;
            ProjectName = command.ProjectName;

            TypeId = command.TypeId;
            VoucherCode = command.VoucherCode;
            OrganizationUnitId = command.OrganizationUnitId;
            OrganizationUnitName = command.OrganizationUnitName;

            IssuedDate = command.IssuedDate;
            PaymentPeriod = command.PaymentPeriod;
            Payment = command.Payment;

            BindingTotalValue(command);
        }

        public void BindingTotalValue(CUVoucherBaseCommand command)
        {
            this.PaidTotal = this.CashTotal + this.ClearingTotal;
            TaxAmount = command.TaxAmount;

            SubTotal = command.SubTotal;
            SubTotalBeforeTax = command.SubTotalBeforeTax;

            GrandTotal = command.GrandTotal;
            GrandTotalBeforeTax = command.GrandTotalBeforeTax;
            GrandTotalIncludeDebt = command.GrandTotalIncludeDebt;

            OtherFee = command.OtherFee;
            ReductionFreeTotal = command.ReductionFreeTotal;
            PromotionTotalAmount = command.PromotionTotalAmount;
            InstallationFee = command.InstallationFee;
            EquipmentTotalAmount = command.EquipmentTotalAmount;

            ClearingTotal = command.ClearingTotal;

            this.RemainingTotal = this.GrandTotal
               - this.PaidTotal
               - this.ClearingTotal;
        }
        public void UpdatePaymentVoucherDetail(CUPaymentVoucherDetailCommand command)
        {
            var updateModel = this.PaymentVoucherDetails.FirstOrDefault(r => r.Id == command.Id);
            if (updateModel != null)
            {
                updateModel.UpdatedBy = this.UpdatedBy;
                updateModel.UpdatedDate = DateTime.UtcNow;
                updateModel.Update(command);
            }
        }

        public PaymentVoucherDetail AddPaymentVoucherDetail(CUPaymentVoucherDetailCommand cUPaymentVoucherDetailCommand)
        {
            var newPaymentVoucherDetail = new PaymentVoucherDetail(cUPaymentVoucherDetailCommand);
            _paymentVoucherDetails.Add(newPaymentVoucherDetail);

            return newPaymentVoucherDetail;
        }

        public void UpdateNextBillingDate(int inContractId, DateTime nextBillingDate)
        {
            var paymentSuccessEvent = new PaymentVoucherCreatedEvent
            {
                InContractId = inContractId,
                GrandTotal = this.GrandTotal,
                TargetId = this.TargetId.Value,
                NextBillingDate = nextBillingDate
            };

            AddDomainEvent(paymentSuccessEvent);
        }

        public PaymentVoucherTax AddPaymentVoucherTax(CreatePaymentVoucherTaxCommand taxCommand)
        {
            var newVoucherTax = new PaymentVoucherTax(taxCommand);
            _paymentVoucherTaxes.Add(newVoucherTax);
            return newVoucherTax;
        }

        public void AddPaymentDetail(CuPaymentVoucherPaymentDetailCommand paymentDetailCommand)
        {
            var paymentDetailEntity = new PaymentVoucherPaymentDetail(paymentDetailCommand);
            _paymentDetails.Add(paymentDetailEntity);
        }

        public void UpdatePaymentDetail(CuPaymentVoucherPaymentDetailCommand paymentDetailCommand)
        {
            var paymentDetailEntity = _paymentDetails.First(p => p.Id == paymentDetailCommand.Id);
            paymentDetailEntity.PaidAmount = paymentDetailCommand.PaidAmount ?? 0;
            paymentDetailEntity.UpdatedBy = paymentDetailCommand.UpdatedBy;
            paymentDetailEntity.UpdatedDate = DateTime.Now;
        }

        public void UpdatePaymentDetail(PaymentVoucherPaymentDetail paymentDetail)
        {
            var paymentDetailEntity = _paymentDetails.First(p => p.IdentityGuid == paymentDetail.IdentityGuid);
            paymentDetailEntity.PaidAmount = paymentDetail.PaidAmount;
        }

        public void RemovePaymentDetail(string lineId)
        {
            var toRemoveElm = _paymentVoucherDetails.Find(s => s.IdentityGuid == lineId);
            _paymentVoucherDetails.Remove(toRemoveElm);
        }
        private void SetSentToAccountantStatus()
        {
            if (this.StatusId == PaymentVoucherStatus.Canceled.Id
                  || this.StatusId == PaymentVoucherStatus.Success.Id)
            {
                throw new DebtDomainException("Phiếu thu này không thể chuyển đến kế toán xác nhận");
            }

            this.StatusId = PaymentVoucherStatus.SentToAccountant.Id;
        }
        private void SetRejectStatus()
        {
            if (this.StatusId == PaymentVoucherStatus.Canceled.Id
                  || this.StatusId == PaymentVoucherStatus.Success.Id)
            {
                throw new DebtDomainException("Phiếu thu này không thể từ chối xác nhận từ kế toán");
            }

            this.StatusId = PaymentVoucherStatus.Rejected.Id;

        }
        private void SetOverdueStatus()
        {
            if (this.StatusId == PaymentVoucherStatus.Canceled.Id
                  || this.StatusId != PaymentVoucherStatus.Rejected.Id
                  || this.StatusId != PaymentVoucherStatus.Success.Id)
            {
                throw new DebtDomainException("Phiếu thu này không thể chuyển quá hạn");
            }

            this.StatusId = PaymentVoucherStatus.Overdue.Id;

        }
        private void SetSuccessStatus()
        {
            if (this.StatusId == PaymentVoucherStatus.Success.Id ||
                this.StatusId == PaymentVoucherStatus.Canceled.Id)
            {
                return;
            }

            this.StatusId = PaymentVoucherStatus.Success.Id;

            //var paymentVoucherSuccessEvent = new PaymentVoucherSuccessEvent()
            //{
            //    GrandTotal = this.GrandTotal,
            //    InContractId = this.InContractId,
            //    TargetId = this.TargetId.Value,
            //    VoucherDetails = this.PaymentVoucherDetails.ToList(),
            //};

            //AddDomainEvent(paymentVoucherSuccessEvent);
        }
        /// <summary>
        /// Đặt trạng thái của phiếu thu là Đang xử lý
        /// Hàm này chỉ được gọi 1 lần duy nhất khi tạo mới phiếu thu
        /// </summary>
        private void SetPendingStatus()
        {
            if (!this.IsTransient()) return;

            this.StatusId = PaymentVoucherStatus.New.Id;

            if (this.TypeId == PaymentVoucherType.ChannelRental.Id)
            {
                var paymentVoucherCreatedEvent = new PaymentVoucherCreatedEvent()
                {
                    GrandTotal = this.GrandTotal,
                    InContractId = this.InContractId,
                    TargetId = this.TargetId.Value,
                    VoucherDetails = this.PaymentVoucherDetails.ToList(),
                };

                AddDomainEvent(paymentVoucherCreatedEvent);
            }
        }

        private void SetCanceledStatus()
        {
            if (this.StatusId == PaymentVoucherStatus.Canceled.Id) return;
            if (this.StatusId != PaymentVoucherStatus.New.Id)
            {
                throw new DebtDomainException("Phiếu thu này không thể hủy");
            }

            this.StatusId = PaymentVoucherStatus.Canceled.Id;
            if (this.TypeId == PaymentVoucherType.ChannelRental.Id)
            {
                var paymentCancelEvent = new PaymentVoucherCanceledEvent
                {
                    InContractId = this.InContractId,
                    GrandTotal = this.GrandTotal,
                    TargetId = this.TargetId.Value,
                    VoucherDetails = this.PaymentVoucherDetails.ToList(),
                    NextBillingDate = this.PaymentVoucherDetails.FirstOrDefault().StartBillingDate
                };

                AddDomainEvent(paymentCancelEvent);
            }
            else if ((this.TypeId == 2 || this.TypeId == 3) && DateTime.Compare(this._paymentVoucherDetails.FirstOrDefault().StartBillingDate.Value, DateTime.Now) <= 0)  // phiếu trong kỳ hiện tại thì mới update
            {
                var paymentCancelEvent = new PaymentVoucherCanceledEvent
                {
                    InContractId = this.InContractId,
                    GrandTotal = this.GrandTotal,
                    TargetId = this.TargetId.Value,
                    NextBillingDate = this.PaymentVoucherDetails.FirstOrDefault().StartBillingDate
                };

                AddDomainEvent(paymentCancelEvent);
            }
        }

        public void SetStatusId(int statusId, string updatedUserId = "")
        {
            if (statusId <= 0 || statusId == PaymentVoucherStatus.New.Id)
            {
                SetPendingStatus();
            }
            else if (statusId == PaymentVoucherStatus.SentToAccountant.Id)
            {
                SetSentToAccountantStatus();
            }
            else if (statusId == PaymentVoucherStatus.Success.Id)
            {
                if (!string.IsNullOrWhiteSpace(updatedUserId))
                {
                    this.PaymentApprovalUserId = updatedUserId;
                }
                SetSuccessStatus();
            }
            else if (statusId == PaymentVoucherStatus.Rejected.Id)
            {
                if (!string.IsNullOrWhiteSpace(updatedUserId))
                {
                    this.PaymentApprovalUserId = updatedUserId;
                }
                SetRejectStatus();
            }
            else if (statusId == PaymentVoucherStatus.Overdue.Id)
            {
                SetOverdueStatus();
            }
            else if (statusId == PaymentVoucherStatus.Canceled.Id)
            {
                SetCanceledStatus();
            }

            if (this.StatusId == PaymentVoucherStatus.New.Id
                || this.StatusId == PaymentVoucherStatus.SentToAccountant.Id
                || this.StatusId == PaymentVoucherStatus.Overdue.Id
                || this.StatusId == PaymentVoucherStatus.Rejected.Id)
            {
                if (this.InvoiceDate.HasValue
                    && this.InvoiceDate.Value.AddDays(this.NumberBillingLimitDays + 1).LessThanOrEqualDate(DateTime.Now))
                {
                    this.StatusId = PaymentVoucherStatus.Overdue.Id;
                    this.NumberDaysOverdue = DateTime.UtcNow.AddHours(7).Day
                        - this.InvoiceDate.Value.AddDays(this.NumberBillingLimitDays).AddHours(7).Day;
                }
            }
        }

        public void SetLock(bool isLock)
        {
            IsLock = isLock;
        }

        public void SetTarget(int targetId)
        {
            if (targetId == default)
            {
                throw new DebtDomainException("Đối tượng thanh toán không thể trống");
            }

            TargetId = targetId;
        }

        public override void CalculateTotal(bool overrideTotal = false)
        {
            // Tính toán số tiền thực chi và tổng tiền còn lại
            if (this.PaymentDetails != null && this.PaymentDetails.Any())
            {
                this.CashTotal = this.PaymentDetails.Sum(e => e.PaidAmount);
            }

            this.PaidTotal = this.CashTotal + this.ClearingTotal;
            this.RemainingTotal = this.GrandTotal - this.PaidTotal;
        }
    }
}
