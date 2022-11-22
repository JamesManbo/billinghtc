using Dapper;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models;
using DebtManagement.Domain.Models.VoucherTarget;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DebtManagement.Infrastructure.Queries
{
    public interface IVoucherTargetQueries : IQueryRepository
    {
        bool IsExisted(string targetId);
        Dictionary<string, int?> GetAllIds();
        VoucherTargetDTO Find(int id);
        VoucherTargetDTO Find(string id);
        IEnumerable<SelectionItem> AutoComplete(RequestFilterModel requestFilterModel, bool? isPayer = null);
        IEnumerable<SelectionVoucherTargetDTO> AutoCompleteForClearing(RequestFilterModel requestFilterModel);
        IPagedList<VoucherTargetClearingDTO> GetList(RequestFilterModel requestFilterModel);
    }

    public class VoucherTargetQueries : QueryRepository<VoucherTarget, int>, IVoucherTargetQueries
    {
        public VoucherTargetQueries(DebtDbContext context) : base(context)
        {
        }

        public bool IsExisted(string targetId)
        {
            return WithConnection(conn =>
                conn.ExecuteScalar<bool>("SELECT COUNT(1) FROM VoucherTargets where IdentityGuid = @targetId",
                new { targetId }));
        }

        public Dictionary<string, int?> GetAllIds()
        {
            return WithConnection(conn =>
                conn.Query<(string IdentityGuid, int? Id)>("GetAllVoucherTargetIds",
                commandType: CommandType.StoredProcedure).ToDictionary(
                        r => r.IdentityGuid,
                        r => r.Id
                    ));
        }

        public IEnumerable<SelectionItem> AutoComplete(RequestFilterModel requestFilterModel, bool? isPayer = null)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<SelectionItem>();

            dapperExecution.SqlBuilder.Select(
                "CONCAT_WS('', t1.TargetFullName, ', SĐT: ', t1.TargetPhone , ', Đ/c: ', t1.TargetAddress) AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `GlobalValue`");

            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.TargetFullName LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetAddress LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetPhone LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetEmail LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetFax LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetIdNo LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetTaxIdNo LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetBRNo LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" });
            }

            if (isPayer.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.IsPayer = @isPayer", new { isPayer });
            }

            dapperExecution.SqlBuilder.Take(requestFilterModel.Take);
            return dapperExecution.ExecuteQuery();

        }

        public IEnumerable<SelectionVoucherTargetDTO> AutoCompleteForClearing(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<SelectionVoucherTargetDTO>();
            dapperExecution.SqlBuilder.Select(
                "CONCAT_WS('', t1.TargetFullName, ', SĐT: ', t1.TargetPhone , ', Đ/c: ', t1.TargetAddress) AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            dapperExecution.SqlBuilder.Where("NULLIF(t1.UserIdentityGuid, '') IS NOT NULL");
            dapperExecution.SqlBuilder.Where("NULLIF(t1.ApplicationUserIdentityGuid, '') IS NOT NULL");
            dapperExecution.SqlBuilder.GroupBy("t1.UserIdentityGuid, t1.ApplicationUserIdentityGuid");

            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.TargetFullName LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetAddress LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetPhone LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetEmail LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetFax LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetIdNo LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.TargetTaxIdNo LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" });
            }

            dapperExecution.SqlBuilder.Take(requestFilterModel.Take);
            return dapperExecution.ExecuteQuery();

        }

        public IPagedList<VoucherTargetClearingDTO> GetList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<VoucherTargetClearingDTO>(requestFilterModel);

            dapperExecution.SqlBuilder.Select("t1.`TargetFullName` AS `TargetFullName`");
            dapperExecution.SqlBuilder.Select("t1.`TargetCode` AS `TargetCode`");
            dapperExecution.SqlBuilder.Select("(SELECT SUM(rv.GrandTotal) FROM ReceiptVouchers rv WHERE t1.Id = rv.TargetId AND rv.GrandTotal > 0 " +
                "AND rv.IsDeleted = FALSE AND rv.IsActive = TRUE AND rv.IssuedDate IS NOT NULL AND rv.IssuedDate <= NOW() " +
                "AND rv.StatusId = @receiptStatusId AND rv.TypeId <> @typeId) AS `TotalReceiptVouchers`"
                , new { receiptStatusId = ReceiptVoucherStatus.Pending.Id, typeId = ReceiptVoucherType.Clearing.Id });
            dapperExecution.SqlBuilder.Select("(SELECT SUM(pv.GrandTotal) FROM PaymentVouchers pv WHERE t1.UserIdentityGuid = (SELECT UserIdentityGuid FROM VoucherTargets WHERE Id = pv.TargetId LIMIT 1) AND pv.GrandTotal > 0 " +
                                                    "AND pv.IsDeleted = FALSE AND pv.IsActive = TRUE AND pv.IssuedDate IS NOT NULL AND pv.IssuedDate <= NOW()" +
                                                    " AND pv.StatusId = @paymentStatusId AND pv.TypeId <> @typeId) AS `TotalPaymentVouchers`"
                                                    , new { paymentStatusId = PaymentVoucherStatus.New.Id, typeId = PaymentVoucherType.Clearing.Id });


            dapperExecution.SqlBuilder.Where("EXISTS (SELECT 1 FROM ReceiptVouchers rv WHERE t1.Id = rv.TargetId AND rv.GrandTotal > 0 " +
                                                    "AND rv.IsDeleted = FALSE AND rv.IsActive = TRUE AND rv.IssuedDate IS NOT NULL AND rv.IssuedDate <= NOW() AND rv.StatusId = @receiptStatusId AND rv.TypeId <> @typeId)"
                                                    , new { receiptStatusId = ReceiptVoucherStatus.Pending.Id, typeId = ReceiptVoucherType.Clearing.Id });
            dapperExecution.SqlBuilder.Where("EXISTS (SELECT 1 FROM PaymentVouchers pv WHERE t1.UserIdentityGuid = (SELECT UserIdentityGuid FROM VoucherTargets WHERE Id = pv.TargetId LIMIT 1) AND pv.GrandTotal > 0 " +
                                                    "AND pv.IsDeleted = FALSE AND pv.IsActive = TRUE AND pv.IssuedDate IS NOT NULL AND pv.IssuedDate <= NOW() AND pv.StatusId = @paymentStatusId AND pv.TypeId <> @typeId)"
                                                    , new { paymentStatusId = PaymentVoucherStatus.New.Id, typeId = PaymentVoucherType.Clearing.Id });

            return dapperExecution.ExecutePaginateQuery();

        }
        private VoucherTargetDTO FindDetail(object id)
        {
            var cached = new Dictionary<int, VoucherTargetDTO>();
            var dapperExecution = BuildByTemplate<VoucherTargetDTO>();

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
                    typeof(VoucherTargetDTO)
                }, results =>
                {
                    var voucherEntry = results[0] as VoucherTargetDTO;
                    if (voucherEntry == null) return null;

                    if (!cached.TryGetValue(voucherEntry.Id, out var result))
                    {
                        result = voucherEntry;
                        cached.Add(voucherEntry.Id, voucherEntry);
                    }

                    return result;
                }, dapperExecution.ExecutionTemplate.Parameters,
                null,
                true,
                ""));

            return cached.Values.FirstOrDefault();
        }

        public VoucherTargetDTO Find(string id)
        {
            return this.FindDetail(id);
        }

        public VoucherTargetDTO Find(int id)
        {
            return this.FindDetail(id);
        }
    }
}