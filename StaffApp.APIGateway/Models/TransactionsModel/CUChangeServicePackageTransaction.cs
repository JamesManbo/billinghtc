using Global.Models.StateChangedResponse;
using MediatR;
using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class CUChangeServicePackageTransaction : CUTransactionBase
    {
        public List<CUTransactionServicePackage> TransactionServicePackages { get; set; }
        public List<CreateUpdateFile> AttachmentFiles { get; set; }
    }
}
