using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Infrastructure.Repositories;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.ReceiptVoucherCommandHandler
{
    public class ImportReceiptVoucherCommandHandler : IRequestHandler<ImportReceiptVoucherCommand, ActionResponse<int>>
    {
        private readonly IMediator _mediator;
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly IReceiptVoucherDetailRepository _receiptVoucherDetailRepository;

        public ImportReceiptVoucherCommandHandler(
            IMediator mediator,
            IReceiptVoucherRepository receiptVoucherRepository,
            IReceiptVoucherDetailRepository receiptVoucherDetailRepository)
        {
            _mediator = mediator;
            _receiptVoucherRepository = receiptVoucherRepository;
            _receiptVoucherDetailRepository = receiptVoucherDetailRepository;
        }

        public async Task<ActionResponse<int>> Handle(ImportReceiptVoucherCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<int>();

            List<string> listReceiptVoucherCode = new List<string>();
            Dictionary<string, string> invoicedDictionary = new Dictionary<string, string>();
            Dictionary<string, decimal> listCashTotal = new Dictionary<string, decimal>();
            Dictionary<string, decimal> listRemainingTotal = new Dictionary<string, decimal>();
            using (var stream = new MemoryStream())
            {
                await request.FormFileReceiptVoucher.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var voucherCodeHeaderIdx = GetColumnByName(worksheet, "so-phieu-thu");
                    var invoiceCodeHeaderIdx = GetColumnByName(worksheet, "so-hoa-don");
                    //var remainingTotalHeaderIdx = GetColumnByName(worksheet, "so-tien-con-thieu") ?? 9;
                    //var cashTotalHeaderIdx = GetColumnByName(worksheet, "so-tien-thuc-thu") ?? 8;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (!voucherCodeHeaderIdx.HasValue || !invoiceCodeHeaderIdx.HasValue) continue;

                        var voucherCode = worksheet.Cells[row, voucherCodeHeaderIdx.Value].Value?.ToString()?.Trim();
                        var invoiceCode = worksheet.Cells[row, invoiceCodeHeaderIdx.Value].Value?.ToString()?.Trim();
                        //var remainingTotal = worksheet.Cells[row, remainingTotalHeaderIdx].Value?.ToString()?.Trim();
                        //var cashTotal = worksheet.Cells[row, cashTotalHeaderIdx].Value?.ToString()?.Trim();

                        if (voucherCode != null
                            && !string.IsNullOrWhiteSpace(invoiceCode)
                            && !string.IsNullOrEmpty(voucherCode.ToString().Trim())
                            && !listReceiptVoucherCode.Contains(voucherCode.ToString().Trim()))
                        {
                            listReceiptVoucherCode.Add(voucherCode.ToString().Trim());
                            invoicedDictionary.Add(voucherCode, invoiceCode);
                            //listRemainingTotal.Add(voucherCode, decimal.Parse(remainingTotal));
                            //listCashTotal.Add(voucherCode, decimal.Parse(cashTotal));
                        }
                    }
                }
            }

            if (listReceiptVoucherCode != null
                && listReceiptVoucherCode.Count > 0)
            {
                var entities = await _receiptVoucherRepository
                    .GetByCodesAsync(listReceiptVoucherCode.ToArray(), ReceiptVoucherStatus.UnpaidStatuses());
                entities.ForEach(e =>
                {
                    e.InvoiceCode = invoicedDictionary[e.VoucherCode];
                    e.InvoiceDate = DateTime.UtcNow.AddHours(7);
                    e.SetStatusId(ReceiptVoucherStatus.Invoiced.Id);
                    e.ApprovedUserId = request.InvoicedUser;
                });
                await _receiptVoucherRepository.SaveChangeAsync();
                commandResponse.SetResult(entities.Count);
            }
            return commandResponse;
        }

        public int? GetColumnByName(ExcelWorksheet ws, string columnName)
        {
            if (ws == null) throw new ArgumentNullException(nameof(ws));
            var firstRow = ws.Cells[1, 1, 1, ws.Dimension.End.Column];
            foreach (var firstRowCell in firstRow)
            {
                if (string.IsNullOrWhiteSpace(firstRowCell.Text)) continue;

                if (columnName.Equals(firstRowCell.Text.Trim().ToAscii(), StringComparison.OrdinalIgnoreCase))
                {
                    return firstRowCell.Start.Column;
                }
            }
            return default;
        }
    }
}
