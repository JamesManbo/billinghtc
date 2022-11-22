using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.TransactionNotification
{
    public static class TransactionEmailStyle
    {
        public static string Table =>
            "table-layout:fixed;" +
            "margin:12px 0px;" +
            "padding:0;" +
            "border-spacing:0;" +
            "border: 1px solid #ebedf2;" +
            "border-bottom-color: transparent;" +
            "border-right-color: transparent;";

        public static string Th => "border: 1px solid #ebedf2;" +
            "border-width: 0 1px 1px 0;" +
            "font-weight: bold;" +
            "padding-top: 12px;" +
            "padding-bottom: 12px;";
        public static string Td => "border: 1px solid #ebedf2;" +
            "border-width: 0 1px 1px 0;" +
            "word-break: break-word;" +
            "padding: 8px;";
        public static string Ul => "list-style: decimal;" +
            "margin: 0;" +
            "padding: 0;";
    }
}
