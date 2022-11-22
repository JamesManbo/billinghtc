using Dapper;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using System.Collections.Generic;
using System.Linq;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.Models.PaymentVoucherModels;
using DebtManagement.Domain.Seed;
using GenericRepository.Extensions;
using DebtManagement.Domain.Models.ReportModels;
using GenericRepository.DapperSqlBuilder;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using System;

namespace DebtManagement.Infrastructure.Queries
{
    public class PaymentFilterModel : RequestFilterModel
    {
        public int? InContractId { get; set; }
    }
    public interface IPaymentVoucherQueries : IQueryRepository
    {
        int GetOrderNumberByNow();
        IEnumerable<ConcludeFinancialReport> FinancialReport(PaymentFilterModel requestFilterModel);
        IPagedList<PaymentVoucherGridDTO> GetList(PaymentFilterModel requestFilterModel);
        PaymentVoucherDTO Find(string id);
        PaymentVoucherDTO Find(int id);
        IEnumerable<PaymentVoucherDTO> GetListByTargetId(int targetId, int clearingId);
        IEnumerable<PaymentVoucherDTO> GetListByTargetUid(string targetId, int clearingId);
        IEnumerable<int> GetPaymentVoucherIds(string inContractCode);
    }

    public class PaymentVoucherSqlBuilder : SqlBuilder
    {
        public PaymentVoucherSqlBuilder SelectVoucherTarget(string alias)
        {
            Select($"{alias}.Id");
            Select($"{alias}.TargetFullName");
            Select($"{alias}.TargetCode");
            Select($"{alias}.TargetAddress");
            Select($"{alias}.TargetPhone");
            Select($"{alias}.TargetEmail");
            Select($"{alias}.TargetFax");
            Select($"{alias}.TargetIdNo");
            Select($"{alias}.TargetTaxIdNo");
            Select($"{alias}.IsEnterprise");
            Select($"{alias}.IsPayer");
            return this;
        }
        public PaymentVoucherSqlBuilder SelectVoucherDetail(string alias)
        {
            Select($"{alias}.Id");
            Select($"{alias}.PaymentVoucherId");
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
            Select($"{alias}.PackagePrice");
            Select($"{alias}.OtherFeeTotal");
            Select($"{alias}.ReductionFee");
            Select($"{alias}.GrandTotalBeforeTax");
            Select($"{alias}.GrandTotal");
            Select($"{alias}.IsFirstDetailOfService");
            Select($"{alias}.PaymentPeriod");
            Select($"{alias}.CId");
            Select($"{alias}.HasDistinguishBandwidth");
            Select($"{alias}.HasStartAndEndPoint");
            Select($"{alias}.DomesticBandwidth");
            Select($"{alias}.InternationalBandwidth");
            return this;
        }
    }

    public class PaymentVoucherQueries : QueryRepository<PaymentVoucher, int>, IPaymentVoucherQueries
    {
        public PaymentVoucherQueries(DebtDbContext context) : base(context)
        {
        }
        public int GetOrderNumberByNow()
        {
            return WithConnection(conn =>
                conn.QueryFirst<int>(
                    "SELECT (COUNT(1) + 1) FROM PaymentVouchers WHERE DATE(IssuedDate) = CURDATE()"));
        }

        public IEnumerable<ConcludeFinancialReport> FinancialReport(PaymentFilterModel requestFilterModel)
        {
            requestFilterModel.Paging = false;
            var dapperExecution = BuildByTemplateWithoutSelect<ConcludeFinancialReport>(requestFilterModel);

            dapperExecution.SqlBuilder.Select("SUM(t1.GrandTotal) AS RevenueTotal");
            dapperExecution.SqlBuilder.Select("SUM(t1.PaidTotal) AS PaidTotal");
            dapperExecution.SqlBuilder.Select("SUM(t1.RemainingTotal) AS DebtTotal");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitId");
            dapperExecution.SqlBuilder.Select("t1.CurrencyUnitCode");
            dapperExecution.SqlBuilder.GroupBy("t1.CurrencyUnitId");

            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<PaymentVoucherGridDTO> GetList(PaymentFilterModel requestFilterModel)
        {
            var cached = new Dictionary<string, PaymentVoucherGridDTO>();
            var dapperExecution = BuildByTemplate<PaymentVoucherGridDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("vt.`TargetFullName` AS `TargetFullName`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Form` AS `Form`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Method` AS `Method`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Address` AS `Address`");

            // Select the receipt voucher detail
            dapperExecution.SqlBuilder.Select("pvd.`Id`");
            dapperExecution.SqlBuilder.Select("pvd.`PaymentVoucherId`");
            dapperExecution.SqlBuilder.Select("pvd.`ServiceId`");
            dapperExecution.SqlBuilder.Select("pvd.`ServicePackageId`");
            dapperExecution.SqlBuilder.Select("pvd.`EndBillingDate`");
            dapperExecution.SqlBuilder.Select("pvd.`SubTotal`");
            dapperExecution.SqlBuilder.Select("pvd.`OtherFeeTotal`");
            dapperExecution.SqlBuilder.Select("pvd.`GrandTotal`");
            dapperExecution.SqlBuilder.Select("pvd.`ServiceName`");
            dapperExecution.SqlBuilder.Select("pvd.`ServicePackageName`");
            dapperExecution.SqlBuilder.Select("pvd.`StartBillingDate`");

            dapperExecution.SqlBuilder.LeftJoin(
               "PaymentVoucherDetails pvd ON t1.Id = pvd.PaymentVoucherId AND pvd.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin("VoucherTargets vt ON vt.Id = t1.TargetId");

            if (requestFilterModel.Any("statusName"))
            {
                dapperExecution.SqlBuilder.Where("t1.StatusId = @statusId",
                    new { statusId = requestFilterModel.Get("statusName") });
            }


            if (requestFilterModel.PropertyFilterModels
                .Any(p => p.Field.ToLower() == "targetfullname"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field.ToLower() == "targetfullname");
                dapperExecution.SqlBuilder.AppendPredicate<string>("vt.`TargetFullName`", propertyFilter);
            }

            if (requestFilterModel.Any("serviceId"))
            {
                dapperExecution.SqlBuilder.Where("pvd.ServiceId = @serviceId",
                    new { serviceId = requestFilterModel.Get("serviceId") });
            }

            if (requestFilterModel.PropertyFilterModels.Any(p => p.Field == "servicePackages"))
            {
                dapperExecution.SqlBuilder.Where("pvd.ServicePackageId = @servicePackageId",
                    new { servicePackageId = requestFilterModel.Get("servicePackages") });
            }

            if (requestFilterModel.InContractId.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.InContractId = @inContractId", new { inContractId = requestFilterModel.InContractId });
            }

            return dapperExecution
                .ExecutePaginateQuery<PaymentVoucherGridDTO, PaymentMethod, PaymentVoucherDetailGridDTO>(
                    (paymentVoucher, paymentMethod, paymentVoucherDetail) =>
                    {
                        if (!cached.TryGetValue(paymentVoucher.Id, out var voucherEntry))
                        {
                            voucherEntry = paymentVoucher;
                            voucherEntry.Payment = paymentMethod;

                            cached.Add(voucherEntry.Id, voucherEntry);
                        }

                        voucherEntry.StatusName = Enumeration.FromValue<PaymentVoucherStatus>(voucherEntry.StatusId)
                            .ToString();

                        if (paymentVoucherDetail != null
                        && voucherEntry.PaymentVoucherDetails.All(s => s.ServiceId != paymentVoucherDetail.ServiceId))
                        {
                            voucherEntry.PaymentVoucherDetails.Add(paymentVoucherDetail);
                        }
                        return voucherEntry;
                    }, "Form,Id");
        }

        public IEnumerable<PaymentVoucherDTO> GetListByTargetId(int targetId, int clearingId)
        {
            return GetListByTargetId(targetId, clearingId);
        }

        public IEnumerable<PaymentVoucherDTO> GetListByTargetUid(string targetId, int clearingId)
        {
            return GetListByTargetId(targetId, clearingId);
        }

        public IEnumerable<PaymentVoucherDTO> GetListByTargetId(object targetId, int clearingId)
        {
            var dapperExecution = BuildByTemplate<PaymentVoucherDTO>();

            dapperExecution.SqlBuilder.Select("SUM(pvt.TaxValue) AS `TaxPercentage`");
            dapperExecution.SqlBuilder.LeftJoin("PaymentVoucherTaxes pvt ON t1.Id = pvt.VoucherId AND pvt.IsDeleted = FALSE");
            if (clearingId > 0)
            {
                dapperExecution.SqlBuilder.Where("t1.ClearingId = @clearingId", new { clearingId });
            }
            else
            {
                dapperExecution.SqlBuilder.Where("t1.GrandTotal > 0 AND t1.StatusId = @statusId", new { statusId = PaymentVoucherStatus.New.Id });
            }

            dapperExecution.SqlBuilder.Where("t1.TypeId <> @typeId", new { typeId = PaymentVoucherType.Clearing.Id });

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


        private PaymentVoucherDTO FindDetail(object id)
        {
            var cached = new Dictionary<int, PaymentVoucherDTO>();
            var dapperExecution = BuildByTemplate<PaymentVoucherDTO, PaymentVoucherSqlBuilder>();

            //dapperExecution.SqlBuilder.Select("t1.`StartBillingDate`");
            //dapperExecution.SqlBuilder.Select("t1.`EndBillingDate`");
            dapperExecution.SqlBuilder.Select(@" CONCAT( IFNULL(t1.Description,'') , 
                                                                      CASE WHEN IFNULL(t1.CancellationReason,'') = '' THEN '' 
                                                                        ELSE CONCAT(' Lý do hủy: ',t1.CancellationReason) END
                                                             ) AS `Description`");
            dapperExecution.SqlBuilder.Select("c.`CodeClearing`");
            dapperExecution.SqlBuilder.Select("t4.`IsEnterprise` AS `IsEnterprise`");

            dapperExecution.SqlBuilder.Select("t1.`Payment_Form` AS `Form`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Method` AS `Method`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_Address` AS `Address`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_BankName` AS `BankName`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_BankAccount` AS `BankAccount`");
            dapperExecution.SqlBuilder.Select("t1.`Payment_BankBranch` AS `BankBranch`");

            // Select the payment voucher detail
            dapperExecution.SqlBuilder.SelectVoucherDetail("t2");
            // Select tax categories
            dapperExecution.SqlBuilder.Select("lt.`Id`");
            dapperExecution.SqlBuilder.Select("lt.`VoucherLineId`");
            dapperExecution.SqlBuilder.Select("lt.`TaxName`");
            dapperExecution.SqlBuilder.Select("lt.`TaxCode`");
            dapperExecution.SqlBuilder.Select("lt.`TaxValue`");

            dapperExecution.SqlBuilder.SelectVoucherTarget("t4");

            // Select tax categories
            dapperExecution.SqlBuilder.Select("tax.`Id`");
            dapperExecution.SqlBuilder.Select("tax.`TaxName`");
            dapperExecution.SqlBuilder.Select("tax.`TaxCode`");
            dapperExecution.SqlBuilder.Select("tax.`TaxValue`");

            // Select the voucher payment detail
            dapperExecution.SqlBuilder.Select("t3.Id");
            dapperExecution.SqlBuilder.Select("t3.PaymentVoucherId");
            dapperExecution.SqlBuilder.Select("t3.PaymentMethod");
            dapperExecution.SqlBuilder.Select("t3.PaymentMethodName");
            dapperExecution.SqlBuilder.Select("t3.PaidAmount");
            dapperExecution.SqlBuilder.Select("t3.PaymentTurn");
            dapperExecution.SqlBuilder.Select("t3.CreatedDate AS PaymentDate");

            //
            dapperExecution.SqlBuilder.Select("af.Id");
            dapperExecution.SqlBuilder.Select("af.Name");
            dapperExecution.SqlBuilder.Select("af.PaymentVoucherId");
            dapperExecution.SqlBuilder.Select("af.PaymentVoucherDetailId");
            dapperExecution.SqlBuilder.Select("af.FileName");
            dapperExecution.SqlBuilder.Select("af.FilePath");
            dapperExecution.SqlBuilder.Select("af.Size");
            dapperExecution.SqlBuilder.Select("af.FileType");
            dapperExecution.SqlBuilder.Select("af.Extension");
            dapperExecution.SqlBuilder.Select("af.RedirectLink");

            dapperExecution.SqlBuilder.InnerJoin(
                "VoucherTargets t4 ON t1.TargetId = t4.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "PaymentVoucherDetails t2 ON t1.Id = t2.PaymentVoucherId AND t2.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "ReceiptVoucherLineTaxes lt ON lt.VoucherLineId = t2.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "PaymentVoucherPaymentDetails t3 ON t1.Id = t3.PaymentVoucherId AND t3.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin("PaymentVoucherTaxes tax ON tax.VoucherId = t1.Id");

            dapperExecution.SqlBuilder.LeftJoin(
                "Clearings c ON c.Id = t1.ClearingId AND c.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "AttachmentFiles af ON t2.Id = af.PaymentVoucherDetailId AND af.IsDeleted = FALSE");

            if (id is int)
            {
                dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            }
            else if (id is string)
            {
                dapperExecution.SqlBuilder.Where("t1.IdentityGuid = @id", new { id });
            }

            WithConnection(conn => conn.Query(dapperExecution.ExecutionTemplate.RawSql,
                new[]
                {
                    typeof(PaymentVoucherDTO),
                    typeof(PaymentMethod),
                    typeof(PaymentVoucherDetailDTO),
                    typeof(PaymentVoucherLineTaxDTO),
                    typeof(VoucherTargetDTO),
                    typeof(TaxCategoryDTO),
                    typeof(PaymentVoucherPaymentDetailDTO),
                    typeof(AttachmentFileDTO),
                }, results =>
                {
                    var voucherEntry = results[0] as PaymentVoucherDTO;
                    if (voucherEntry == null) return null;

                    if (!cached.TryGetValue(voucherEntry.Id, out var result))
                    {
                        result = voucherEntry;
                        cached.Add(voucherEntry.Id, voucherEntry);
                    }

                    result.Payment = results[1] as PaymentMethod;

                    if (results[2] is PaymentVoucherDetailDTO paymentVoucherDetail)
                    {
                        //if (result.PaymentVoucherDetails.All(s => s.Id != paymentVoucherDetail.Id))
                        //    result.PaymentVoucherDetails.Add(paymentVoucherDetail);

                        if (result.PaymentVoucherDetails.Any(r => r.Id == paymentVoucherDetail.Id))
                        {
                            paymentVoucherDetail = result.PaymentVoucherDetails.First(r => r.Id == paymentVoucherDetail.Id);
                        }
                        else
                        {
                            result.PaymentVoucherDetails.Add(paymentVoucherDetail);
                        }

                        if (results[3] is PaymentVoucherLineTaxDTO lineTaxValue
                            && lineTaxValue != null)
                        {
                            if (paymentVoucherDetail.PaymentVoucherLineTaxes.All(s => s.Id != lineTaxValue.Id))
                                paymentVoucherDetail.PaymentVoucherLineTaxes.Add(lineTaxValue);
                        }

                        if (results[7] is AttachmentFileDTO attachmentFile)
                        {
                            if (paymentVoucherDetail.AttachmentFiles.All(s => s.Id != attachmentFile.Id))
                            {
                                paymentVoucherDetail.AttachmentFiles.Add(attachmentFile);
                            }
                        }
                    }

                    result.Target = results[4] as VoucherTargetDTO;

                    if (results[5] is TaxCategoryDTO taxCategory
                        && taxCategory != null)
                    {
                        if (result.PaymentVoucherTaxes.All(s => s.Id != taxCategory.Id))
                            result.PaymentVoucherTaxes.Add(taxCategory);
                    }

                    if (results[6] is PaymentVoucherPaymentDetailDTO paymentDetail)
                    {
                        if (result.PaymentDetails.All(s => s.Id != paymentDetail.Id))
                            result.PaymentDetails.Add(paymentDetail);
                    }

                    return result;
                }, dapperExecution.ExecutionTemplate.Parameters,
                null,
                true,
                "Form,Id,Id,Id,Id,Id,Id"));

            var result = cached.Values.FirstOrDefault();
            if (result != null)
            {
                var latestPaymentTurn = result.PaymentDetails
                    .Select(c => c.PaymentTurn)
                    .DefaultIfEmpty(0)
                    .Max();

                if (result.StatusId != PaymentVoucherStatus.SentToAccountant.Id
                    && result.StatusId != PaymentVoucherStatus.Canceled.Id
                    && result.GrandTotal > result.PaidTotal)
                {
                    foreach (var method in VoucherPaymentMethod.ActiveList())
                    {
                        var defaultPaymentDetail = new PaymentVoucherPaymentDetailDTO()
                        {
                            PaidAmount = 0,
                            PaymentMethod = method.Id,
                            PaymentMethodName = method.Name,
                            PaymentDate = DateTime.Now,
                            PaymentVoucherId = 0
                        };
                        defaultPaymentDetail.PaymentTurn = latestPaymentTurn == 0 ? 1 : latestPaymentTurn + 1;
                        defaultPaymentDetail.PaymentDate = DateTime.Now;
                        result.PaymentDetails.Add(defaultPaymentDetail);
                    }
                }
            }

            return result;
        }

        public PaymentVoucherDTO Find(string id)
        {
            return this.FindDetail(id);
        }

        public PaymentVoucherDTO Find(int id)
        {
            return this.FindDetail(id);
        }

        public IEnumerable<int> GetPaymentVoucherIds(string inContractCode)
        {
            var dapperExecution = BuildByTemplate<int>();
            dapperExecution.SqlBuilder.Select("t1.Id");
            dapperExecution.SqlBuilder.Where("t1.ContractCode = @ContractCode", new { ContractCode = inContractCode });
            return dapperExecution.ExecuteQuery();
        }
    }
}