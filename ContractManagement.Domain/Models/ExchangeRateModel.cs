using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ContractManagement.Domain.Models
{
    public class ExchangeRateModel
    {
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
        [XmlAttribute(AttributeName = "CurrencyName")]
        public string CurrencyName { get; set; }
        [XmlAttribute(AttributeName = "Buy")]
        public string Buy { get; set; }
        [XmlAttribute(AttributeName = "Transfer")]
        public string Transfer { get; set; }
        [XmlAttribute(AttributeName = "Sell")]
        public string Sell { get; set; }
    }
}
