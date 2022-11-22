using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ReceiptVoucherCommand
{
    public class ImportReceiptVoucherCommand: IRequest<ActionResponse<int>>
    {
        public ImportReceiptVoucherCommand(IFormFile formFile)
        {
            FormFileReceiptVoucher = formFile;
        }

        public IFormFile FormFileReceiptVoucher { get; set; }
        public string InvoicedUser { get; set; }
        public int? TypeImport { get; set; }
    }
}
