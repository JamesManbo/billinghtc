using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.BaseContract
{
    public class TransactionStatus : Enumeration
    {
        public static TransactionStatus WaitAcceptanced = new TransactionStatus(1, "Chờ triển khai");
        public static TransactionStatus Acceptanced = new TransactionStatus(2, "Đã triển khai");
        public static TransactionStatus Cancelled = new TransactionStatus(3, "Từ chối nghiệm thu");
        public static TransactionStatus AcceptanceApproved = new TransactionStatus(4, "Đã nghiệm thu");
        public static TransactionStatus WarehouseInOutConfirmed = new TransactionStatus(5, "Thiết bị đã nhập kho");
        public static TransactionStatus Completed = new TransactionStatus(6, "Hoàn thành");

        public TransactionStatus(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<TransactionStatus> List() => new[]
            {WaitAcceptanced, Cancelled, Acceptanced, AcceptanceApproved, WarehouseInOutConfirmed, Completed};

        public static IEnumerable<SelectionItem> SelectionList() => new SelectionItem[]
            {
                new SelectionItem(){ Value = WaitAcceptanced.Id, Text = WaitAcceptanced.Name },
                new SelectionItem(){ Value = Cancelled.Id, Text = Cancelled.Name },
                new SelectionItem(){ Value = Acceptanced.Id, Text = Acceptanced.Name },
                new SelectionItem(){ Value = AcceptanceApproved.Id, Text = AcceptanceApproved.Name },
                new SelectionItem(){ Value = WarehouseInOutConfirmed.Id, Text = WarehouseInOutConfirmed.Name },
                new SelectionItem(){ Value = Completed.Id, Text = Completed.Name }
            };

        public static TransactionStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (state == null)
            {
                throw new ContractDomainException($"Possible values for TransactionStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }


        public static TransactionStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ContractDomainException($"Possible values for TransactionStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
