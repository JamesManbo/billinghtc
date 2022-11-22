using ContractManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.TransactionNotification
{
    public class TransactionEmailContentBuilder
    {
        private StringBuilder _stringBuilder;
        private readonly string WebsiteDomain = "https://billing.htc-itc.vn/";

        public TransactionEmailContentBuilder()
        {
            this._stringBuilder = new StringBuilder();
        }
        public void Break()
        {
            this._stringBuilder.AppendLine("<br/>");
        }

        public TransactionEmailContentBuilder OpenTable(int width = 100)
        {
            this._stringBuilder.AppendFormat("<table style=\"{0}width: {1}%; min-width: {1}%\">", TransactionEmailStyle.Table, width);
            return this;
        }

        public TransactionEmailContentBuilder CloseTable()
        {
            this._stringBuilder.AppendLine("</table>");
            return this;
        }
        public TransactionEmailContentBuilder OpenThead()
        {
            this._stringBuilder.AppendLine("<thead>");
            this._stringBuilder.AppendLine("<tr>");
            return this;
        }
        public TransactionEmailContentBuilder CloseThead()
        {
            this._stringBuilder.AppendLine("</thead>");
            this._stringBuilder.AppendLine("</tr>");
            return this;
        }
        public TransactionEmailContentBuilder OpenTableBody()
        {
            this._stringBuilder.AppendLine("<tbody>");
            return this;
        }
        public TransactionEmailContentBuilder CloseTableBody()
        {
            this._stringBuilder.AppendLine("</tbody>");
            return this;
        }
        public TransactionEmailContentBuilder OpenTableRow()
        {
            this._stringBuilder.AppendLine("<tr>");
            return this;
        }
        public TransactionEmailContentBuilder CloseTableRow()
        {
            this._stringBuilder.AppendLine("</tr>");
            return this;
        }

        public TransactionEmailContentBuilder AddTh(string label, int width = 0, string customStyle = "")
        {
            this._stringBuilder.AppendFormat("<th style=\"{0}{3}\" width=\"{1}\">{2}</th>", TransactionEmailStyle.Th, width > 0 ? width.ToString() : "auto", label, customStyle);
            return this;
        }
        public TransactionEmailContentBuilder AddTd(string content, int width = 0, string customStyle = "")
        {
            this._stringBuilder.AppendFormat("<td style=\"{0}{3}\" width=\"{1}\">{2}</td>", TransactionEmailStyle.Td, width > 0 ? width.ToString() : "auto", content, customStyle);
            return this;
        }
        public TransactionEmailContentBuilder OpenTd(int width = 0, string customStyle = "")
        {
            this._stringBuilder.AppendFormat("<td style=\"{0}{2}\" width=\"{1}\">", TransactionEmailStyle.Td, width > 0 ? width.ToString() : "auto", customStyle);
            return this;
        }
        
        public TransactionEmailContentBuilder CloseTd()
        {
            this._stringBuilder.AppendFormat("</td>");
            return this;
        }

        public TransactionEmailContentBuilder OpenUl()
        {
            this._stringBuilder.AppendFormat("<ul style=\"{0}\">", TransactionEmailStyle.Ul);
            return this;
        }

        public TransactionEmailContentBuilder CloseUl()
        {
            this._stringBuilder.AppendLine("</ul>");
            return this;
        }

        public TransactionEmailContentBuilder AddLi(string content, string customStyle = "")
        {
            this._stringBuilder.AppendFormat("<li style=\"{0}\">{1}</li>", content, customStyle);
            return this;
        }
        public TransactionEmailContentBuilder AddHtml(string text, params string[] objs)
        {
            this._stringBuilder.AppendFormat("{0}", text);
            return this;
        }

        public TransactionEmailContentBuilder AddH1(string text)
        {
            this._stringBuilder.AppendFormat("<h1>{0}</h1>", text);
            return this;
        }
        public TransactionEmailContentBuilder AddH2(string text)
        {
            this._stringBuilder.AppendFormat("<h2>{0}</h2>", text);
            return this;
        }
        public TransactionEmailContentBuilder AddH3(string text)
        {
            this._stringBuilder.AppendFormat("<h3>{0}</h3>", text);
            return this;
        }
        public TransactionEmailContentBuilder AddH4(string text)
        {
            this._stringBuilder.AppendFormat("<h4>{0}</h4>", text);
            return this;
        }
        public TransactionEmailContentBuilder AddP(string text)
        {
            this._stringBuilder.AppendFormat("<p>{0}</p>", text);
            return this;
        }
        public TransactionEmailContentBuilder AddTextStrong(string text)
        {
            this._stringBuilder.AppendFormat("<strong>{0}</strong>", text);
            return this;
        }
        public TransactionEmailContentBuilder AddTextItalic(string text)
        {
            this._stringBuilder.AppendFormat("<i>{0}</i>", text);
            return this;
        }
        public TransactionEmailContentBuilder AddTextWithLabel(string label, string text)
        {
            this._stringBuilder.AppendFormat("<label>{0}</label>: {1}", label, text);
            return this;
        }

        public TransactionEmailContentBuilder AddMailHeader1(string contractCode, ContractorDTO contractor)
        {
            this.AddHtml("Hợp đồng số ")
                .AddTextStrong(contractCode)
                .Break();
            this.AddTextWithLabel("Khách hàng", contractor.ContractorFullName);
            this.Break();
            this.AddTextWithLabel("Số điện thoại", contractor.ContractorPhone);
            this.Break();
            return this;
        }

        public TransactionEmailContentBuilder AddMailFooter(string transactionCode)
        {
            this.AddTextItalic($"*Chi tiết xem tại đây: <a href=\"{WebsiteDomain}/transaction-management?c={transactionCode}\">{transactionCode}</a>");
            this.Break();
            this.Break();
            this.AddHtml("<p>Email được gửi tự động từ <strong>Phần mềm quản lý hợp đồng và tính cước.</strong></p>");
            this.AddHtml("<p>Vui lòng không trả lời email này.</p>");
            return this;
        }

        public string Build()
        {
            var body = _stringBuilder.ToString();
            return $"<body>{body}<body/>";
        }
    }
}
