using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Exceptions;
using Microsoft.EntityFrameworkCore.Internal;
using DebtManagement.Domain.Events.ContractEvents;
using DebtManagement.Domain.Events.ReceiptVoucherEvents;
using DebtManagement.Domain.Commands.DebtCommand;
using GenericRepository.Extensions;
using DebtManagement.Domain.Commands.BaseVoucherCommand;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("ReceiptVouchers")]
    public class ReceiptVoucher : VoucherAbstraction
    {
        public int OutContractId { get; set; }
        public DateTime? CashierCollectingDate { get; set; }
        public DateTime? CashierReceivedDate { get; set; }
        public decimal OffsetUpgradePackageAmount { get; set; }
        public bool IsFirstVoucherOfContract { get; set; }
        public decimal DiscountAmountSuspendTotal { get; set; }
        public string BadDebtApprovalContent { get; set; }
        public bool? IsBadDebt { get; set; }
        public int NumberOfOpeningDebtHistories { get; set; }
        public int NumberOfDebtHistories { get; set; }
        public bool InvalidIssuedDate { get; set; }
        public decimal CashierDebtRemaningTotal { get; set; }
        public decimal TargetDebtRemaningTotal { get; set; }
        /// <summary>
        /// Phí thu tiền tận nơi
        /// </summary>
        public bool IsHasCollectionFee { get; set; }
        public decimal CODCollectionFee { get; set; }

        private List<ReceiptVoucherDetail> _receiptVoucherDetails;
        public IReadOnlyCollection<ReceiptVoucherDetail> ReceiptVoucherDetails => _receiptVoucherDetails;

        private List<PromotionForReceiptVoucher> _promotionForReceiptVoucher;
        public IReadOnlyCollection<PromotionForReceiptVoucher> PromotionForReceiptVoucher => _promotionForReceiptVoucher;

        public IReadOnlyCollection<ReceiptVoucherDetail> ValidReceiptVoucherDetails
                => _receiptVoucherDetails.Where(d => !d.IsDeleted && d.IsActive).ToList();

        // Danh sách lịch sử thu nợ
        private List<ReceiptVoucherDebtHistory> _debtHistories;
        public IReadOnlyCollection<ReceiptVoucherDebtHistory> DebtHistories => _debtHistories;

        // Danh sách lịch sử thu nợ đầu kì
        private List<ReceiptVoucherDebtHistory> _openingDebtHistories;
        public IReadOnlyCollection<ReceiptVoucherDebtHistory> OpeningDebtHistories => _openingDebtHistories;

        public ReceiptVoucherDebtHistory ActivatedDebt
            => DebtHistories.OrderByDescending(d => d.CreatedDate).FirstOrDefault();

        public virtual HashSet<AttachmentFile> AttachmentFiles { get; set; }

        private void Initialize()
        {
            NumberDaysOverdue = 0;
            NumberBillingLimitDays = 60;
            IdentityGuid = Guid.NewGuid().ToString();
            _receiptVoucherDetails = new List<ReceiptVoucherDetail>();

            _promotionForReceiptVoucher = new List<PromotionForReceiptVoucher>();

            _debtHistories = new List<ReceiptVoucherDebtHistory>();
            _openingDebtHistories = new List<ReceiptVoucherDebtHistory>();

            AttachmentFiles = new HashSet<AttachmentFile>();
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
            CashierDebtRemaningTotal = 0;
            TargetDebtRemaningTotal = 0;
            DiscountAmountSuspendTotal = 0;
            OffsetUpgradePackageAmount = 0;
            InstallationFee = 0;
            EquipmentTotalAmount = 0;
            NumberOfOpeningDebtHistories = 0;
            NumberOfDebtHistories = 0;
            CurrencyUnitId = CurrencyUnit.VND.Id;
            CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode;
            StatusId = ReceiptVoucherStatus.Pending.Id;
        }

        public ReceiptVoucher()
        {
            Initialize();
        }

        public ReceiptVoucher(CreateReceiptVoucherCommand createCommand)
        {
            Initialize();

            CurrencyUnitId = createCommand.CurrencyUnitId;
            CurrencyUnitCode = createCommand.CurrencyUnitCode;
            CancellationReason = createCommand.CancellationReason;

            NumberBillingLimitDays = createCommand.NumberBillingLimitDays;

            MarketAreaId = createCommand.MarketAreaId;
            MarketAreaName = createCommand.MarketAreaName;
            ProjectId = createCommand.ProjectId;
            ProjectName = createCommand.ProjectName;

            AccountingCode = createCommand.AccountingCode;
            ContractCode = createCommand.ContractCode;
            OutContractId = createCommand.OutContractId;

            TypeId = createCommand.TypeId;
            VoucherCode = createCommand.VoucherCode;
            Content = createCommand.Content;
            Description = createCommand.Description;
            CreatedUserId = createCommand.CreatedUserId;
            CreatedUserFullName = createCommand.CreatedUserFullName;
            CreatedBy = createCommand.CreatedBy;
            OrganizationUnitId = createCommand.OrganizationUnitId;
            OrganizationUnitName = createCommand.OrganizationUnitName;
            InvoiceCode = createCommand.InvoiceCode;
            IssuedDate = createCommand.IssuedDate.ToExactLocalDate();
            CashierCollectingDate = createCommand.IssuedDate;
            CashierReceivedDate = createCommand.CashierReceivedDate;
            InvoiceDate = createCommand.InvoiceDate;
            InvoiceReceivedDate = createCommand.InvoiceReceivedDate;
            PaymentDate = createCommand.PaymentDate;

            Payment = createCommand.Payment;
            CreatedDate = createCommand.CreatedDate;
            IsFirstVoucherOfContract = createCommand.IsFirstVoucherOfContract;
            IsAutomaticGenerate = createCommand.IsAutomaticGenerate;
            NumberBillingLimitDays = createCommand.NumberBillingLimitDays;
            IsEnterprise = createCommand.Target.IsEnterprise;

            InvalidIssuedDate = createCommand.InvalidIssuedDate;

            // Người tạo phiếu có thể sửa tổng tiền thu trước thuế và giá trị thuế ở dưới client
            TaxAmount = createCommand.TaxAmount;
            GrandTotalBeforeTax = createCommand.GrandTotalBeforeTax;
            IsHasCollectionFee = createCommand.IsHasCollectionFee;
            CODCollectionFee = createCommand.CODCollectionFee;

            SetCashierUser(createCommand.CashierUserId,
                createCommand.CashierUserName,
                createCommand.CashierFullName);

            BindingTotalValue(createCommand);
        }

        public void Update(UpdateReceiptVoucherCommand updateCommand)
        {
            AccountingCode = updateCommand.AccountingCode;
            CreatedUserId = updateCommand.CreatedUserId;
            Content = updateCommand.Content;
            Description = updateCommand.Description;
            InvoiceCode = updateCommand.InvoiceCode;

            CashierCollectingDate = IssuedDate;
            CashierReceivedDate = updateCommand.CashierReceivedDate;
            InvoiceDate = updateCommand.InvoiceDate;
            InvoiceReceivedDate = updateCommand.InvoiceReceivedDate;
            PaymentDate = updateCommand.PaymentDate;

            UpdatedBy = updateCommand.UpdatedBy;
            UpdatedDate = updateCommand.UpdatedDate;
            NumberBillingLimitDays = updateCommand.NumberBillingLimitDays;

            IsHasCollectionFee = updateCommand.IsHasCollectionFee;
            CODCollectionFee = updateCommand.CODCollectionFee;

            SetCashierUser(updateCommand.CashierUserId,
                updateCommand.CashierUserName,
                updateCommand.CashierFullName);

            BindingTotalValue(updateCommand);
        }

        public void BindingTotalValue(CUVoucherBaseCommand command)
        {
            TaxAmount = command.TaxAmount;

            SubTotal = command.SubTotal;
            SubTotalBeforeTax = command.SubTotalBeforeTax;

            GrandTotal = command.GrandTotal;
            GrandTotalBeforeTax = command.GrandTotalBeforeTax;
            GrandTotalIncludeDebt = command.GrandTotalIncludeDebt;

            OtherFee = command.OtherFee;
            ReductionFreeTotal = command.ReductionFreeTotal;
            PromotionTotalAmount = command.PromotionTotalAmount;
            OffsetUpgradePackageAmount = command.OffsetUpgradePackageAmount;
            DiscountAmountSuspendTotal = command.DiscountAmountSuspendTotal;
            InstallationFee = command.InstallationFee;
            EquipmentTotalAmount = command.EquipmentTotalAmount;

            ClearingTotal = command.ClearingTotal;
        }

        public void UpdateStatusOverdue()
        {
            if (this.StatusId == ReceiptVoucherStatus.Pending.Id
                || this.StatusId == ReceiptVoucherStatus.CollectOnBehalf.Id)
            {
                if (this.IssuedDate.AddDays(this.NumberBillingLimitDays + 1).LessThanOrEqualDate(DateTime.Now))
                {
                    this.StatusId = ReceiptVoucherStatus.Overdue.Id;
                    this.NumberDaysOverdue = DateTime.UtcNow.AddHours(7).Day
                        - this.IssuedDate.AddDays(this.NumberBillingLimitDays).AddHours(7).Day;
                }
            }
        }

        public void UpdateStatusBadDebt(int debtAge)
        {
            if (this.NumberDaysOverdue >= debtAge && !this.IsEnterprise)
            {
                SetBadDebtStatus();
            }
        }

        public void SetStatusId(int statusId)
        {
            if (statusId == ReceiptVoucherStatus.Pending.Id)
            {
                SetPendingStatus();
            }
            else
            if (statusId == ReceiptVoucherStatus.CollectOnBehalf.Id)
            {
                SetCollectOnBehalfStatus();
            }
            else
            if (statusId == ReceiptVoucherStatus.SentToAccountant.Id)
            {
                SetSentToAccountantStatus();
            }
            else
            if (statusId == ReceiptVoucherStatus.Invoiced.Id)
            {
                SetInvoicedStatus();
            }
            else
            if (statusId == ReceiptVoucherStatus.Success.Id)
            {
                SetSuccessStatus();
            }
            else
            if (statusId == ReceiptVoucherStatus.Canceled.Id)
            {
                SetCanceledStatus();
            }
            else
            if (statusId == ReceiptVoucherStatus.PayingBadDebt.Id)
            {
                SetPayingBadDebtStatus();
            }
            else
            if (statusId == ReceiptVoucherStatus.Overdue.Id)
            {
                SetOverdueStatus();
            }
            else
            if (statusId == ReceiptVoucherStatus.BadDebt.Id)
            {
                SetBadDebtStatus();
            }

        }

        private void SetSentToAccountantStatus()
        {
            if (this.StatusId != ReceiptVoucherStatus.Canceled.Id) return;

            this.StatusId = ReceiptVoucherStatus.SentToAccountant.Id;
        }

        private void SetBadDebtStatus()
        {
            if (this.StatusId == ReceiptVoucherStatus.BadDebt.Id
                || this.StatusId == ReceiptVoucherStatus.Canceled.Id) return;

            this.StatusId = ReceiptVoucherStatus.BadDebt.Id;
        }

        private void SetCollectOnBehalfStatus()
        {
            if (this.StatusId != ReceiptVoucherStatus.Pending.Id
                && this.StatusId != ReceiptVoucherStatus.CollectOnBehalf.Id
                && this.StatusId != ReceiptVoucherStatus.Overdue.Id) return;

            if (this.StatusId == ReceiptVoucherStatus.Overdue.Id)
            {
                var today = DateTime.UtcNow.AddHours(7);
                this.NumberBillingLimitDays = (today - this.IssuedDate).Days + 30;
            }

            this.StatusId = ReceiptVoucherStatus.CollectOnBehalf.Id;
            this.OpeningDebtCollectingHandler();
        }

        /// <summary>
        /// kế toán giờ có thể duyệt các phiếu có trạng thái không chỉ là thu hộ
        /// trừ đã hủy và đã thu
        /// 2021-05-26 tuandv        
        /// </summary>
        private void SetSuccessStatus()
        {
            //if (this.StatusId != ReceiptVoucherStatus.CollectOnBehalf.Id &&
            //    this.StatusId != ReceiptVoucherStatus.Invoiced.Id &&
            //    this.StatusId != ReceiptVoucherStatus.SentToAccountant.Id &&
            //    this.StatusId != ReceiptVoucherStatus.Success.Id &&
            //    this.TypeId != ReceiptVoucherType.Billing.Id &&
            //    //Payment Method 2 là bù trừ
            //    this.Payment.Method != 2) return;

            //if (               
            //        ( this.TypeId != ReceiptVoucherType.Signed.Id &&
            //          this.TypeId != ReceiptVoucherType.Billing.Id 
            //        ) ||
            //    //Payment Method 2 là bù trừ
            //    this.Payment.Method == 2)return;
            if ((this.StatusId == ReceiptVoucherStatus.Pending.Id && !this.DebtHistories.Any(p => p.PaymentDetails.Any(e => e.PaidAmount > 0)))
                || (this.StatusId == ReceiptVoucherStatus.Overdue.Id && this.DebtHistories.Any(p => p.PaymentDetails.Any(e => e.PaidAmount > 0)))
               ) // xác nhận công nợ với phiếu đang xử lý
            {
                this.Payment.Method = 1;  // chuyển khoản
                this.PaidTotal = this.GrandTotal;
                //tạo paymentDetail
                var paymentDetail = new CuReceiptVoucherPaymentDetailCommand
                {
                    ReceiptVoucherId = this.Id,
                    PaymentMethod = 1, // chuyển khoản
                    PaymentMethodName = "Chuyển khoản",
                    PaidAmount = this.PaidTotal,
                    IssuedDate = DateTime.Now,
                    CreatedBy = this.CreatedBy,
                    CurrencyUnitCode = this.CurrencyUnitCode
                };
                var newDebtHistory = new ReceiptVoucherDebtHistory
                {
                    IdentityGuid = Guid.NewGuid().ToString(),
                    IssuedDate = this.IssuedDate,
                    CashierUserId = "",
                    CashierUserName = "",
                    CashierFullName = "",
                    CreatedBy = this.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    ReceiptVoucherId = this.Id,
                    ReceiptVoucherCode = this.VoucherCode,
                    ReceiptVoucherContent = this.Content,
                    Status = PaymentStatus.Assigned,
                    CurrencyUnitCode = this.CurrencyUnitCode,
                    IsActive = true,
                };
                newDebtHistory.AddPaymentDetail(paymentDetail);
                this._debtHistories.Add(newDebtHistory);
            }

            this.StatusId = ReceiptVoucherStatus.Success.Id;

            this.ActivatedDebtPayingHandler();
        }

        private void SetInvoicedStatus()
        {
            if (this.StatusId == ReceiptVoucherStatus.Canceled.Id) return;

            this.StatusId = ReceiptVoucherStatus.Invoiced.Id;
        }

        /// <summary>
        /// Đặt trạng thái của phiếu thu là Đang xử lý
        /// Hàm này chỉ được gọi 1 lần duy nhất khi tạo mới phiếu thu
        /// </summary>
        private void SetPendingStatus()
        {
            if (this.Id > 0) return;

            this.StatusId = ReceiptVoucherStatus.Pending.Id;
            if (!this.IsAutomaticGenerate && !this.InvalidIssuedDate)
            {
                var billingReceiptVchrPendingEvent = new BillingPaymentPendingEvent
                {
                    IsActiveSPST = true,
                    VoucherDetails = this.ValidReceiptVoucherDetails.ToList(),
                    Promotions = this.PromotionForReceiptVoucher.ToList()
                };

                AddDomainEvent(billingReceiptVchrPendingEvent);
            }
        }

        private void SetCanceledStatus()
        {
            if (this.StatusId == ReceiptVoucherStatus.Canceled.Id ||
                this.StatusId == ReceiptVoucherStatus.Invoiced.Id ||
                this.StatusId == ReceiptVoucherStatus.PayingBadDebt.Id ||
                this.StatusId == ReceiptVoucherStatus.Success.Id)
            {
                throw new DebtDomainException("Phiếu thu này không thể hủy");
            }

            this.StatusId = ReceiptVoucherStatus.Canceled.Id;

            this.ClearDebtPaymentDetails();
            this.ClearPaymentDetails();

            var paymentSuccessEvent = new BillingPaymentCanceledEvent
            {
                IsActiveSPST = false,
                OutContractId = this.OutContractId,
                IsFirstVoucherOfContract = this.IsFirstVoucherOfContract,
                VoucherDetails = this.ValidReceiptVoucherDetails.ToList(),
                Promotions = this.PromotionForReceiptVoucher.ToList()
            };

            AddDomainEvent(paymentSuccessEvent);
        }

        private void SetPayingBadDebtStatus()
        {
            if (this.StatusId != ReceiptVoucherStatus.BadDebt.Id)
            {
                throw new DebtDomainException("Phiếu thu này không thể thanh toán nợ xấu");
            }

            this.StatusId = ReceiptVoucherStatus.PayingBadDebt.Id;
        }

        private void SetOverdueStatus()
        {
            if (this.StatusId == ReceiptVoucherStatus.Overdue.Id)
            {
                if (this.IsBadDebt == true)
                {
                    this.StatusId = ReceiptVoucherStatus.BadDebt.Id;
                }

                if (this.IsEnterprise == false)
                {
                    this.NumberDaysOverdue =
                        DateTime.UtcNow.AddHours(7).Day
                        - this.IssuedDate.AddDays(this.NumberBillingLimitDays).AddHours(7).Day;
                }
                else
                {
                    this.NumberDaysOverdue =
                        DateTime.UtcNow.AddHours(7).Day
                        - this.IssuedDate.AddDays(this.NumberBillingLimitDays > 30 ? this.NumberBillingLimitDays : 30).AddHours(7).Day;
                }
            }
            else
            {
                this.StatusId = ReceiptVoucherStatus.Overdue.Id;
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

        public void AddReceiptVoucherDetail(CUReceiptVoucherDetailCommand command)
        {
            if (command.BusTablePricingCalculators != null &&
                command.BusTablePricingCalculators.Any() &&
                command.BusTablePricingCalculators.Select(b => b.ChannelId).Distinct().Count() > 1)
            {
                command.IsJoinedPayment = true;
                var otherChannelBustableCals = command.BusTablePricingCalculators.Where(b => !b.IsMainRcptVoucherLine);
                this.AddJoinedChannelVcherDetail(command, otherChannelBustableCals);

                command.BusTablePricingCalculators.RemoveAll(b => !b.IsMainRcptVoucherLine);
                command.IsMainPaymentChannel = true;
                var mainRcptVoucherLine = new ReceiptVoucherDetail(command);
                _receiptVoucherDetails.Add(mainRcptVoucherLine);
                return;
            }

            command.IsJoinedPayment = false;
            command.IsMainPaymentChannel = true;
            var newReceiptVoucherDetail = new ReceiptVoucherDetail(command);
            _receiptVoucherDetails.Add(newReceiptVoucherDetail);
        }

        private void AddJoinedChannelVcherDetail(CUReceiptVoucherDetailCommand detailCmd,
            IEnumerable<CUBusTablePricingCalculatorCommand> cals)
        {
            var groupedCals = cals.GroupBy(g => new
            {
                g.ChannelId,
                g.ChannelCid,
                g.StartingBillingDate,
                g.ServiceId,
                g.ServiceName,
                g.DomesticBandwidth,
                g.InternationalBandwidth
            });

            foreach (var channel in groupedCals)
            {
                var rptVoucherDetailCommand = new CUReceiptVoucherDetailCommand(detailCmd)
                {
                    OutContractServicePackageId = channel.Key.ChannelId,
                    CId = channel.Key.ChannelCid,
                    ServiceId = channel.Key.ServiceId,
                    ServiceName = channel.Key.ServiceName,
                    DomesticBandwidth = channel.Key.DomesticBandwidth,
                    InternationalBandwidth = channel.Key.InternationalBandwidth,
                    StartBillingDate = channel.Key.StartingBillingDate,
                    BusTablePricingCalculators = channel.ToList(),
                    IsMainPaymentChannel = false,
                    OtherFeeTotal = 0,
                    InstallationFee = 0,
                    ReductionFee = 0
                };

                if (detailCmd.ReceiptVoucherLineTaxes != null &&
                    detailCmd.ReceiptVoucherLineTaxes.Count > 0)
                {
                    foreach (var taxValue in detailCmd.ReceiptVoucherLineTaxes)
                    {
                        var applyTaxValue = taxValue.ShallowCopy();
                        rptVoucherDetailCommand.ReceiptVoucherLineTaxes.Add(taxValue);
                    }
                }

                var rptVchrDetailEntity = new ReceiptVoucherDetail(rptVoucherDetailCommand);
                _receiptVoucherDetails.Add(rptVchrDetailEntity);
            }
        }

        public void UpdateReceiptVoucherDetail(CUReceiptVoucherDetailCommand command)
        {
            var updateModel = this.ReceiptVoucherDetails.FirstOrDefault(r => r.Id == command.Id);
            if (updateModel != null)
            {
                updateModel.UpdatedBy = this.UpdatedBy;
                updateModel.UpdatedDate = DateTime.UtcNow;
                updateModel.Update(command);
            }
        }

        public void AddReceiptVoucherDetail(ReceiptVoucherDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.CurrencyUnitId = this.CurrencyUnitId;
            receiptVoucherDetail.CurrencyUnitCode = this.CurrencyUnitCode;
            _receiptVoucherDetails.Add(receiptVoucherDetail);
        }

        private void OpeningDebtCollectingHandler()
        {
            if (this.OpeningDebtHistories.Count > 0 &&
                this.OpeningDebtHistories.Any(r => r.Status != PaymentStatus.Accounted))
            {
                AddDomainEvent(new CollectingOpeningDebtDomainEvent()
                {
                    IssuedDate = this.IssuedDate,
                    ReceiptVoucherId = this.IdentityGuid,
                    OpeningDebtPayments = this.OpeningDebtHistories,
                    InvoiceDate = this.InvoiceDate,
                    InvoiceReceivedDate = this.InvoiceReceivedDate,
                    PaymentDate = this.PaymentDate
                });
            }
        }

        private void ActivatedDebtPayingHandler()
        {
            AddDomainEvent(new BillingPaymentSuccessEvent
            {
                IsActiveSPST = false,
                OutContractId = this.OutContractId,
                IsFirstVoucherOfContract = this.IsFirstVoucherOfContract,
                VoucherDetails = this.ValidReceiptVoucherDetails.ToList(),
                Promotions = _promotionForReceiptVoucher
            });

            if (this.DebtHistories.Any())
            {
                foreach (var debtHistory in this.DebtHistories)
                {
                    debtHistory.PaymentDate = this.PaymentDate;
                    debtHistory.SetAccountedStatus();
                }
            }

            if (this.OpeningDebtHistories
                .Where(p => p.Status != PaymentStatus.Accounted)
                .Any())
            {
                var paymentDebtEvent = new PaidOpeningDebtDomainEvent()
                {
                    OpeningDebtHistories = this.OpeningDebtHistories.Where(p => p.Status != PaymentStatus.Accounted),
                    CashierFullName = this.ActivatedDebt.CashierFullName,
                    CashierUserId = this.ActivatedDebt.CashierUserId,
                    CashierUserName = this.ActivatedDebt.CashierUserName,
                    AccountingCode = this.AccountingCode,
                    InvoiceCode = this.InvoiceCode,
                    InvoiceDate = this.InvoiceDate,
                    InvoiceReceivedDate = this.InvoiceReceivedDate,
                    PaymentDate = this.PaymentDate,
                    ReceiptVoucherId = this.IdentityGuid,
                    ApprovedUserId = this.ApprovedUserId
                };
                AddDomainEvent(paymentDebtEvent);
            }
        }

        public void AddPaymentDetail(CuOpeningDebtPaymentCommand command)
        {
            if (command != null
                && command.Id == this.ActivatedDebt.Id)
            {
                if (command.PaymentDetails == null || command.PaymentDetails.Count == 0) return;

                command.PaymentDetails.ForEach(d =>
                {
                    bool force = command.PaymentDetails.Any(p => p.PaymentTurn == d.PaymentTurn && p.PaidAmount > 0);
                    this.ActivatedDebt.AddPaymentDetail(d);
                });
                this.CalculateNumberOfPaymentDetails();
            }
        }

        public void UpdatePaymentDetail(CuOpeningDebtPaymentCommand command)
        {
            if (this.ActivatedDebt != null
                && this.ActivatedDebt.Id > 0
                && this.ActivatedDebt.Id == command.Id)
            {
                if (command.PaymentDetails == null ||
                    command.PaymentDetails.Count == 0 ||
                    command.PaymentDetails.All(p => !p.PaidAmount.HasValue || p.PaidAmount.Value <= 0))
                {
                    this.ActivatedDebt.ClearPaymentDetails();
                    return;
                }

                command.PaymentDetails.ForEach(d =>
                {
                    bool force = command.PaymentDetails.Any(p => p.PaymentTurn == d.PaymentTurn && p.PaidAmount > 0);
                    this.ActivatedDebt.AddOrUpdatePaymentDetail(d, force);
                });
            }
            else if (!string.IsNullOrEmpty(command.CashierUserId)
                && command.CashierUserId.Equals(this.ActivatedDebt.CashierUserId))
            {
                command.PaymentDetails.ForEach(d =>
                {
                    bool force = command.PaymentDetails.Any(p => p.PaymentTurn == d.PaymentTurn && p.PaidAmount > 0);
                    this.ActivatedDebt.AddOrUpdatePaymentDetail(d, force);
                });
            }
            else if (string.IsNullOrEmpty(command.CashierUserId))
            {
                var targetDebtPayment = this._debtHistories
                    .FirstOrDefault(d => string.IsNullOrEmpty(d.CashierUserId));
                if (targetDebtPayment != null)
                {
                    command.PaymentDetails.ForEach(d =>
                    {
                        bool force = command.PaymentDetails.Any(p => p.PaymentTurn == d.PaymentTurn && p.PaidAmount > 0);
                        targetDebtPayment.AddOrUpdatePaymentDetail(d, force);
                    });
                }
                else
                {
                    AddDebtHistoryWithoutCashier();
                    command.PaymentDetails.ForEach(d =>
                    {
                        bool force = command.PaymentDetails.Any(p => p.PaymentTurn == d.PaymentTurn && p.PaidAmount > 0);
                        this.ActivatedDebt.AddOrUpdatePaymentDetail(d, force);
                    });
                }
            }

            this.CalculateNumberOfPaymentDetails();
        }

        public void ClearPaymentDetails()
        {
            this.ActivatedDebt?.ClearPaymentDetails();
        }

        public void AddDebtPaymentDetail(CuOpeningDebtPaymentCommand command)
        {
            if (command.PaymentDetails == null
                   || command.PaymentDetails.All(p => !p.PaidAmount.HasValue || p.PaidAmount.Value <= 0))
            {

                this.OpeningDebtAmount += command.OpeningTargetDebtTotal;
                return;
            }

            var collectingOpeningDebtHis = new ReceiptVoucherDebtHistory()
            {
                IssuedDate = this.IssuedDate,
                ReceiptVoucherCode = command.ReceiptVoucherCode,
                ReceiptVoucherId = command.ReceiptVoucherId,
                ReceiptVoucherContent = command.ReceiptVoucherContent,
                Status = PaymentStatus.CollectionOnBeHalf,
                SubstituteVoucherId = this.Id,
                CashierUserId = this.ActivatedDebt.CashierUserId,
                CashierUserName = this.ActivatedDebt.CashierUserName,
                CashierFullName = this.ActivatedDebt.CashierFullName,
                OpeningTargetDebtTotal = command.OpeningTargetDebtTotal,
                OpeningCashierDebtTotal = command.OpeningCashierDebtTotal,
                CurrencyUnitCode = this.CurrencyUnitCode,
                CreatedBy = this.CreatedBy
            };

            foreach (var paymentDetail in command.PaymentDetails)
            {
                collectingOpeningDebtHis.AddPaymentDetail(paymentDetail);
            }

            this._openingDebtHistories.Add(collectingOpeningDebtHis);
        }

        public void UpdateDebtPaymentDetail(CuOpeningDebtPaymentCommand command)
        {
            var targetOpeningDebtHis = _openingDebtHistories.FirstOrDefault(d => d.Id == command.Id);
            if (targetOpeningDebtHis == null)
            {
                this.AddDebtPaymentDetail(command);
            }
            else
            {
                foreach (var paymentDetail in command.PaymentDetails)
                {
                    if (command.Id == default)
                    {
                        targetOpeningDebtHis.AddPaymentDetail(paymentDetail);
                    }
                    else
                    {
                        targetOpeningDebtHis.AddOrUpdatePaymentDetail(paymentDetail);
                    }
                }
            }
        }

        public void ClearDebtPaymentDetails()
        {
            this._openingDebtHistories?.Clear();
        }

        public void RemoveReceiptVoucherLine(string lineId)
        {
            var toRemoveElm = _receiptVoucherDetails.Find(s => s.IdentityGuid == lineId);
            _receiptVoucherDetails.Remove(toRemoveElm);
        }
        public void SetPromotionForReceiptVoucher(PromotionForReceiptVoucher promotionForReceiptNews)
        {
            _promotionForReceiptVoucher.Add(promotionForReceiptNews);
        }
        public void RemovePromotion(List<int> Ids)
        {
            foreach (var Id in Ids)
            {
                var toRemoveElm = _promotionForReceiptVoucher.Find(e => e.Id == Id);
                _promotionForReceiptVoucher.Remove(toRemoveElm);
            }

        }

        public void UpdateReceiptVoucherDetailByNextBilling(string lineId, DateTime nextBilling, int paymentPeriod)
        {
            var toUpdateElm = _receiptVoucherDetails.Find(s => s.IdentityGuid == lineId);
            toUpdateElm.StartBillingDate = nextBilling;
            toUpdateElm.EndBillingDate = toUpdateElm.StartBillingDate.Value.AddMonths(paymentPeriod).AddDays(-1);
        }

        public void PassiveDebtCollectingHandler(
            DateTime collectingPeriodDate)
        {
            this.CashierCollectingDate = collectingPeriodDate;
            // Ghi nhận công nợ đầu kỳ cho nhân viên/đại lý thu hộ
            // và khách hàng của kỳ thanh toán mới
            this.CalculateCurrentDebtRemaining();
            this.CalculatePaidTotal();

            if (this.StatusId == ReceiptVoucherStatus.Pending.Id)
            {
                SetCollectOnBehalfStatus();
            }
        }

        public void PassiveDebtPayingHandler(string substituteVoucherId,
            string accountedUserId,
            string accountingCode,
            string invoiceCode,
            DateTime? invoicedDate,
            DateTime? invoiceReceivedDate,
            DateTime? paymentDate)
        {

            foreach (var debtHistory in this.DebtHistories.Where(p => p.SubstituteVoucherId > 0
                        && p.SubstituteVoucherId.Equals(substituteVoucherId) && p.Status != PaymentStatus.Accounted))
            {
                var payingPaymentDetails = debtHistory.PaymentDetails
                    .ToList();
                debtHistory.SetAccountedStatus();
            }

            if (string.IsNullOrEmpty(AccountingCode))
            {
                this.AccountingCode = accountingCode;
            }

            if (string.IsNullOrEmpty(InvoiceCode))
            {
                this.InvoiceCode = invoiceCode;
            }

            if (InvoiceDate == null)
            {
                this.InvoiceDate = invoicedDate;
            }

            if (InvoiceReceivedDate == null)
            {
                this.InvoiceReceivedDate = invoiceReceivedDate;
            }

            if (PaymentDate == null)
            {
                PaymentDate = paymentDate;
            }

            if (string.IsNullOrEmpty(this.ApprovedUserId))
            {
                ApprovedUserId = accountedUserId;
            }

            if (this.StatusId == ReceiptVoucherStatus.CollectOnBehalf.Id)
            {
                SetSuccessStatus();
            }

            this.CalculatePaidTotal();
        }

        public void ClearReceiptVoucherLines()
        {
            _receiptVoucherDetails.Clear();
        }

        /// <summary>
        /// Gán nhân viên thu tiền(thu hộ) 
        /// đồng thời chuyển nợ đầu kỳ nếu có sang nhân viên thu tiền mới
        /// </summary>
        /// <param name="userId">Id nhân viên</param>
        /// <param name="userName">Tài khoản nhân viên</param>
        /// <param name="userFullName">Họ tên nhân viên</param>
        public void SetCashierUser(string userId,
            string userName,
            string userFullName,
            bool isAutomaticGenerate = false)
        {
            //if ((string.IsNullOrWhiteSpace(userId) && !this.IsFirstVoucherOfContract)||
            //if (string.IsNullOrWhiteSpace(userId)  || (this.ActivatedDebt != null && userId.Equals(this.ActivatedDebt.CashierUserId))) return;


            if (this.ActivatedDebt != null)
            {
                if (this.StatusId == ReceiptVoucherStatus.Pending.Id
                 || this.StatusId == ReceiptVoucherStatus.Overdue.Id)
                {
                    this.ActivatedDebt.CashierUserId = userId;
                    this.ActivatedDebt.CashierUserName = userName;
                    this.ActivatedDebt.CashierFullName = userFullName;
                    return;
                }
            }
            else
            {
                AddNewDebtHistory(userId, userName, userFullName, isAutomaticGenerate);
            }
        }

        public void AddNewDebtHistory(string userId, string userName, string userFullName, bool isAutomaticGenerate = false)
        {
            var newDebtHistory = new ReceiptVoucherDebtHistory
            {
                IdentityGuid = Guid.NewGuid().ToString(),
                IssuedDate = this.IssuedDate,
                CashierUserId = userId,
                CashierUserName = userName,
                CashierFullName = userFullName,
                CreatedBy = this.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                ReceiptVoucherId = this.Id,
                ReceiptVoucherCode = this.VoucherCode,
                ReceiptVoucherContent = this.Content,
                Status = PaymentStatus.Assigned,
                IsActive = true,
                IsAutomaticGenerate = isAutomaticGenerate,
                CurrencyUnitCode = this.CurrencyUnitCode
            };
            this._debtHistories.Add(newDebtHistory);
        }

        private void AddDebtHistoryWithoutCashier()
        {
            var newDebtHistory = new ReceiptVoucherDebtHistory
            {
                IdentityGuid = Guid.NewGuid().ToString(),
                IssuedDate = this.IssuedDate,
                CashierUserId = null,
                CashierUserName = null,
                CashierFullName = null,
                CreatedBy = this.CreatedBy,
                CreatedDate = DateTime.UtcNow,
                ReceiptVoucherId = this.Id,
                ReceiptVoucherCode = this.VoucherCode,
                ReceiptVoucherContent = this.Content,
                Status = PaymentStatus.Assigned,
                IsActive = true,
                CurrencyUnitCode = this.CurrencyUnitCode,
                IsAutomaticGenerate = false
            };
            this._debtHistories.Add(newDebtHistory);
        }

        public void CalculateCurrentDebtRemaining()
        {
            if (ActivatedDebt != null && ActivatedDebt.IsOpeningDebtRecorded) return;

            // Ghi nhận công nợ đầu vào của kỳ thanh toán mới
            ActivatedDebt.OpeningTargetDebtTotal = this.TargetDebtRemaningTotal;
            ActivatedDebt.OpeningCashierDebtTotal = this.CashierDebtRemaningTotal;
            ActivatedDebt.IsOpeningDebtRecorded = true;
        }

        /// <summary>
        /// Tính tổng các thành phần và thành tiền cuối cùng của phiếu thu
        /// </summary>
        /// <param name="taxCategories"></param>
        public override void CalculateTotal(bool overrideTotal = false)
        {
            if (ValidReceiptVoucherDetails != null && ValidReceiptVoucherDetails.Any())
            {
                this.SubTotalBeforeTax = ValidReceiptVoucherDetails.Sum(e => e.SubTotalBeforeTax);
                this.SubTotal = ValidReceiptVoucherDetails.Sum(e => e.SubTotal);
                this.InstallationFee = ValidReceiptVoucherDetails.Sum(e => e.InstallationFee);
                this.EquipmentTotalAmount = ValidReceiptVoucherDetails.Sum(e => e.EquipmentTotalAmount);
                this.OtherFee = ValidReceiptVoucherDetails.Sum(e => e.OtherFeeTotal);
                this.PromotionTotalAmount = ValidReceiptVoucherDetails.Sum(e => e.PromotionAmount);
                // Giảm trừ nâng hạ gói cước/dịch vụ
                this.OffsetUpgradePackageAmount = ValidReceiptVoucherDetails.Sum(e => e.OffsetUpgradePackageAmount);
                // Giảm trừ tạm ngưng
                this.DiscountAmountSuspendTotal = ValidReceiptVoucherDetails.Sum(e => e.DiscountAmountSuspend);
                //Tổng tất cả các giá trị giảm trừ
                this.ReductionFreeTotal = ValidReceiptVoucherDetails.Sum(e => e.ReductionFee);

                // Tính tổng giá trị trước thuế, giá trị thuế nếu là phiếu thu tự sinh theo định kỳ
                // và không ghi đè lại từ command(người dùng sửa giá trị thuế và tổng tiền)
                if (!overrideTotal && (this.IsAutomaticGenerate || this.IsFirstVoucherOfContract))
                {
                    this.TaxAmount = this.ValidReceiptVoucherDetails.Sum(r => r.TaxAmount);
                    this.GrandTotalBeforeTax = ValidReceiptVoucherDetails.Sum(r => r.GrandTotalBeforeTax);
                }
                else
                {
                    if (this.ReceiptVoucherDetails != null && this.ReceiptVoucherDetails.Count == 1)
                    {
                        var vchrDetail = this.ReceiptVoucherDetails.First();
                        vchrDetail.TaxAmount = this.TaxAmount;
                        vchrDetail.GrandTotalBeforeTax = this.GrandTotalBeforeTax;
                        vchrDetail.GrandTotal = vchrDetail.TaxAmount + vchrDetail.GrandTotalBeforeTax;
                    }
                }

                this.GrandTotal = this.GrandTotalBeforeTax + this.TaxAmount;
                if (this.ReductionFreeTotal > 0)
                {
                    this.GrandTotal = this.GrandTotal > this.ReductionFreeTotal
                        ? this.GrandTotal - this.ReductionFreeTotal
                        : 0;
                }

                if (this.IsHasCollectionFee)
                {
                    this.GrandTotal += this.CODCollectionFee;
                }

                // Số tiền cuối cùng của phiếu thu
                this.GrandTotal = CurrencyUnit.RoundByCurrency(
                    this.CurrencyUnitId, this.GrandTotal);

                // Tính toán giá trị chiết khấu
                if (Discount != null && (Discount.Percent > 0 || Discount.Amount > 0))
                {
                    if (Discount.Type) // discount by percent
                    {
                        Discount.Amount = CurrencyUnit
                            .RoundByCurrency(this.CurrencyUnitId, GrandTotal * (decimal)Discount.Percent / 100);
                    }
                    else // discount by amount
                    {
                        Discount.Percent = (float)(Discount.Amount * 100 / GrandTotal);
                    }

                    GrandTotalBeforeTax -= Discount.Amount;
                    GrandTotal -= Discount.Amount;
                }

                this.GrandTotalIncludeDebt = this.GrandTotal + this.OpeningDebtAmount;

                // Tính toán số tiền thực thu và tổng tiền còn lại
                this.CalculatePaidTotal();
            }
        }

        /// <summary>
        /// Tính tổng tiền đã thu được(có thể từ nhiều đợt thu tiền)
        /// </summary>
        public void CalculatePaidTotal()
        {
            //Tổng nợ đầu kỳ đã thu
            this.OpeningDebtPaidAmount = this.OpeningDebtHistories
                .SelectMany(o => o.PaymentDetails)
                .Sum(dp => dp.PaidAmount);

            if (this.DebtHistories != null && this.DebtHistories.Any())
            {
                // Tổng số tiền đã thu không bao gồm bù trừ công nợ
                this.CashTotal = this.DebtHistories
                    .SelectMany(p => p.PaymentDetails)
                    .Sum(e => e.PaidAmount);
            }

            // Tổng tiền đã thu của phiếu bao gồm bù trừ công nợ

            this.PaidTotal = this.CashTotal + this.ClearingTotal;


            // Số tiền còn lại phải thu của phiếu thu
            this.RemainingTotal = this.GrandTotal - this.PaidTotal;

            var paidCashTotal = this.DebtHistories.Sum(d => d.CashingPaidTotal);
            var transferCashTotal = this.DebtHistories.Sum(d => d.TransferringPaidTotal);
            var accountedCashTotal = this.DebtHistories.Sum(d => d.CashingAccountedTotal);
            var accountedTransferTotal = this.DebtHistories.Sum(d => d.TransferringAccountedTotal);

            // Tính tổng công nợ của khách hàng
            // = số tiền khách hàng phải trả - số tiền đã hạch toán/bù trừ/giảm trừ
            this.TargetDebtRemaningTotal = this.GrandTotal
                - paidCashTotal
                - accountedTransferTotal
                - this.ClearingTotal;

            //khi thanh toán cũng cần trừ số tiền còn lại trên phiếu thu
            //this.RemainingTotal = this.GrandTotal
            //    - this.CashTotal
            //    - this.ClearingTotal;

            // Tính tổng công nợ của nhân viên/đại lý thu hộ
            // = số tiền đã thu của khách hàng nhưng chưa hạch toán với kế toán
            if (this.ActivatedDebt != null && !string.IsNullOrEmpty(this.ActivatedDebt.CashierUserId))
            {
                this.CashierDebtRemaningTotal = paidCashTotal - accountedCashTotal;
            }
            else
            {
                this.CashierDebtRemaningTotal = 0;
            }

            // Tính tổng số bản ghi thanh toán chi tiết theo phương thức thanh toán
            this.CalculateNumberOfPaymentDetails();
        }

        private void CalculateNumberOfPaymentDetails()
        {
            // Số bản ghi lịch sử thu ngân công nợ
            this.NumberOfDebtHistories = this.DebtHistories.Count();
            // Số bản ghi lịch sử thu ngân công nợ đầu kỳ
            this.NumberOfOpeningDebtHistories = this.DebtHistories.Count();
        }

    }
}
