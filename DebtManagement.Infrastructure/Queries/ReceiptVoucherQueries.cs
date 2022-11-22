using Dapper;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.Models;
using GenericRepository;
using Global.Models.PagedList;
using System.Collections.Generic;
using System.Linq;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.FilterModels;
using DebtManagement.Domain.Models.ReceiptVoucherModels;
using GenericRepository.Extensions;
using System;
using Microsoft.Extensions.Configuration;
using GenericRepository.DapperSqlBuilder;
using System.Data;
using DebtManagement.Domain.Models.DebtModels.OutDebts;
using Global.Models.Auth;
using System.Diagnostics;
using DebtManagement.Domain.Models.ReportModels;
using AutoMapper;
using DebtManagement.Domain.Models.ExportInvoiceFilesModels;
using System.Threading.Tasks;
using DebtManagement.Domain.Models.PrintModels.Debts;
using DebtManagement.Domain.Models.PaymentVoucherModels;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;

namespace DebtManagement.Infrastructure.Queries
{
    public interface IReceiptVoucherQueries : IQueryRepository
    {
        int GetLastVoucherId();
        HashSet<string> GetVoucherCodeFromIssuedDate(DateTime startDate);
        HashSet<string> GetVoucherCodeFromStartingId(int startingId);
        int GetOrderNumberByDate(DateTime startingIssuedDate);
        int GetOrderNumberByNow();
        Dictionary<DateTime, int> GetOrderNumberByIssuedDate(DateTime? startingIssuedDate = null);
        ReceiptVoucherDTO Find(int id);
        IEnumerable<ConcludeFinancialReport> FinancialReport(ReceiptVoucherFilterModel requestFilterModel);
        IPagedList<ReceiptVoucherGridDTO> GetList(ReceiptVoucherBothProjectNullFilterModel requestFilterModel);
        Task<IEnumerable<ReceiptVoucherPrintModel>> GetForPrinting(string voucherIds);
        IEnumerable<ReceiptVoucherDTO> GetOverdueVouchers();
        IEnumerable<ReceiptVoucherDTO> GetDueVouchers();
        IEnumerable<ReceiptVoucherDTO> GetListByTargetId(int targetId, int clearingId);
        IEnumerable<ReceiptVoucherDTO> GetListByTargetUid(string targetId, int clearingId);
        ReceiptVoucherDTO PrintVoucherById(int id);
        IEnumerable<CollectedVouchersDTO> GetCollectedAndUnCollectedVoucherByMonth(CollectedVoucherFilter voucherFilter);
        string GetReceiptVoucherCode(DateTime issuedDate, string projectCode, string marketAreaCode, bool isEnterprise, int? voucherIdx = null);
        IEnumerable<ReceiptVoucherSharingRevenueDTO> GetTotalSharingRevenusByReceiptVoucher(int outContractIds, string startBillingDate, string endBillingDate, int currencyUnitId, int paymentVoucherId = 0, int skip = 0);
        IEnumerable<ReceiptVoucherDTO> GetTotalSharingRevenusByReceiptVoucher2(string[] outContractIds, string startBillingDate, string endBillingDate, int currencyUnitId, int paymentVoucherId = 0);
        PagedList<ReportTotalRevenueRaw> GetListTotalRevenue(ReportTotalRevenueFilter filterModel);
        IEnumerable<int> GetReceiptVoucherIds(string outContractCode);
        IPagedList<ExportInvoiceFileModel> GetInvoiceReportData(ExportInvoiceFilterModel filter);
        IPagedList<TotalRevenuePersonalDTO> GetTotalRevenueEnterpiseReport(PaymentVoucherReportFilter filterModel);
        IPagedList<FTTHProjectRevenueDTO> GetFTTHProjectRevenue(PaymentVoucherReportFilter filterModel);
    }

    public class ReceiptVoucherSqlBuilder : SqlBuilder
    {
        public ReceiptVoucherSqlBuilder() { }
        public ReceiptVoucherSqlBuilder(string tableName) : base(tableName)
        {
        }

        public ReceiptVoucherSqlBuilder SelectCashier(string alias)
        {
            Select($"{alias}.`CashierUserId` AS `CashierUserId`");
            Select($"{alias}.`CashierUserName` AS `CashierUserName`");
            Select($"{alias}.`CashierFullName` AS `CashierFullName`");
            return this;
        }

        public ReceiptVoucherSqlBuilder SelectVoucherDetail(string alias)
        {
            Select($"{alias}.Id");
            Select($"{alias}.ReceiptVoucherId");
            Select($"{alias}.CurrencyUnitId");
            Select($"{alias}.CurrencyUnitCode");
            Select($"{alias}.OutContractServicePackageId");
            Select($"{alias}.ServiceId");
            Select($"{alias}.ServiceName");
            Select($"{alias}.ServicePackageId");
            Select($"{alias}.ServicePackageName");
            Select($"{alias}.StartBillingDate");
            Select($"{alias}.EndBillingDate");
            Select($"{alias}.TaxPercent");
            Select($"{alias}.TaxAmount");
            Select($"{alias}.SubTotalBeforeTax");
            Select($"{alias}.SubTotal");
            Select($"{alias}.EquipmentTotalAmount");
            Select($"{alias}.InstallationFee");
            Select($"{alias}.PromotionAmount");
            Select($"{alias}.PackagePrice");
            Select($"{alias}.OtherFeeTotal");
            Select($"{alias}.ReductionFee");
            Select($"{alias}.GrandTotalBeforeTax");
            Select($"{alias}.GrandTotal");
            Select($"{alias}.UsingMonths");
            Select($"{alias}.IsFirstDetailOfService");
            Select($"{alias}.CId");
            Select($"{alias}.HasDistinguishBandwidth");
            Select($"{alias}.HasStartAndEndPoint");
            Select($"{alias}.DomesticBandwidth");
            Select($"{alias}.InternationalBandwidth");
            Select($"{alias}.PricingType");
            Select($"{alias}.OverloadUsageDataPrice");
            Select($"{alias}.IOverloadUsageDataPrice");
            Select($"{alias}.ConsumptionBasedPrice");
            Select($"{alias}.IConsumptionBasedPrice");
            Select($"{alias}.DataUsage");
            Select($"{alias}.DataUsageUnit");
            Select($"{alias}.IDataUsageUnit");
            Select($"{alias}.IDataUsage");
            Select($"{alias}.UsageDataAmount");
            Select($"{alias}.IUsageDataAmount");
            Select($"{alias}.IsMainPaymentChannel");
            Select($"{alias}.IsJoinedPayment");
            Select($"{alias}.IsShow");
            return this;
        }

        public ReceiptVoucherSqlBuilder SelectVoucherTarget(string alias)
        {
            Select($"{alias}.Id");
            Select($"{alias}.TargetFullName");
            Select($"{alias}.TargetCode");
            Select($"{alias}.TargetPhone");
            Select($"{alias}.TargetEmail");
            Select($"{alias}.TargetFax");
            Select($"{alias}.TargetIdNo");
            Select($"{alias}.TargetTaxIdNo");
            Select($"{alias}.IsEnterprise");
            Select($"{alias}.IsPayer");
            Select($"{alias}.City");
            Select($"{alias}.CityId");
            Select($"{alias}.District");
            Select($"{alias}.DistrictId");
            Select($"{alias}.TargetAddress");
            return this;
        }

        public ReceiptVoucherSqlBuilder SelectDetailReduction(string alias)
        {
            Select($"{alias}.Id");
            Select($"{alias}.ReceiptVoucherId");
            Select($"{alias}.ReceiptVoucherDetailId");
            Select($"{alias}.ReasonId");
            Select($"{alias}.ReductionReason");
            Select($"{alias}.StartTime");
            Select($"{alias}.StopTime");
            Select($"{alias}.Duration");
            return this;
        }
        public ReceiptVoucherSqlBuilder SelectPromotion(string alias)
        {
            Select($"{alias}.Id");
            Select($"{alias}.ReceiptVoucherId");
            Select($"{alias}.PromotionId");
            Select($"{alias}.PromotionDetailId");
            Select($"{alias}.PromotionType");
            Select($"{alias}.OutContractServicePackageId");
            Select($"{alias}.PromotionAmount");
            Select($"{alias}.PromotionValue");
            Select($"{alias}.PromotionValueType");
            Select($"{alias}.NumberMonthApplied");
            Select($"{alias}.IsApplied");
            return this;
        }

        public ReceiptVoucherSqlBuilder SelectDebtHistory(string alias)
        {
            Select($"{alias}.Id");
            Select($"{alias}.IssuedDate");
            Select($"{alias}.Status");
            Select($"{alias}.ReceiptVoucherId");
            Select($"{alias}.ReceiptVoucherCode");
            Select($"{alias}.ReceiptVoucherContent");
            Select($"{alias}.SubstituteVoucherId");
            Select($"{alias}.CashierUserId");
            Select($"{alias}.CashierUserName");
            Select($"{alias}.CashierFullName");
            Select($"{alias}.OpeningTargetDebtTotal");
            Select($"{alias}.OpeningCashierDebtTotal");
            Select($"{alias}.CreatedDate");
            Select($"{alias}.PaymentDate");
            Select($"{alias}.CurrencyUnitCode");

            return this;
        }

        public ReceiptVoucherSqlBuilder SelectPaymentDetail(string alias)
        {
            Select($"{alias}.`Id`");
            Select($"{alias}.`DebtHistoryId`");
            Select($"{alias}.`ReceiptVoucherId`");
            Select($"{alias}.`Status`");
            Select($"{alias}.`PaymentMethod`");
            Select($"{alias}.`PaymentMethodName`");
            Select($"{alias}.`PaidAmount`");
            Select($"{alias}.`CashierUserId`");
            Select($"{alias}.`CurrencyUnitCode`");
            Select($"{alias}.`PaymentTurn`");
            Select($"{alias}.`CreatedDate` AS PaymentDate");

            return this;
        }

        public ReceiptVoucherSqlBuilder WhereValidVoucher(UserIdentity currentUser)
        {
            Where("t1.IsActive = True");

            if (!currentUser.Roles.Contains("ADMIN"))
            {
                Where("t1.InvalidIssuedDate <> TRUE && t1.IssuedDate IS NOT NULL");
                Where("DATE(t1.IssuedDate) <= DATE(@currentDate)", new { currentDate = DateTime.Now.AddHours(7) });
            }

            return this;
        }

        public ReceiptVoucherSqlBuilder JoinCurrentCashier()
        {
            LeftJoin("ReceiptVoucherDebtHistories dh ON dh.Id = (" +
                " SELECT Id FROM ReceiptVoucherDebtHistories" +
                " WHERE ReceiptVoucherId = t1.Id" +
                " ORDER BY CreatedDate DESC" +
                " LIMIT 1" +
                ")");

            return this;
        }
    }

    public class ReceiptVoucherQueries : QueryRepository<ReceiptVoucher, int>, IReceiptVoucherQueries
    {
        private readonly string ACCOUNTANT_CONFIRMATION = "ACCOUNTANT_CONFIRMATION";
        private readonly IConfiguration _configuration;
        private readonly IOutDebtManagementQueries _outDebtManagementQueries;
        private readonly IMapper _mapper;
        public ReceiptVoucherQueries(DebtDbContext context,
            IConfiguration configuration,
            IOutDebtManagementQueries outDebtManagementQueries,
            IMapper mapper = null) : base(context)
        {
            _configuration = configuration;
            _outDebtManagementQueries = outDebtManagementQueries;
            _mapper = mapper;
        }

        public ReceiptVoucherDTO Find(int id)
        {
            var cached = new Dictionary<int, ReceiptVoucherDTO>();
            var dapperExecution = BuildByTemplate<ReceiptVoucherDTO, ReceiptVoucherSqlBuilder>();
            // 6: Select Clearings
            dapperExecution.SqlBuilder.Select("c.`CodeClearing`");
            dapperExecution.SqlBuilder.Select("t3.`IsEnterprise` AS `IsEnterprise`");

            dapperExecution.SqlBuilder.Select("t1.`Payment_Form` AS `Form`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Method` AS `Method`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Address` AS `Address`");
            dapperExecution.SqlBuilder.SelectVoucherDetail("t2");
            dapperExecution.SqlBuilder.SelectVoucherTarget("t3");
            // 4: Select tax categories
            dapperExecution.SqlBuilder.Select("t.`Id`");
            dapperExecution.SqlBuilder.Select("t.`VoucherLineId`");
            dapperExecution.SqlBuilder.Select("t.`TaxName`");
            dapperExecution.SqlBuilder.Select("t.`TaxCode`");
            dapperExecution.SqlBuilder.Select("t.`TaxValue`");
            // 5: Select incurred debt payment details
            dapperExecution.SqlBuilder.SelectDebtHistory("idh");
            // 6
            dapperExecution.SqlBuilder.SelectPaymentDetail("pd");
            // 7: Select opening debt payment details
            dapperExecution.SqlBuilder.SelectDebtHistory("odh");
            // 8
            dapperExecution.SqlBuilder.SelectPaymentDetail("opd");
            //9
            dapperExecution.SqlBuilder.SelectDetailReduction("dr");

            //10
            dapperExecution.SqlBuilder.SelectPromotion("pr");
            //
            // 9: Select opening debt by voucher
            //dapperExecution.SqlBuilder.Select("srv.Id AS `ReceiptVoucherId`");
            //dapperExecution.SqlBuilder.Select("srv.VoucherCode AS `ReceiptVoucherCode`");
            //dapperExecution.SqlBuilder.Select("srv.Content AS `ReceiptVoucherContent`");
            //dapperExecution.SqlBuilder.Select("srv.IssuedDate AS `IssuedDate`");
            //dapperExecution.SqlBuilder.Select("srv.GrandTotal AS `GrandTotal`");
            //dapperExecution.SqlBuilder.Select("srv.RemainingTotal AS `RemainingTotal`");

            //11
            dapperExecution.SqlBuilder.Select("af.Id");
            dapperExecution.SqlBuilder.Select("af.Name");
            dapperExecution.SqlBuilder.Select("af.ReceiptVoucherId");
            dapperExecution.SqlBuilder.Select("af.ReceiptVoucherDetailId");
            dapperExecution.SqlBuilder.Select("af.FileName");
            dapperExecution.SqlBuilder.Select("af.FilePath");
            dapperExecution.SqlBuilder.Select("af.Size");
            dapperExecution.SqlBuilder.Select("af.FileType");
            dapperExecution.SqlBuilder.Select("af.Extension");
            dapperExecution.SqlBuilder.Select("af.RedirectLink");

            //12: Channel price bus table
            dapperExecution.SqlBuilder.Select("bt.`Id` AS `Id`");
            dapperExecution.SqlBuilder.Select("bt.`CurrencyUnitCode` AS `CurrencyUnitCode`");
            dapperExecution.SqlBuilder.Select("bt.`ChannelId` AS `ChannelId`");
            dapperExecution.SqlBuilder.Select("bt.`ReceiptVoucherLineId` AS `ReceiptVoucherLineId`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueFrom` AS `UsageValueFrom`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueFromUomId` AS `UsageValueFromUomId`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueTo` AS `UsageValueTo`");
            dapperExecution.SqlBuilder.Select("bt.`UsageValueToUomId` AS `UsageValueToUomId`");
            dapperExecution.SqlBuilder.Select("bt.`BasedPriceValue` AS `BasedPriceValue`");
            dapperExecution.SqlBuilder.Select("bt.`PriceValue` AS `PriceValue`");
            dapperExecution.SqlBuilder.Select("bt.`PriceUnitUomId` AS `PriceUnitUomId`");
            dapperExecution.SqlBuilder.Select("bt.`UsageBaseUomValueTo` AS `UsageBaseUomValueTo`");
            dapperExecution.SqlBuilder.Select("bt.`UsageBaseUomValueFrom` AS `UsageBaseUomValueFrom`");
            dapperExecution.SqlBuilder.Select("bt.`IsDomestic` AS `IsDomestic`");

            //13: BusTable pricing calculators
            dapperExecution.SqlBuilder.Select("cal.Id AS `Id`");
            dapperExecution.SqlBuilder.Select("cal.`CurrencyUnitCode` AS `CurrencyUnitCode`");
            dapperExecution.SqlBuilder.Select("cal.ChannelId AS `ChannelId`");
            dapperExecution.SqlBuilder.Select("cal.ChannelCid AS `ChannelCid`");
            dapperExecution.SqlBuilder.Select("cal.ReceiptVoucherLineId AS `ReceiptVoucherLineId`");
            dapperExecution.SqlBuilder.Select("cal.Day AS `Day`");
            dapperExecution.SqlBuilder.Select("cal.UsageDataByBaseUnit AS `UsageDataByBaseUnit`");
            dapperExecution.SqlBuilder.Select("cal.UsageData AS `UsageData`");
            dapperExecution.SqlBuilder.Select("cal.UsageDataUnit AS `UsageDataUnit`");
            dapperExecution.SqlBuilder.Select("cal.Price AS `Price`");
            dapperExecution.SqlBuilder.Select("cal.TotalAmount AS `TotalAmount`");
            dapperExecution.SqlBuilder.Select("cal.IsDomestic AS `IsDomestic`");

            dapperExecution.SqlBuilder.Select("cal.PricingType AS `PricingType`");
            dapperExecution.SqlBuilder.Select("cal.StartingBillingDate AS `StartingBillingDate`");
            dapperExecution.SqlBuilder.Select("cal.IsMainRcptVoucherLine AS `IsMainRcptVoucherLine`");

            dapperExecution.SqlBuilder.InnerJoin(
                "VoucherTargets t3 ON t1.TargetId = t3.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "ReceiptVoucherDetails t2 ON t1.Id = t2.ReceiptVoucherId");

            dapperExecution.SqlBuilder.LeftJoin(
                "ChannelPriceBusTables bt ON t2.Id = bt.ReceiptVoucherLineId");

            dapperExecution.SqlBuilder.LeftJoin(
                "BusTablePricingCalculators cal ON t2.Id = cal.ReceiptVoucherLineId");

            dapperExecution.SqlBuilder.LeftJoin(
                "ReceiptVoucherDetailReductions dr ON dr.ReceiptVoucherDetailId = t2.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "ReceiptVoucherLineTaxes t ON t.VoucherLineId = t2.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "Clearings c ON c.Id = t1.ClearingId");

            dapperExecution.SqlBuilder.LeftJoin(
                "ReceiptVoucherDebtHistories idh ON t1.Id = idh.ReceiptVoucherId");

            dapperExecution.SqlBuilder.LeftJoin(
                "ReceiptVoucherPaymentDetails pd ON pd.DebtHistoryId = idh.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "ReceiptVoucherDebtHistories odh ON odh.SubstituteVoucherId = t1.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "ReceiptVoucherPaymentDetails opd ON opd.DebtHistoryId = odh.Id");

            dapperExecution.SqlBuilder.LeftJoin(
               "PromotionForReceiptVoucher pr ON pr.ReceiptVoucherId = t1.Id");
            //dapperExecution.SqlBuilder.LeftJoin(
            //    "ReceiptVouchers srv ON dpd.ReceiptVoucherId = srv.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "AttachmentFiles af ON t2.Id = af.ReceiptVoucherDetailId AND af.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            //dapperExecution.SqlBuilder.Where("IFNULL(pd.Id,0) <> 0");
            WithConnection(conn => conn.Query(dapperExecution.ExecutionTemplate.RawSql,
                new[]
                {
                    typeof(ReceiptVoucherDTO),
                    typeof(PaymentMethod),
                    typeof(ReceiptVoucherDetailDTO),
                    typeof(VoucherTargetDTO),
                    typeof(ReceiptVoucherLineTaxDTO),
                    typeof(OpeningDebtByReceiptVoucherModel),
                    typeof(ReceiptVoucherPaymentDetailDTO),
                    typeof(OpeningDebtByReceiptVoucherModel),
                    typeof(ReceiptVoucherPaymentDetailDTO),
                    typeof(ReductionDetailDTO),
                    typeof(PromotionForReceiptVoucherDTO),
                    typeof(AttachmentFileDTO),
                    typeof(ChannelPriceBusTableDTO),
                    typeof(BusTablePricingCalculatorDTO),
                }, results =>
                {
                    var voucherEntry = results[0] as ReceiptVoucherDTO;
                    if (voucherEntry == null) return null;

                    if (!cached.TryGetValue(voucherEntry.Id, out var result))
                    {
                        result = voucherEntry;
                        cached.Add(voucherEntry.Id, voucherEntry);
                    }

                    result.Payment = results[1] as PaymentMethod;

                    if (results[2] is ReceiptVoucherDetailDTO receiptVoucherDetail)
                    {
                        if (result.ReceiptLines.Any(r => r.Id == receiptVoucherDetail.Id))
                        {
                            receiptVoucherDetail = result.ReceiptLines.First(r => r.Id == receiptVoucherDetail.Id);
                        }
                        else
                        {
                            result.ReceiptLines.Add(receiptVoucherDetail);
                        }

                        if (results[4] is ReceiptVoucherLineTaxDTO vchrDetailTax)
                        {
                            if (receiptVoucherDetail.ReceiptVoucherLineTaxes.All(s => s.Id != vchrDetailTax.Id))
                                receiptVoucherDetail.ReceiptVoucherLineTaxes.Add(vchrDetailTax);
                        }

                        if (results[9] is ReductionDetailDTO reductionDetail)
                        {
                            if (receiptVoucherDetail.ReductionDetails.All(s => s.Id != reductionDetail.Id))
                                receiptVoucherDetail.ReductionDetails.Add(reductionDetail);
                        }

                        if (results[11] is AttachmentFileDTO attachmentFile)
                        {
                            if (receiptVoucherDetail.AttachmentFiles.All(s => s.Id != attachmentFile.Id))
                            {
                                receiptVoucherDetail.AttachmentFiles.Add(attachmentFile);
                            }
                        }

                        if (results[12] is ChannelPriceBusTableDTO channelPriceBusTable)
                        {
                            if (receiptVoucherDetail.PriceBusTables.All(s => s.Id != channelPriceBusTable.Id))
                            {
                                receiptVoucherDetail.PriceBusTables.Add(channelPriceBusTable);
                            }
                        }

                        if (results[13] is BusTablePricingCalculatorDTO busTableCalculatorDTO)
                        {
                            if (receiptVoucherDetail.BusTablePricingCalculators.All(s => s.Id != busTableCalculatorDTO.Id))
                            {
                                receiptVoucherDetail.BusTablePricingCalculators.Add(busTableCalculatorDTO);
                            }
                        }
                    }

                    result.Target = results[3] as VoucherTargetDTO;

                    if (results[5] is OpeningDebtByReceiptVoucherModel incurredDebt)
                    {
                        OpeningDebtByReceiptVoucherModel persistentIncurredDebt;
                        if (result.IncurredDebtPayments.All(s => s.Id != incurredDebt.Id))
                        {
                            persistentIncurredDebt = incurredDebt;
                            result.IncurredDebtPayments.Add(persistentIncurredDebt);
                        }
                        else
                        {
                            persistentIncurredDebt = result.IncurredDebtPayments.First(s => s.Id == incurredDebt.Id);
                        }

                        if (results[6] is ReceiptVoucherPaymentDetailDTO debtPaymentDetail)
                        {
                            persistentIncurredDebt.AddPaymentDetail(debtPaymentDetail);
                        }
                    }

                    if (results[7] is OpeningDebtByReceiptVoucherModel openingDebt)
                    {
                        OpeningDebtByReceiptVoucherModel persistentOpeningDebt;
                        if (result.OpeningDebtPayments.All(s => s.Id != openingDebt.Id))
                        {
                            persistentOpeningDebt = openingDebt;
                            result.OpeningDebtPayments.Add(persistentOpeningDebt);
                        }
                        else
                        {
                            persistentOpeningDebt = result.OpeningDebtPayments.First(s => s.Id == openingDebt.Id);
                        }

                        if (results[8] is ReceiptVoucherPaymentDetailDTO debtPaymentDetail)
                        {
                            persistentOpeningDebt.AddPaymentDetail(debtPaymentDetail);
                        }
                    }
                    if (results[10] is PromotionForReceiptVoucherDTO promotion)
                    {
                        if (result.PromotionForReceiptVouchers.All(e => e.PromotionDetailId != promotion.PromotionDetailId))
                        {
                            result.PromotionForReceiptVouchers.Add(promotion);
                        }
                    }
                    return result;
                }, dapperExecution.ExecutionTemplate.Parameters,
                null,
                true,
                "Form,Id,Id,Id,Id,Id,Id,Id,Id,Id,Id,Id,Id"));

            var result = cached.Values.FirstOrDefault();

            var mainRcptVchrDetail = result.ReceiptLines.FirstOrDefault(r => r.IsMainPaymentChannel);
            if (mainRcptVchrDetail != null)
            {
                mainRcptVchrDetail.NumberOfJoinedChannels = 1;
                foreach (var vchrLine in result.ReceiptLines)
                {
                    if (vchrLine.IsJoinedPayment && !vchrLine.IsMainPaymentChannel)
                    {
                        mainRcptVchrDetail.JoinedCids += $", {vchrLine.CId}";
                        if (!string.IsNullOrEmpty(vchrLine.ServiceName))
                        {
                            mainRcptVchrDetail.JoinedServices += $", {vchrLine.ServiceName}";
                            if (!mainRcptVchrDetail.JoinedDistinctServices.Contains(vchrLine.ServiceName))
                            {
                                mainRcptVchrDetail.JoinedDistinctServices += $", {vchrLine.ServiceName}";
                            }
                        }

                        mainRcptVchrDetail.JoinedBandwidth += $", {vchrLine.DomesticBandwidth}" +
                            $"{(!string.IsNullOrEmpty(vchrLine.InternationalBandwidth) ? "/" + vchrLine.InternationalBandwidth : string.Empty)}";

                        mainRcptVchrDetail.JoinedSubTotalBeforeTax += vchrLine.SubTotalBeforeTax;
                        mainRcptVchrDetail.JoinedTaxAmount += vchrLine.TaxAmount;
                        mainRcptVchrDetail.JoinedSubTotal += vchrLine.SubTotal;
                        mainRcptVchrDetail.JoinedGrandTotalBeforeTax += vchrLine.GrandTotalBeforeTax;
                        mainRcptVchrDetail.JoinedGrandTotal += vchrLine.GrandTotal;
                        if (mainRcptVchrDetail.StartBillingDate > vchrLine.StartBillingDate)
                        {
                            mainRcptVchrDetail.StartBillingDate = vchrLine.StartBillingDate;
                        }
                        foreach (var busTableCalculator in vchrLine.BusTablePricingCalculators)
                        {
                            mainRcptVchrDetail.BusTablePricingCalculators.Add(busTableCalculator);
                        }
                        mainRcptVchrDetail.NumberOfJoinedChannels++;
                    }
                }
            }

            /// Không hiển thị các chi tiết thanh toán không cho phép
            result.ReceiptLines.RemoveAll(c => !c.IsShow);

            if (result.IncurredDebtPayments.Any())
            {
                var latestDebtPayment = result.IncurredDebtPayments.OrderByDescending(id => id.CreatedDate).First();
                var latestPaymentTurn = latestDebtPayment.PaymentDetails
                    .Select(c => c.PaymentTurn)
                    .DefaultIfEmpty(0)
                    .Max();

                result.CashierUserId = latestDebtPayment.CashierUserId;
                result.CashierUserName = latestDebtPayment.CashierUserName;
                result.CashierFullName = latestDebtPayment.CashierFullName;

                if (result.StatusId != ReceiptVoucherStatus.SentToAccountant.Id
                    && result.StatusId != ReceiptVoucherStatus.Canceled.Id
                    && result.GrandTotal > result.PaidTotal)
                {
                    foreach (var method in VoucherPaymentMethod.ActiveList())
                    {
                        var defaultPaymentDetail = ReceiptVoucherPaymentDetailDTO.Default(method.Id);
                        defaultPaymentDetail.CashierUserId = latestDebtPayment.CashierUserId;
                        defaultPaymentDetail.PaymentTurn = latestPaymentTurn == 0 ? 1 : latestPaymentTurn + 1;
                        defaultPaymentDetail.PaymentDate = DateTime.Now;
                        latestDebtPayment.AddPaymentDetail(defaultPaymentDetail);
                    }
                }
            }

            // Lấy công nợ đầu kỳ của khách hàng(cá nhân)
            if (!result.IsEnterprise &&
                (result.StatusId == ReceiptVoucherStatus.Pending.Id ||
                    result.StatusId == ReceiptVoucherStatus.CollectOnBehalf.Id))
            {
                var openingDebts = _outDebtManagementQueries.GetOpeningDebtByTarget(result.TargetId, result.IssuedDate, result.Id);
                foreach (var openingDebt in openingDebts)
                {
                    result.OpeningDebtPayments.Add(openingDebt);
                }
            }

            result.CashierReceivedDate = result.CashierReceivedDate ?? DateTime.Now;
            return result;
        }
        public int GetOrderNumberByDate(DateTime issuedDate)
        {
            string sql = "SELECT (COUNT(1) + 1) FROM ReceiptVouchers WHERE DATE(IssuedDate) = DATE(@issuedDate)";
            return WithConnection(conn =>
                conn.QueryFirst<int>(sql, new
                {
                    issuedDate
                }));
        }
        public int GetOrderNumberByNow()
        {
            string sql = "SELECT (COUNT(1) + 1) FROM ReceiptVouchers WHERE DATE(IssuedDate) = CURDATE()";
            return WithConnection(conn =>
                conn.QueryFirst<int>(sql));
        }

        public Dictionary<DateTime, int> GetOrderNumberByIssuedDate(DateTime? startingIssuedDate)
        {
            if (startingIssuedDate.HasValue)
            {
                startingIssuedDate = startingIssuedDate.Value.AddHours(7);
            }

            return WithConnection(conn =>
                conn.Query<(DateTime IssuedDate, int Count)>("GetReceiptVoucherNumberByIssuedDate",
                    new { startingIssuedDate },

                    commandType: CommandType.StoredProcedure))
                .ToDictionary(
                    c => c.IssuedDate,
                    c => c.Count);
        }

        public string GetReceiptVoucherCode(DateTime issuedDate, string projectCode, string marketAreaCode, bool isEnterprise, int? voucherIdx = null)
        {
            var dateTimeLocal = issuedDate.AddHours(7);
            int voucherLatestIndex = voucherIdx ?? this.GetOrderNumberByDate(issuedDate);
            string indexAsString = voucherLatestIndex.ToString().PadLeft(2, '0');
            string projectPart = !string.IsNullOrEmpty(projectCode) ? projectCode + "/" : "";
            string marketAreaPart = !string.IsNullOrEmpty(marketAreaCode) ? marketAreaCode + "/" : "";

            if (isEnterprise)
            {
                return $"PT-{ marketAreaPart.ToUpper()}{ projectPart.ToUpper()}{dateTimeLocal.ToString("yyMMdd")}/{ indexAsString}";
            }
            else
            {
                return $"PT-{ marketAreaPart.ToUpper()}{ projectPart.ToUpper()}{dateTimeLocal.ToString("yyMMdd")}/{ indexAsString}";
            }
        }

        private bool NeedJoinToDetail(ReceiptVoucherFilterModel requestFilterModel)
        {
            var needToOrder =
                    "StartBillingDate".EqualsIgnoreCase(requestFilterModel.OrderBy) ||
                    "EndBillingDate".EqualsIgnoreCase(requestFilterModel.OrderBy);

            var needToFilter = requestFilterModel.Any("ServicePackages") ||
                requestFilterModel.Any("ServiceId") ||
                !string.IsNullOrWhiteSpace(requestFilterModel.ServiceIds) ||
                requestFilterModel.Any("StartBillingDate") ||
                requestFilterModel.Any("EndBillingDate");

            return needToOrder || needToFilter;
        }

        private bool NeedJoinToVoucherTarget(ReceiptVoucherFilterModel requestFilterModel)
        {
            var needToOrder = "TargetId".EqualsIgnoreCase(requestFilterModel.OrderBy) ||
                "TargetCode".EqualsIgnoreCase(requestFilterModel.OrderBy) ||
                "TargetFullName".EqualsIgnoreCase(requestFilterModel.OrderBy);

            var needToFilter = !string.IsNullOrWhiteSpace(requestFilterModel.Keywords) ||
                requestFilterModel.Any("TargetId") ||
                requestFilterModel.Any("TargetCode") ||
                requestFilterModel.Any("TargetFullName") ||
                requestFilterModel.Any("TargetPhone") ||
                requestFilterModel.Any("TargetAddress");
            return needToOrder || needToFilter;
        }

        public IPagedList<ReceiptVoucherGridDTO> GetList(ReceiptVoucherBothProjectNullFilterModel requestFilterModel)
        {
            var sqlInnerTemplate = "SELECT DISTINCT t1.Id FROM `ReceiptVouchers` AS t1" +
                $"/**insightjoin**//**insightleftjoin**//**insightrightjoin**/" +
                $"/**where**/" +
                $"/**innergroupby**//**having**/" +
                $"/**innerorderby**//**take**//**skip**/";

            var sqlTemplate =
                $"SELECT\n/**select**/\nFROM" +
                $"\n(" +
                sqlInnerTemplate +
                $"\n) AS s" +
                $"\nINNER JOIN ReceiptVouchers t1 ON t1.Id = s.Id" +
                $"/**innerjoin**//**leftjoin**//**rightjoin**/" +
                $"/**groupby**//**orderby**/";

            var sqlCoutingTemplate =
                $"\nSELECT COUNT(DISTINCT t1.Id) FROM `ReceiptVouchers` AS t1" +
                $"/**insightjoin**//**insightleftjoin**//**insightrightjoin**//**where**//**innergroupby**//**having**/";


            var cached = new Dictionary<int, ReceiptVoucherGridDTO>();
            var dapperExecution = Build<ReceiptVoucherGridDTO, ReceiptVoucherSqlBuilder>(requestFilterModel);
            dapperExecution.SqlBuilder.Select(@"CASE WHEN IFNULL(t1.IsEnterprise,0) = 0 THEN 'Cá nhân' 
                                                    WHEN t1.IsEnterprise = 1 THEN 'Doanh nghiệp' 
                                                    ELSE 'Chưa phân loại' 
                                                    END AS IsEnterpriseName");
            var needToJoinDetail = this.NeedJoinToDetail(requestFilterModel);
            var needToJoinVoucherTarget = this.NeedJoinToVoucherTarget(requestFilterModel);
            var needToJoinDebtHistory = false;

            #region Select clause building handler
            dapperExecution.SqlBuilder.Select("dh.CashierUserName AS `CashierUserName`");

            dapperExecution.SqlBuilder.Select("t1.`Content`");
            dapperExecution.SqlBuilder.Select("vt.`TargetFullName` AS `TargetFullName`");
            dapperExecution.SqlBuilder.Select("vt.`TargetCode` AS `TargetCode`");
            dapperExecution.SqlBuilder.Select("vt.`TargetPhone` AS `TargetPhone`");
            dapperExecution.SqlBuilder.Select("vt.`TargetAddress` AS `TargetAddress`");
            dapperExecution.SqlBuilder.Select("vt.`IsEnterprise` AS `IsEnterprise`");

            // 2: Payment
            dapperExecution.SqlBuilder.Select("t1.`Payment_Form` AS `Form`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Method` AS `Method`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Address` AS `Address`");

            // 3: Select the receipt voucher detail
            dapperExecution.SqlBuilder.SelectVoucherDetail("rvd");

            dapperExecution.SqlBuilder.LeftJoin("ReceiptVoucherDetails rvd ON t1.Id = rvd.ReceiptVoucherId");
            dapperExecution.SqlBuilder.InnerJoin("VoucherTargets vt ON vt.Id = t1.TargetId");
            dapperExecution.SqlBuilder.LeftJoin("ReceiptVoucherDebtHistories dh ON dh.ReceiptVoucherId = t1.Id");

            #endregion

            #region Where clause building handler

            dapperExecution.SqlBuilder.WhereValidVoucher(this.UserIdentity);

            if (string.Equals(requestFilterModel.Get<string>("managementMode"), ACCOUNTANT_CONFIRMATION, StringComparison.OrdinalIgnoreCase))
            {

                dapperExecution.SqlBuilder.Where("t1.StatusId NOT IN @collectingDebtStatus", new
                {
                    collectingDebtStatus = new int[] {
                    ReceiptVoucherStatus.Success.Id,
                    ReceiptVoucherStatus.Canceled.Id
                    }
                });
                //dapperExecution.SqlBuilder.Where("EXISTS ("
                //    + " SELECT dh.Id FROM ReceiptVoucherDebtHistories dh"
                //    + " WHERE (t1.Id = dh.ReceiptVoucherId OR t1.Id = dh.SubstituteVoucherId)"
                //    + " AND dh.Status <> @accountedStatus"
                //    + " AND dh.CashingPaidTotal + dh.TransferringPaidTotal > 0"
                //    + ")",
                //    new
                //    {
                //        accountedStatus = (int)PaymentStatus.Accounted
                //    }
                //);
            }

            if (!string.IsNullOrEmpty(requestFilterModel.ProjectIds))
            {
                if (requestFilterModel.OnlyProject.HasValue && requestFilterModel.OnlyProject == true)
                {
                    var lstProjectId = requestFilterModel.ProjectIds.Split(",");
                    dapperExecution.SqlBuilder.Where("(t1.ProjectId IN @projectIds)",
                        new { projectIds = lstProjectId });
                }
                else
                {
                    var lstProjectId = requestFilterModel.ProjectIds.Split(",");
                    dapperExecution.SqlBuilder.Where("(t1.ProjectId IN @projectIds Or t1.ProjectId IS NULL)",
                        new { projectIds = lstProjectId });
                }

            }


            if (requestFilterModel.Any("targetId"))
            {
                dapperExecution.SqlBuilder.Where("t1.TargetId = @targetId",
                    new { targetId = requestFilterModel.Get("targetId") });
            }
            if (requestFilterModel.Any("targetCode"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<string>(
                    "vt.`TargetCode`",
                    requestFilterModel.GetProperty("targetCode"));
                needToJoinDebtHistory = true;
            }

            if (requestFilterModel.Any("targetFullName"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<string>(
                    "vt.`TargetFullName`",
                    requestFilterModel.GetProperty("targetFullName"));
                needToJoinDebtHistory = true;
            }

            if (requestFilterModel.Any("serviceId"))
            {
                dapperExecution.SqlBuilder.Where("rvd.ServiceId = @serviceId",
                    new { serviceId = requestFilterModel.Get("serviceId") });
            }

            if (requestFilterModel.Any("servicePackage"))
            {
                dapperExecution.SqlBuilder.Where("rvd.ServicePackageId = @servicePackageId",
                    new { servicePackageId = requestFilterModel.Get("servicePackage") });
            }
            if (!string.IsNullOrEmpty(requestFilterModel.ServiceIds))
            {
                var serviceIds = requestFilterModel.ServiceIds.Split(",");
                dapperExecution.SqlBuilder.Where("(rvd.ServiceId IN @serviceIds)", new { serviceIds = serviceIds })
                    ;
            }

            if (requestFilterModel.Any("startBillingDate"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "startBillingDate");
                dapperExecution.SqlBuilder.AppendPredicate<string>("rvd.`StartBillingDate`", propertyFilter);
            }

            if (requestFilterModel.Any("endBillingDate"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "endBillingDate");
                dapperExecution.SqlBuilder.AppendPredicate<string>("rvd.`EndBillingDate`", propertyFilter);
            }

            if (requestFilterModel.Any("cashierUserName"))
            {
                var cashierSearchingKeyword = requestFilterModel.Get<string>("cashierUserName");
                dapperExecution.SqlBuilder.Where("dh.CashierUserName LIKE @cashierKeywords", new { cashierKeywords = $"%{cashierSearchingKeyword}%" });
                needToJoinDebtHistory = true;
            }

            //if (requestFilterModel.PropertyFilterModels.Any(p => p.Field.Contains("IsEnterprise")))
            //{
            //    var propertyFilter = requestFilterModel.PropertyFilterModels
            //        .First(p => p.Field.Contains("IsEnterprise"));
            //    dapperExecution.SqlBuilder.AppendPredicate<string>("t1.`IsEnterprise`", propertyFilter);
            //}
            if (!string.IsNullOrWhiteSpace(requestFilterModel.CashierUserId))
            {
                dapperExecution.SqlBuilder.Where("dh.CashierUserId = @cashierUserId", new { cashierUserId = requestFilterModel.CashierUserId });
                needToJoinDebtHistory = true;
            }

            if (requestFilterModel.OutContractId.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.OutContractId = @outContractId", new { outContractId = requestFilterModel.OutContractId.Value });
            }
            if (!string.IsNullOrEmpty(requestFilterModel.OutContractIds))
            {
                dapperExecution.SqlBuilder.Where("t1.OutContractId IN @outContractIds", new { outContractIds = requestFilterModel.OutContractIds.Split(",") });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.StatusIds))
            {
                var lstStatusId = requestFilterModel.StatusIds.Split(",");
                dapperExecution.SqlBuilder.Where("t1.StatusId IN @statusIds",
                    new { statusIds = lstStatusId });
            }

            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.ContractCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.VoucherCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("vt.TargetFullName LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("vt.TargetPhone LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("vt.TargetAddress LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" });
            }

            if (requestFilterModel.StartingDate.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.CreatedDate >= @fromDate", new { fromDate = requestFilterModel.StartingDate.Value });
            }

            if (requestFilterModel.EndingDate.HasValue)
            {
                var toD = requestFilterModel.EndingDate.Value;
                dapperExecution.SqlBuilder.Where("t1.CreatedDate < @toDate", new { toDate = toD.AddDays(1) });
            }

            if (requestFilterModel.Any("badDebt"))
            {
                dapperExecution.SqlBuilder.Where("t1.StatusId = @statusId",
                    new
                    {
                        statusId = ReceiptVoucherStatus.BadDebt.Id
                    });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.CashierUserId))
            {
                dapperExecution.SqlBuilder.Where("EXISTS (SELECT 1 FROM ReceiptVoucherDebtHistories WHERE ReceiptVoucherId = t1.Id AND CashierUserId = @cashierUserId LIMIT 1)",
                    new { cashierUserId = requestFilterModel.CashierUserId });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.UserId))
            {
                dapperExecution.SqlBuilder.Where("(t1.CreatedUserId = @userId OR EXISTS (SELECT 1 FROM ReceiptVoucherDebtHistories WHERE ReceiptVoucherId = t1.Id AND CashierUserId = @userId LIMIT 1))",
                    new { userId = requestFilterModel.UserId });
            }
            #endregion

            dapperExecution.AddTemplate(sqlTemplate);
            dapperExecution.AddCountingTemplate(sqlCoutingTemplate);

            if (needToJoinDetail)
            {
                dapperExecution.SqlBuilder.InsightJoin("ReceiptVoucherDetails rvd ON t1.Id = rvd.ReceiptVoucherId");
            }

            if (needToJoinDebtHistory)
            {
                dapperExecution.SqlBuilder.InsightLeftJoin("ReceiptVoucherDebtHistories dh ON dh.ReceiptVoucherId = t1.Id");
            }

            if (needToJoinVoucherTarget)
            {
                dapperExecution.SqlBuilder.InsightJoin("VoucherTargets AS vt ON vt.Id = t1.TargetId");
            }

            if (!string.IsNullOrWhiteSpace(requestFilterModel?.OrderBy))
            {
                var orderByPrefix = this.IsBelongToPrimaryTable(requestFilterModel.OrderBy) ? "t1." : string.Empty;

                if (string.IsNullOrEmpty(requestFilterModel.Dir) || requestFilterModel.Dir.Equals("ASC", StringComparison.OrdinalIgnoreCase))
                {
                    dapperExecution.SqlBuilder.InsightOrderBy($"{orderByPrefix}`{requestFilterModel.OrderBy.ToUpperFirstLetter()}`");
                }
                else
                {
                    dapperExecution.SqlBuilder.InsightOrderDescBy($"{orderByPrefix}`{requestFilterModel.OrderBy.ToUpperFirstLetter()}`");
                }
            }

            var queryResult = dapperExecution
                .ExecutePaginateQuery<ReceiptVoucherGridDTO, PaymentMethod, ReceiptVoucherDetailGridDTO>(
                    (receiptVoucher, paymentMethod, receiptVoucherDetail) =>
                    {
                        if (!cached.TryGetValue(receiptVoucher.Id, out var voucherEntry))
                        {
                            voucherEntry = receiptVoucher;
                            voucherEntry.Payment = paymentMethod;
                            voucherEntry.StatusName = ReceiptVoucherStatus.List()
                                .Find(e => e.Id == voucherEntry.StatusId).ToString();
                            if (string.IsNullOrEmpty(voucherEntry.CashierUserName)
                                || !voucherEntry.CashierUserName.Contains(receiptVoucher.CashierUserName))
                            {
                                voucherEntry.CashierUserName +=
                                    $"{(string.IsNullOrWhiteSpace(voucherEntry.CashierUserName) ? string.Empty : ", ")}" +
                                    $"{receiptVoucher.CashierUserName}";
                            }

                            if (requestFilterModel.IsOutOfDate.HasValue)
                            {
                                if (requestFilterModel.IsOutOfDate.Value)
                                {
                                    voucherEntry.StatusNameApp = $"Quá hạn {voucherEntry.NumberDaysOverdue} ngày";
                                }
                                else
                                {
                                    voucherEntry.StatusNameApp = "Chưa thanh toán";
                                }
                            }

                            cached.Add(voucherEntry.Id, voucherEntry);
                        }

                        if (receiptVoucherDetail != null
                            && voucherEntry.ReceiptLines.All(s => s.ServiceId != receiptVoucherDetail.ServiceId))
                        {
                            voucherEntry.ReceiptLines.Add(receiptVoucherDetail);
                        }

                        return voucherEntry;
                    }, "Form,Id");

            return queryResult;
        }

        public IEnumerable<ConcludeFinancialReport> FinancialReport(ReceiptVoucherFilterModel requestFilterModel)
        {
            var allowDayNumber = _configuration.GetValue<int>("AllowIssuedDateNumber");
            requestFilterModel.NoPaging().ClearOrder();
            var dapperExecution = BuildByTemplateWithoutSelect<ConcludeFinancialReport, ReceiptVoucherSqlBuilder>(requestFilterModel);

            dapperExecution.SqlBuilder.Select("SUM(t1.GrandTotal) AS RevenueTotal");
            dapperExecution.SqlBuilder.Select("SUM(t1.PaidTotal) AS PaidTotal");
            dapperExecution.SqlBuilder.Select("SUM(t1.RemainingTotal) AS DebtTotal");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode");

            dapperExecution.SqlBuilder.InnerJoin("VoucherTargets vt ON vt.Id = t1.TargetId");
            dapperExecution.SqlBuilder.Where($"t1.StatusId != {ReceiptVoucherStatus.Canceled.Id}");  // không lấy phiếu đã hủy
            dapperExecution.SqlBuilder.GroupBy("t1.CurrencyUnitId");
            //dapperExecution.SqlBuilder.LeftJoin("ReceiptVoucherDebtHistories dh ON dh.ReceiptVoucherId = t1.Id");
            //dapperExecution.SqlBuilder.Where("dh.`Status` > 0 ");

            if (requestFilterModel.Any("badDebt"))
            {
                dapperExecution.SqlBuilder.Where("t1.StatusId = @statusId",
                    new { statusId = ReceiptVoucherStatus.BadDebt.Id });
            }

            dapperExecution.SqlBuilder.WhereValidVoucher(this.UserIdentity);

            if (string.Equals(requestFilterModel.Get<string>("managementMode"), ACCOUNTANT_CONFIRMATION, StringComparison.OrdinalIgnoreCase))
            {
                dapperExecution.SqlBuilder.Where("t1.StatusId = @collectingDebtStatus", new { collectingDebtStatus = ReceiptVoucherStatus.CollectOnBehalf.Id });
                dapperExecution.SqlBuilder.Where("EXISTS ("
                    + " SELECT dh.Id FROM ReceiptVoucherDebtHistories dh"
                    + " WHERE (t1.Id = dh.ReceiptVoucherId OR t1.Id = dh.SubstituteVoucherId)"
                    + " AND dh.Status <> @accountedStatus"
                    + " AND dh.CashingPaidTotal + dh.TransferringPaidTotal > 0"
                    + ")",
                    new
                    {
                        accountedStatus = (int)PaymentStatus.Accounted
                    }
                );
            }

            if (requestFilterModel.Any("serviceId") ||
                requestFilterModel.Any("servicePackages") ||
                requestFilterModel.Any("startBillingDate") ||
                requestFilterModel.Any("endBillingDate"))
            {
                dapperExecution.SqlBuilder.InnerJoin("ReceiptVoucherDetails rvd ON rvd.ReceiptVoucherId = t1.Id");
            }

            if (!string.IsNullOrEmpty(requestFilterModel.ProjectIds))
            {
                var lstProjectId = requestFilterModel.ProjectIds.Split(",");
                dapperExecution.SqlBuilder.Where("(t1.ProjectId IN @projectIds Or t1.ProjectId IS NULL)",
                    new { projectIds = lstProjectId });
            }

            if (requestFilterModel.Any("serviceId"))
            {
                dapperExecution.SqlBuilder.Where("rvd.ServiceId = @serviceId",
                    new { serviceId = requestFilterModel.Get("serviceId") });
            }

            if (requestFilterModel.Any("targetId"))
            {
                dapperExecution.SqlBuilder.Where("t1.TargetId = @targetId",
                    new { targetId = requestFilterModel.Get("targetId") });
            }

            if (requestFilterModel.PropertyFilterModels
                .Any(p => p.Field.ToLower() == "targetfullname"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field.ToLower() == "targetfullname");
                dapperExecution.SqlBuilder.AppendPredicate<string>("vt.`TargetFullName`", propertyFilter);
            }
            if (requestFilterModel.Any("targetCode"))
            {
                dapperExecution.SqlBuilder.AppendPredicate<string>(
                    "vt.`TargetCode`",
                    requestFilterModel.GetProperty("targetCode"));
            }

            if (requestFilterModel.Any("servicePackages"))
            {
                dapperExecution.SqlBuilder.Where("rvd.ServicePackageId = @servicePackageId",
                    new { servicePackageId = requestFilterModel.Get("servicePackages") });
            }

            if (requestFilterModel.Any("startBillingDate"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "startBillingDate");
                dapperExecution.SqlBuilder.AppendPredicate<string>("rvd.`StartBillingDate`", propertyFilter);
            }

            if (requestFilterModel.Any("endBillingDate"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "endBillingDate");
                dapperExecution.SqlBuilder.AppendPredicate<string>("rvd.`EndBillingDate`", propertyFilter);
            }

            if (requestFilterModel.Any("cashierUserId"))
            {
                var cashierUserId = requestFilterModel.Get<string>("cashierUserId");
                dapperExecution.SqlBuilder.Where("dh.CashierUserId = @cashierUserId", new { cashierUserId });
            }

            if (requestFilterModel.OutContractId.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.OutContractId = @outContractId", new { outContractId = requestFilterModel.OutContractId.Value });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.StatusIds))
            {
                var lstStatusId = requestFilterModel.StatusIds.Split(",");
                dapperExecution.SqlBuilder.Where("t1.StatusId IN @statusIds",
                    new { statusIds = lstStatusId });
            }

            if (requestFilterModel.IsOutOfDate.HasValue)
            {
                var iDate = DateTime.Now.AddDays(-allowDayNumber);
                if (requestFilterModel.IsOutOfDate.Value)
                {
                    dapperExecution.SqlBuilder.Where("t1.IssuedDate < @issuedDate", new { issuedDate = iDate });
                }
                else
                {
                    dapperExecution.SqlBuilder.Where("t1.IssuedDate >= @issuedDate", new { issuedDate = iDate });
                }

            }

            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.ContractCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("vt.TargetFullName LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("vt.TargetPhone LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("vt.TargetAddress LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" });
            }

            if (requestFilterModel.StartingDate.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.CreatedDate >= @fromDate", new { fromDate = requestFilterModel.StartingDate.Value });
            }

            if (requestFilterModel.EndingDate.HasValue)
            {
                var toD = requestFilterModel.EndingDate.Value;
                dapperExecution.SqlBuilder.Where("t1.CreatedDate < @toDate", new { toDate = toD.AddDays(1) });
            }

            if (requestFilterModel.Any("badDebt"))
            {
                dapperExecution.SqlBuilder.Where("t1.StatusId = @statusId",
                    new { statusId = ReceiptVoucherStatus.BadDebt.Id });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.CashierUserId))
            {

                dapperExecution.SqlBuilder.Where("EXISTS (SELECT 1 FROM ReceiptVoucherDebtHistories WHERE ReceiptVoucherId = t1.Id AND CashierUserId = @cashierUserId LIMIT 1)", new { cashierUserId = requestFilterModel.CashierUserId });
            }

            if (!string.IsNullOrEmpty(requestFilterModel.UserId))
            {

                dapperExecution.SqlBuilder.Where("(t1.CreatedUserId = @userId OR EXISTS (SELECT 1 FROM ReceiptVoucherDebtHistories WHERE ReceiptVoucherId = t1.Id AND CashierUserId = @userId LIMIT 1))", new { userId = requestFilterModel.UserId });
            }

            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<ReceiptVoucherDTO> GetListByTargetId(int targetId, int clearingId)
        {
            return GetListByTargetId(targetId, clearingId);
        }

        public IEnumerable<ReceiptVoucherDTO> GetListByTargetUid(string targetId, int clearingId)
        {
            return GetListByTargetId(targetId, clearingId);
        }

        private IEnumerable<ReceiptVoucherDTO> GetListByTargetId(object targetId, int clearingId)
        {
            var dapperExecution = BuildByTemplate<ReceiptVoucherDTO, ReceiptVoucherSqlBuilder>();

            dapperExecution.SqlBuilder.SelectCashier("dh");
            if (clearingId > 0)
            {
                dapperExecution.SqlBuilder.Where("t1.ClearingId = @clearingId", new { clearingId });
            }
            else
            {
                dapperExecution.SqlBuilder.Where("t1.GrandTotal > 0 AND t1.StatusId = @statusId", new { statusId = ReceiptVoucherStatus.Pending.Id });
            }

            dapperExecution.SqlBuilder.JoinCurrentCashier();

            dapperExecution.SqlBuilder.WhereValidVoucher(this.UserIdentity);
            dapperExecution.SqlBuilder.Where("t1.TypeId <> @typeId", new { typeId = ReceiptVoucherType.Clearing.Id });

            if (targetId is int)
            {
                dapperExecution.SqlBuilder.Where("t1.TargetId = @targetId", new { targetId });
            }
            else if (targetId is string)
            {
                dapperExecution.SqlBuilder.Where("t1.TargetId = (SELECT Id From VoucherTargets WHERE IdentityGuid = @targetId LIMIT 1)", new { targetId });
            }

            dapperExecution.SqlBuilder.GroupBy("t1.Id");
            return dapperExecution.ExecuteQuery();
        }

        public ReceiptVoucherDTO PrintVoucherById(int id)
        {
            var cached = new Dictionary<int, ReceiptVoucherDTO>();
            var dapperExecution = BuildByTemplate<ReceiptVoucherDTO, ReceiptVoucherSqlBuilder>();

            dapperExecution.SqlBuilder.SelectCashier("dh");

            // Select the receipt voucher detail
            dapperExecution.SqlBuilder.SelectVoucherDetail("t2");

            //VoucherTarget
            dapperExecution.SqlBuilder.SelectVoucherTarget("t3");

            //Taxes
            dapperExecution.SqlBuilder.Select("t4.Id");
            dapperExecution.SqlBuilder.Select("t4.TaxName");
            dapperExecution.SqlBuilder.Select("t4.TaxCode");
            dapperExecution.SqlBuilder.Select("t4.TaxValue");

            dapperExecution.SqlBuilder.InnerJoin(
                "VoucherTargets t3 ON t1.TargetId = t3.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "ReceiptVoucherDetails t2 ON t1.Id = t2.ReceiptVoucherId");

            dapperExecution.SqlBuilder.LeftJoin(
                "ReceiptVoucherLineTaxes t4 ON t4.VoucherLineId = t2.Id");

            dapperExecution.SqlBuilder.JoinCurrentCashier();

            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            var result = WithConnection(conn => conn.Query(dapperExecution.ExecutionTemplate.RawSql,
                 new[]
                 {
                    typeof(ReceiptVoucherDTO),
                    typeof(ReceiptVoucherDetailDTO),
                    typeof(VoucherTargetDTO),
                    typeof(ReceiptVoucherLineTaxDTO)
                 }, results =>
                 {
                     var voucherEntry = results[0] as ReceiptVoucherDTO;
                     if (voucherEntry == null) return null;

                     if (!cached.TryGetValue(voucherEntry.Id, out var result))
                     {
                         result = voucherEntry;
                         cached.Add(voucherEntry.Id, voucherEntry);
                     }

                     if (results[1] is ReceiptVoucherDetailDTO receiptVoucherDetail)
                     {
                         if (results[3] is ReceiptVoucherLineTaxDTO vchrDetailTax)
                         {
                             if (vchrDetailTax != null
                                && receiptVoucherDetail.ReceiptVoucherLineTaxes.All(s => s.Id != vchrDetailTax.Id))
                             {
                                 receiptVoucherDetail.ReceiptVoucherLineTaxes.Add(vchrDetailTax);
                             }
                         }

                         if (receiptVoucherDetail != null && result.ReceiptLines.All(s => s.ServiceId != receiptVoucherDetail.ServiceId))
                         {
                             result.ReceiptLines.Add(receiptVoucherDetail);
                         }
                         else if (receiptVoucherDetail != null)
                         {
                             var receiptLine = result.ReceiptLines.FirstOrDefault(e => e.ServiceId == receiptVoucherDetail.ServiceId && e.Id != receiptVoucherDetail.Id);
                             if (receiptLine != null)
                             {
                                 receiptLine.GrandTotal += receiptVoucherDetail.GrandTotal;
                                 receiptLine.ReductionFee += receiptVoucherDetail.ReductionFee;
                             }
                         }
                     }

                     result.Target = results[2] as VoucherTargetDTO;
                     return result;

                 }, dapperExecution.ExecutionTemplate.Parameters,
                 null,
                 true,
                 "Id,Id,Id"))
                .FirstOrDefault();

            var taxPercentValue = 0f;
            foreach (var tax in result.ReceiptLines.SelectMany(d => d.ReceiptVoucherLineTaxes))
            {
                taxPercentValue += tax.TaxValue;
            }

            result.ReceiptLines
                .ForEach(e => e.GrandTotal += (e.GrandTotal * (decimal)(taxPercentValue / 100)));

            return result;
        }

        public IEnumerable<CollectedVouchersDTO> GetCollectedAndUnCollectedVoucherByMonth(CollectedVoucherFilter voucherFilter)
        {
            var p = new DynamicParameters();
            p.Add("cashierUserId", voucherFilter.CashierUserId);
            p.Add("month", voucherFilter.Month);
            p.Add("year", voucherFilter.Year);
            return WithConnection(conn =>
               conn.Query<CollectedVouchersDTO>("sp_GetCollectedAndUnCollectedVoucherByMonth",
               p,
               commandType: CommandType.StoredProcedure));
        }

        public int GetLastVoucherId()
        {
            var result = WithConnection(conn =>
                conn.QueryFirstOrDefault<int?>("SELECT Id FROM ReceiptVouchers ORDER BY Id DESC LIMIT 1"));
            return result ?? default;
        }

        public HashSet<string> GetVoucherCodeFromStartingId(int startingId)
        {
            return WithConnection(conn =>
                conn.
                    Query<string>(
                        "SELECT VoucherCode FROM ReceiptVouchers WHERE IsDeleted = FALSE AND Id > @startingId",
                        new { startingId }
                    )
                    .ToHashSet());
        }

        /// <summary>
        /// Lấy tổng tiền phân chia của hợp đồng đầu vào theo từng hợp đồng đầu ra
        /// Số tiền dựa trên tiền đã thu trên các phiếu thu ở trạng thái đã hoàn thành
        /// </summary>
        /// <param name="outContractIds"> List hợp đồng đầu ra</param>
        /// <param name="paymentTerm">Kỳ hạn thanh toán</param>
        /// <param name="currencyUnitId">Đơn vị tiền tệ</param>
        /// <returns></returns>
        public IEnumerable<ReceiptVoucherSharingRevenueDTO> GetTotalSharingRevenusByReceiptVoucher(int inContractId, string startBillingDate, string endBillingDate,
                int currencyUnitId, int paymentVoucherId = 0, int skip = 0)
        {

            var dapperExecution = BuildByTemplate<ReceiptVoucherSharingRevenueDTO>();
            var p = new DynamicParameters();
            p.Add("skip", skip);
            p.Add("inContractId", inContractId);
            p.Add("startDate", startBillingDate, DbType.Date);
            p.Add("endDate", endBillingDate, DbType.Date);
            p.Add("total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var masters = WithConnection(
                (conn) =>
                    conn.Query<ReceiptVoucherSharingRevenueDTO>("sp_GetTotalSharingRevenusByReceiptVoucher",
                    p,
                    commandType: CommandType.StoredProcedure)
            );
            var lstReceiptVouchers = new List<ReceiptVoucherSharingRevenue>();
            var receiptVoucher = new ReceiptVoucherSharingRevenue();
            foreach (var item in masters)
            {
                receiptVoucher = _mapper.Map<ReceiptVoucherSharingRevenue>(item);
                var service = _mapper.Map<ServiceSharingRevenue>(item);
                receiptVoucher.ServiceSharingRevenue.Add(service);
                lstReceiptVouchers.Add(receiptVoucher);
            }

            return masters;
        }

        public IEnumerable<ReceiptVoucherDTO> GetTotalSharingRevenusByReceiptVoucher2(string[] outContractIds, string startBillingDate, string endBillingDate, int currencyUnitId, int paymentVoucherId = 0)
        {

            var dapperExecution = BuildByTemplate<ReceiptVoucherDTO, ReceiptVoucherSqlBuilder>();
            if (paymentVoucherId > 0)
            {
                dapperExecution.SqlBuilder.SelectVoucherDetail("t2");
                dapperExecution.SqlBuilder.InnerJoin("ReceiptVoucherDetails as t2 ON t2.ReceiptVoucherId = t1.Id");
                dapperExecution.SqlBuilder.RightJoin("ReceiptVoucherInPaymentVoucher as t3 ON t3.ReceiptVoucherId = t1.Id");
                dapperExecution.SqlBuilder.Where("t3.PaymentVoucherId = @paymentVoucherId", new { paymentVoucherId });
            }
            else
            {
                string sIds = string.Join(",", outContractIds);
                string sqlWhere = "t1.OutContractId IN (" + sIds + ")";
                dapperExecution.SqlBuilder.SelectVoucherDetail("t2");
                // dapperExecution.SqlBuilder.WhereValidVoucher(this.UserIdentity);

                dapperExecution.SqlBuilder.InnerJoin("ReceiptVoucherDetails as t2 ON t2.ReceiptVoucherId = t1.Id");
                dapperExecution.SqlBuilder.Where("t1.StatusId IN (2,3,4,6)");
                dapperExecution.SqlBuilder.Where("t1.CurrencyUnitId = @currencyUnitId", new { currencyUnitId });
                dapperExecution.SqlBuilder.Where(sqlWhere);
                dapperExecution.SqlBuilder.Where("DATE(t1.IssuedDate) BETWEEN @startBillingDate AND @endBillingDate", new { startBillingDate = startBillingDate, endBillingDate = endBillingDate });
            }

            var cached = new Dictionary<int, ReceiptVoucherDTO>();
            WithConnection(conn => conn.Query(dapperExecution.ExecutionTemplate.RawSql,
              new[]
              {
                    typeof(ReceiptVoucherDTO),
                    typeof(ReceiptVoucherDetailDTO)
              }, results =>
              {
                  var voucherEntry = results[0] as ReceiptVoucherDTO;
                  if (voucherEntry == null) return null;

                  if (!cached.TryGetValue(voucherEntry.Id, out var result))
                  {
                      result = voucherEntry;
                      cached.Add(voucherEntry.Id, voucherEntry);
                  }

                  if (results[1] is ReceiptVoucherDetailDTO receiptVoucherDetail)
                  {
                      if (result.ReceiptLines.Any(r => r.Id == receiptVoucherDetail.Id))
                      {
                          receiptVoucherDetail = result.ReceiptLines.First(r => r.Id == receiptVoucherDetail.Id);
                      }
                      else
                      {
                          result.ReceiptLines.Add(receiptVoucherDetail);
                      }
                  }

                  return result;
              }, dapperExecution.ExecutionTemplate.Parameters,
              null,
              true,
              "Id"));
            return cached.Values;
        }

        public PagedList<ReportTotalRevenueRaw> GetListTotalRevenue(ReportTotalRevenueFilter filterModel)
        {
            var result = new List<ReportTotalRevenueRaw>();
            var dapperExecution = BuildByTemplate<ReportTotalRevenue>();
            dapperExecution.SqlBuilder.Select("t1.ContractCode");
            dapperExecution.SqlBuilder.Select("vt.TargetFullName");
            dapperExecution.SqlBuilder.Select("t1.InvoiceCode");
            dapperExecution.SqlBuilder.Select("octsp.TimeLine_Effective");
            dapperExecution.SqlBuilder.Select("t1.GrandTotal");
            dapperExecution.SqlBuilder.InnerJoin("VoucherTargets vt ON vt.Id = t1.TargetId");
            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.OutContracts  oct ON oct.Id = t1.OutContractId");
            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.OutContractServicePackages octsp on octsp.OutContractId = t1.OutContractId");
            dapperExecution.SqlBuilder.Where("t1.IsDeleted = false");
            dapperExecution.SqlBuilder.OrderDescBy("t1.OutContractId");
            dapperExecution.SqlBuilder.OrderDescBy("t1.Id");

            if (filterModel.CustomerInfor != null && filterModel.CustomerInfor != "")
            {
                var targetId = filterModel.CustomerInfor;
                dapperExecution.SqlBuilder.Where("vt.IdentityGuid = @targetId", new { targetId });
            }
            if (filterModel.OutContractCode != null && filterModel.OutContractCode != "")
            {
                dapperExecution.SqlBuilder.Where("t1.ContractCode LIKE @outContractCode", new { outContractCode = $"%{filterModel.OutContractCode}%" });
            }
            if (filterModel.InvoiceCode != null && filterModel.InvoiceCode != "")
            {
                dapperExecution.SqlBuilder.Where("t1.InvoiceCode LIKE @invoiceCode", new { invoiceCode = $"%{filterModel.InvoiceCode}%" });
            }
            if (filterModel.TimeLine_Effective_StartDate != null)
            {
                var timeLine_Effective_StartDate = filterModel.TimeLine_Effective_StartDate;
                dapperExecution.SqlBuilder.Where("octsp.TimeLine_Effective >= @timeLine_Effective_StartDate", new { timeLine_Effective_StartDate });
            }
            if (filterModel.TimeLine_Effective_EndDate != null)
            {
                var timeLine_Effective_EndDate = filterModel.TimeLine_Effective_EndDate;
                dapperExecution.SqlBuilder.Where("octsp.TimeLine_Effective <= @timeLine_Effective_EndDate", new { timeLine_Effective_EndDate });
            }

            var lstTotalRevenue = dapperExecution.ExecuteQuery();

            if (lstTotalRevenue.Count() > 0)
            {
                var revenuesGroupByContractCode = lstTotalRevenue.GroupBy(x => x.ContractCode);
                foreach (var item in revenuesGroupByContractCode)
                {
                    var report = new ReportTotalRevenueRaw();
                    foreach (var r in item)
                    {
                        report.ContractCode = item.Key;
                        if (r.TargetFullName != null && r.TargetFullName != "")
                        {
                            report.ListTargetFullName.Add(r.TargetFullName);
                        }
                        else
                        {
                            report.ListTargetFullName.Add("");
                        }

                        if (r.InvoiceCode != null && r.InvoiceCode != "")
                        {
                            report.ListInVoiceCode.Add(r.InvoiceCode);
                        }
                        else
                        {
                            report.ListTargetFullName.Add("");
                        }

                        if (r.TimeLine_Effective != null)
                        {
                            report.TimeLine_Effectives_OutContract.Add(r.TimeLine_Effective);
                        }

                        report.GrandTotal += r.GrandTotal;
                    }

                    result.Add(report);
                }
            }

            var y = result.Sum(x => x.GrandTotal);
            result.Select(x => x.Total == y);

            var page = new PagedList<ReportTotalRevenueRaw>(filterModel.Skip, filterModel.Take, result.Count());
            page.Subset = result.Skip(filterModel.Skip).Take(filterModel.Take).ToList();
            return page;
        }

        public IEnumerable<ReceiptVoucherDTO> GetOverdueVouchers()
        {
            var dapperExecution = BuildByTemplate<ReceiptVoucherDTO, ReceiptVoucherSqlBuilder>();
            dapperExecution.SqlBuilder.Where($"t1.StatusId = {ReceiptVoucherStatus.Overdue.Id}");

            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<ReceiptVoucherDTO> GetDueVouchers()
        {
            var dapperExecution = BuildByTemplate<ReceiptVoucherDTO, ReceiptVoucherSqlBuilder>();
            dapperExecution.SqlBuilder.Where("t1.StatusId IN (@validStatuses)",
                new { validStatuses = ReceiptVoucherStatus.UnpaidStatuses().Where(s => s != ReceiptVoucherStatus.Overdue.Id) });
            dapperExecution.SqlBuilder.Where("t1.NumberBillingLimitDays > DATEDIFF(CURDATE(), t1.IssuedDate)");

            return dapperExecution.ExecuteQuery();
        }

        public async Task<IEnumerable<ReceiptVoucherPrintModel>> GetForPrinting(string voucherIds)
        {
            var cached = new Dictionary<int, ReceiptVoucherPrintModel>();
            var getVouchersQuery = WithConnectionAsync(async conn =>
                await conn.QueryAsync("GetReceiptVoucherPrintData",
                new[]
                {
                    typeof(ReceiptVoucherPrintModel),
                    typeof(PaymentMethod),
                    typeof(ReceiptVoucherLinePrintModel),
                    typeof(VoucherTargetDTO)
                }, results =>
                {
                    var voucherEntry = results[0] as ReceiptVoucherPrintModel;
                    if (voucherEntry == null) return null;

                    if (!cached.TryGetValue(voucherEntry.Id, out var result))
                    {
                        result = voucherEntry;
                        cached.Add(voucherEntry.Id, voucherEntry);
                    }

                    result.Payment = results[1] as PaymentMethod;

                    if (results[2] is ReceiptVoucherLinePrintModel receiptVoucherDetail)
                    {
                        if (result.ReceiptLines.Any(r => r.Id == receiptVoucherDetail.Id))
                        {
                            receiptVoucherDetail = result.ReceiptLines.First(r => r.Id == receiptVoucherDetail.Id);
                        }
                        else
                        {
                            result.ReceiptLines.Add(receiptVoucherDetail);
                        }
                    }

                    result.Target = results[3] as VoucherTargetDTO;
                    return result;
                },
                new
                {
                    voucherIds
                },
                splitOn: "Form,Id,Id",
                commandType: CommandType.StoredProcedure));

            var getOpeningDebtsQuery = WithConnectionAsync(async conn =>
                await conn.QueryAsync<OpeningDebtByServiceModel>("GetOpeningDebts",
                new
                {
                    voucherIds
                },
                commandType: CommandType.StoredProcedure
                ));

            await Task.WhenAll(getVouchersQuery, getOpeningDebtsQuery);

            var vouchers = getVouchersQuery.Result?.Distinct();
            var openingDebts = getOpeningDebtsQuery.Result?.Distinct();

            /// Nhóm các kênh thanh toán của phiếu thu lại theo dịch vụ
            /// Đồng thời cộng công nợ đầu kỳ
            if (vouchers != null && vouchers.Any())
            {
                foreach (var voucher in vouchers)
                {
                    voucher.DebtsByService = voucher.ReceiptLines
                        .GroupBy(r => r.ServiceId)
                        .Select(g =>
                        {
                            var minStartBillingDate = g.Min(c => c.StartBillingDate);
                            var maxEndBillingDate = g.Max(c => c.EndBillingDate);

                            var result = new DebtPrintModel()
                            {
                                ServiceId = g.Key,
                                ServiceName = g.FirstOrDefault()?.ServiceName,
                                CurrencyUnitCode = voucher.CurrencyUnitCode,
                                CurrencyUnitId = voucher.CurrencyUnitId,
                                GrandTotalBeforeTax = g.Sum(c => c.GrandTotalBeforeTax),
                                TaxAmount = g.Sum(c => c.TaxAmount),
                                GrandTotal = g.Sum(c => c.GrandTotalBeforeTax + c.TaxAmount),
                                OpeningDebtTotal = openingDebts?
                                .Where(c => c.ServiceId == g.Key)
                                .Sum(c => c.DebtAmount) ?? 0,
                                ReductionFee = g.Sum(c => c.ReductionFee)
                            };

                            result.Content = $"Cước thuê kênh {g.FirstOrDefault()?.ServiceName} " +
                            $"từ {minStartBillingDate:dd/MM/yyyy} đến {maxEndBillingDate:dd/MM/yyyy}";

                            return result;
                        })
                        .ToList();
                    voucher.GrandTotalIncludeDebt =
                        voucher.GrandTotal + (openingDebts?.Sum(c => c.DebtAmount) ?? 0);
                    voucher.IncurredDebtLabel = "Phát sinh";
                    var minStartBillingDate = voucher.ReceiptLines.Min(c => c.StartBillingDate);
                    var maxEndBillingDate = voucher.ReceiptLines.Max(c => c.EndBillingDate);
                    if (minStartBillingDate != null && maxEndBillingDate != null)
                    {
                        if (
                            minStartBillingDate.Value.Month == maxEndBillingDate.Value.Month &&
                            minStartBillingDate.Value.Year == maxEndBillingDate.Value.Year)
                        {
                            voucher.IncurredDebtLabel = $"Phát sinh tháng " +
                                $"{minStartBillingDate.Value.Month:D2}/{minStartBillingDate.Value.Year}";
                        }

                        if (
                            minStartBillingDate.Value.Month + 1 == maxEndBillingDate.Value.Month &&
                            minStartBillingDate.Value.Year == maxEndBillingDate.Value.Year &&
                            minStartBillingDate.Value.Day <= 7 && maxEndBillingDate.Value.Day <= 7)
                        {
                            voucher.IncurredDebtLabel = $"Phát sinh tháng " +
                                $"{minStartBillingDate.Value.Month:D2}/{minStartBillingDate.Value.Year}";
                        }

                        if (
                            minStartBillingDate.Value.Month + 1 == maxEndBillingDate.Value.Month &&
                            minStartBillingDate.Value.Year == maxEndBillingDate.Value.Year &&
                            minStartBillingDate.Value.Day > 7)
                        {
                            voucher.IncurredDebtLabel = $"Phát sinh tháng " +
                                $"{maxEndBillingDate.Value.Month:D2}/{maxEndBillingDate.Value.Year}";
                        }
                    }
                }
            }

            return vouchers;
        }

        public IEnumerable<int> GetReceiptVoucherIds(string outContractCode)
        {
            var dapperExecution = BuildByTemplate<int>();
            dapperExecution.SqlBuilder.Select("t1.Id");
            dapperExecution.SqlBuilder.Where("t1.ContractCode = @ContractCode", new { ContractCode = outContractCode });
            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<ExportInvoiceFileModel> GetInvoiceReportData(ExportInvoiceFilterModel filter)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<ExportInvoiceFileModel>(filter);

            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode");

            dapperExecution.SqlBuilder.Select("t5.GroupName as ServiceGroupName");
            dapperExecution.SqlBuilder.Select("t2.MarketAreaName");
            dapperExecution.SqlBuilder.Select("t2.TimeLine_PaymentPeriod as TimeLinePaymentPeriod");

            dapperExecution.SqlBuilder.Select("t1.PaidTotal * t1.ExchangeRate AS PaidTotal");
            dapperExecution.SqlBuilder.Select("t1.PaymentDate");
            dapperExecution.SqlBuilder.Select("0 as LuyKeThang");
            dapperExecution.SqlBuilder.Select("t1.VoucherCode");
            dapperExecution.SqlBuilder.Select("t1.InvoiceCode");
            dapperExecution.SqlBuilder.Select("t1.InvoiceDate");
            dapperExecution.SqlBuilder.Select("t1.InvoiceReceivedDate");
            dapperExecution.SqlBuilder.Select("t1.Content");
            dapperExecution.SqlBuilder.Select("t1.Description");

            dapperExecution.SqlBuilder.Select("us.CustomerCode as CustomerCode");
            dapperExecution.SqlBuilder.Select("us.FullName");
            dapperExecution.SqlBuilder.Select("vch.TargetCode AS CustomerCode");
            dapperExecution.SqlBuilder.Select("vch.TargetFullName AS FullName");

            dapperExecution.SqlBuilder.Select("vtp.`CategoryName` AS CategoryName");
            dapperExecution.SqlBuilder.Select("t6.Name AS StatusName");

            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.OutContracts AS t2 ON t1.OutContractId = t2.Id");
            dapperExecution.SqlBuilder.InnerJoin("VoucherTargets AS vch ON vch.Id = t1.TargetId");
            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_CRM.ApplicationUsers AS us ON us.IdentityGuid = vch.ApplicationUserIdentityGuid");
            dapperExecution.SqlBuilder.LeftJoin("VoucherTargetProperties  AS vtp ON vtp.TargetId = vch.Id");
            dapperExecution.SqlBuilder.InnerJoin("ReceiptVoucherDetails AS t3 ON t3.ReceiptVoucherId  = t1.Id");
            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.Services AS t4 ON t4.Id  = t3.ServiceId");
            dapperExecution.SqlBuilder.InnerJoin("ITC_FBM_Contracts.ServiceGroups AS t5 ON t5.Id  = t4.GroupId");
            dapperExecution.SqlBuilder.InnerJoin("ReceiptVoucherStatuses AS t6 ON t6.Id  = t1.StatusId");

            dapperExecution.SqlBuilder.Where("(@marketAreaId = 0 OR t1.MarketAreaId = @marketAreaId )", new { marketAreaId = filter.MarketAreaId });
            dapperExecution.SqlBuilder.Where("IFNULL(t1.InvoiceCode,'') <> ''");
            if (filter.TimelineSignedStart.HasValue && filter.TimelineSignedEnd.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) BETWEEN DATE(@startDate) AND DATE(@endDate)", new
                {
                    startDate = filter.TimelineSignedStart.Value.Date,
                    endDate = filter.TimelineSignedEnd.Value.Date
                });
            }
            else if (filter.TimelineSignedStart.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) >= @startDate", new
                {
                    startDate = filter.TimelineSignedStart.Value.Date
                });
            }
            else if (filter.TimelineSignedEnd.HasValue)
            {
                dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) <= @endDate", new
                {
                    endDate = filter.TimelineSignedEnd.Value.Date
                });
            }

            dapperExecution.SqlBuilder.Where("DATE(t2.TimeLine_Signed) BETWEEN DATE(@startDate) AND DATE(@endDate)", new
            {
                startDate = filter.TimelineSignedStart.Value.Date,
                endDate = filter.TimelineSignedEnd.Value.Date
            });
            if (filter.ServiceGroupId > 0)
            {
                dapperExecution.SqlBuilder.Where("t5.Id = @serviceGroupId", new { serviceGroupId = filter.ServiceGroupId });
            }
            if (filter.ServiceId > 0)
            {
                dapperExecution.SqlBuilder.Where("t4.ServiceId = @serviceId", new { serviceId = filter.ServiceId });
            }
            if (filter.ProjectId > 0)
            {
                dapperExecution.SqlBuilder.Where("t1.ProjectId = @projectId", new { projectId = filter.ProjectId });
            }
            if (filter.IsEnterprise < 2)
            {
                dapperExecution.SqlBuilder.Where("vch.IsEnterprise = @isEnterprise", new { isEnterprise = filter.IsEnterprise });
            }
            if (!string.IsNullOrEmpty(filter.ContractCode))
            {
                dapperExecution.SqlBuilder.Where("t1.ContractCode LIKE @contractCode ", new { contractCode = $"%{filter.ContractCode}%" });
            }
            if (!string.IsNullOrEmpty(filter.CustomerCode))
            {
                dapperExecution.SqlBuilder.Where("vch.TargetCode LIKE @customerCode ", new { customerCode = $"%{filter.CustomerCode}%" });
            }

            var res = dapperExecution.ExecutePaginateQuery();
            return res;

        }

        public HashSet<string> GetVoucherCodeFromIssuedDate(DateTime startDate)
        {
            return WithConnection(conn =>
                conn.
                    Query<string>(
                        "SELECT VoucherCode " +
                        "FROM ReceiptVouchers " +
                        "WHERE DATE(CreatedDate) >= DATE(@startDate) " +
                        "ORDER BY Id DESC",
                        new { startDate }
                    )
                    .ToHashSet());
        }

        public IPagedList<TotalRevenuePersonalDTO> GetTotalRevenueEnterpiseReport(PaymentVoucherReportFilter requestFilterModel)
        {
            var pr = new DynamicParameters();
            pr.Add("skip", requestFilterModel.Skip);
            pr.Add("take", requestFilterModel.Take);

            pr.Add("orderBy", "ORDER BY " + requestFilterModel.OrderBy.ToUpperFirstLetter() + " " + requestFilterModel.Dir);
            pr.Add("total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            pr.Add("marketAreaId", requestFilterModel.MarketAreaId);
            pr.Add("contractCode", requestFilterModel.ContractCode ?? "");
            pr.Add("customerCode", requestFilterModel.CustomerCode ?? "");
            pr.Add("startDate", requestFilterModel.TimelineSignedStart, DbType.Date);
            pr.Add("endDate", requestFilterModel.TimelineSignedEnd, DbType.Date);

            pr.Add("serviceId", requestFilterModel.ServiceId);
            pr.Add("projectId", requestFilterModel.ProjectId);
            pr.Add("agentId", requestFilterModel.AgentId ?? "");

            pr.Add("isEnterprise", requestFilterModel.IsEnterprise);
            pr.Add("status", requestFilterModel.Status);

            string sql = "Some stored";

            var subSetResult = WithConnection(conn =>
                    conn.Query<TotalRevenuePersonalDTO>(
                        sql,
                        pr,
                        commandType: CommandType.StoredProcedure
                        )
                    ).Distinct().ToList();

            var totalRecords = requestFilterModel.Paging ? pr.Get<int>("total") : subSetResult.Count();
            var result = new PagedList<TotalRevenuePersonalDTO>(requestFilterModel.Skip, requestFilterModel.Take, totalRecords)
            {
                Subset = subSetResult.ToList(),
                TotalItemCount = totalRecords
            };

            return result;
        }

        public IPagedList<FTTHProjectRevenueDTO> GetFTTHProjectRevenue(PaymentVoucherReportFilter requestFilterModel)
        {
            var pr = new DynamicParameters();
            pr.Add("skip", requestFilterModel.Skip);
            pr.Add("take", requestFilterModel.Take);

            pr.Add("orderBy", "ORDER BY " + requestFilterModel.OrderBy.ToUpperFirstLetter() + " " + requestFilterModel.Dir);
            pr.Add("total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            pr.Add("marketAreaId", requestFilterModel.MarketAreaId);
            pr.Add("contractCode", requestFilterModel.ContractCode ?? "");
            pr.Add("customerCode", requestFilterModel.CustomerCode ?? "");
            pr.Add("startDate", requestFilterModel.TimelineSignedStart, DbType.Date);
            pr.Add("endDate", requestFilterModel.TimelineSignedEnd, DbType.Date);

            pr.Add("serviceId", requestFilterModel.ServiceId);
            pr.Add("projectId", requestFilterModel.ProjectId);
            pr.Add("agentId", requestFilterModel.AgentId ?? "");

            pr.Add("isEnterprise", requestFilterModel.IsEnterprise);
            pr.Add("status", requestFilterModel.Status);

            string sql = "Some stored";

            var subSetResult = WithConnection(conn =>
                    conn.Query<FTTHProjectRevenueDTO>(
                        sql,
                        pr,
                        commandType: CommandType.StoredProcedure
                        )
                    ).Distinct().ToList();

            var totalRecords = requestFilterModel.Paging ? pr.Get<int>("total") : subSetResult.Count();
            var result = new PagedList<FTTHProjectRevenueDTO>(requestFilterModel.Skip, requestFilterModel.Take, totalRecords)
            {
                Subset = subSetResult.ToList(),
                TotalItemCount = totalRecords
            };

            return result;
        }
    }
}