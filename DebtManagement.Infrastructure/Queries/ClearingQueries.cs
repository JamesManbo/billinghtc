using DebtManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.AggregatesModel.ClearingAggregate;
using Dapper;
using System.Linq;
using GenericRepository.DapperSqlBuilder;

namespace DebtManagement.Infrastructure.Queries
{
    public interface IClearingQueries : IQueryRepository
    {
        IPagedList<ClearingDTO> GetList(RequestFilterModel requestFilterModel);
        ClearingDTO Find(string id);
        ClearingDTO Find(int id);
        int GetOrderNumberByNow();
    }

    public class ClearingSqlBuilder : SqlBuilder {

        public ClearingSqlBuilder() { }
        public ClearingSqlBuilder(string tableName) : base(tableName)
        {
        }

        public ClearingSqlBuilder SelectVoucherTarget(string alias)
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
    }

    public class ClearingQueries : QueryRepository<Clearing, int>, IClearingQueries
    {
        public ClearingQueries(DebtDbContext context) : base(context)
        {
        }

        public IPagedList<ClearingDTO> GetList(RequestFilterModel requestFilterModel)
        {

            var dapperExecution = BuildByTemplate<ClearingDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("t2.`TargetFullName`");
            dapperExecution.SqlBuilder.Select("t2.`TargetPhone`");

            dapperExecution.SqlBuilder.LeftJoin("VoucherTargets AS t2 ON t2.Id = t1.TargetId");

            return dapperExecution.ExecutePaginateQuery();
        }
        private ClearingDTO FindDetail(object id)
        {
            var cached = new Dictionary<int, ClearingDTO>();
            var dapperExecution = BuildByTemplate<ClearingDTO, ClearingSqlBuilder>();

            dapperExecution.SqlBuilder.SelectVoucherTarget("vt");

            // Truy vấn phiếu thu thực hiện bù trừ
            dapperExecution.SqlBuilder.Select("t2.`Id`");
            dapperExecution.SqlBuilder.Select("t2.`IssuedDate`");
            dapperExecution.SqlBuilder.Select("t2.`VoucherCode`");
            dapperExecution.SqlBuilder.Select("t2.`InvoiceCode`");
            dapperExecution.SqlBuilder.Select("t2.`GrandTotalBeforeTax`");
            dapperExecution.SqlBuilder.Select("t2.`TaxAmount`");
            dapperExecution.SqlBuilder.Select("t2.`GrandTotal`");
            dapperExecution.SqlBuilder.Select("t2.`StatusId`");
            dapperExecution.SqlBuilder.Select("t2.`CurrencyUnitCode`");
            dapperExecution.SqlBuilder.Select("t2.`CurrencyUnitId`");

            // Truy vấn phiếu đề nghị thanh toán thực hiện bù trừ
            dapperExecution.SqlBuilder.Select("t3.`Id`");
            dapperExecution.SqlBuilder.Select("t3.`IssuedDate`");
            dapperExecution.SqlBuilder.Select("t3.`VoucherCode`");
            dapperExecution.SqlBuilder.Select("t3.`InvoiceCode`");
            dapperExecution.SqlBuilder.Select("t3.`GrandTotalBeforeTax`");
            dapperExecution.SqlBuilder.Select("t3.`TaxAmount`");
            dapperExecution.SqlBuilder.Select("t3.`GrandTotal`");
            dapperExecution.SqlBuilder.Select("t3.`StatusId`");
            dapperExecution.SqlBuilder.Select("t3.`CurrencyUnitCode`");
            dapperExecution.SqlBuilder.Select("t3.`CurrencyUnitId`");

            // Truy vấn file đính kèm
            dapperExecution.SqlBuilder.Select("af.Id");
            dapperExecution.SqlBuilder.Select("af.Name");
            dapperExecution.SqlBuilder.Select("af.ClearingVoucherId");
            dapperExecution.SqlBuilder.Select("af.FileName");
            dapperExecution.SqlBuilder.Select("af.FilePath");
            dapperExecution.SqlBuilder.Select("af.Size");
            dapperExecution.SqlBuilder.Select("af.FileType");
            dapperExecution.SqlBuilder.Select("af.Extension");
            dapperExecution.SqlBuilder.Select("af.RedirectLink");

            dapperExecution.SqlBuilder.InnerJoin("VoucherTargets vt ON vt.Id = t1.TargetId");

            dapperExecution.SqlBuilder.LeftJoin("ReceiptVouchers AS t2 ON t2.ClearingId = t1.Id");
            dapperExecution.SqlBuilder.LeftJoin("PaymentVouchers AS t3 ON t3.ClearingId = t1.Id");
            dapperExecution.SqlBuilder.LeftJoin("AttachmentFiles AS af ON af.ClearingVoucherId = t1.Id");

            if (id is int)
            {
                dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            }
            else if(id is string)
            {
                dapperExecution.SqlBuilder.Where("t1.IdentityGuid = @id", new { id });
            }

            return dapperExecution.ExecuteScalarQuery<ClearingDTO, VoucherTargetDTO, ReceiptVoucherDTO, PaymentVoucherDTO, AttachmentFileDTO>(
                    (clearing, voucherTarget, receiptVoucher, paymentVoucher, attachmentFile) =>
                    {
                        if (!cached.TryGetValue(clearing.Id, out var clearingEntry))
                        {
                            clearingEntry = clearing;
                            clearingEntry.Target = voucherTarget;
                            cached.Add(clearing.Id, clearingEntry);
                        }

                        if (receiptVoucher != null 
                            && clearingEntry.ReceiptVouchers.All(r => r.Id != receiptVoucher.Id))
                        {
                            clearingEntry.ReceiptVouchers.Add(receiptVoucher);
                        }

                        if(paymentVoucher != null
                            && clearingEntry.PaymentVouchers.All(p => p.Id != paymentVoucher.Id))
                        {
                            clearingEntry.PaymentVouchers.Add(paymentVoucher);
                        }    

                        if(attachmentFile != null
                            && clearingEntry.AttachmentFiles.All(p => p.Id != attachmentFile.Id))
                        {
                            clearingEntry.AttachmentFiles.Add(attachmentFile);
                        }    

                        return clearingEntry;
                    }, "Id,Id,Id,Id");
        }

        public ClearingDTO Find(int id)
        {
            return this.FindDetail(id);
        }

        public ClearingDTO Find(string id)
        {
            return this.FindDetail(id);
        }

        public int GetOrderNumberByNow()
        {
            return WithConnection(conn =>
                conn.QueryFirst<int>(
                    "SELECT (COUNT(1) + 1) FROM Clearings WHERE DATE(CreatedDate) = CURDATE() AND IsDeleted = FALSE"));
        }
    }
}
