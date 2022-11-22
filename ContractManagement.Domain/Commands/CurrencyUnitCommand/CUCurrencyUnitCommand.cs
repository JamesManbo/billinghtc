using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.CUCurrencyUnitCommand
{
    public class CreateCurrencyUnitCommand : IRequest<ActionResponse<CurrencyUnitDTO>>
    {
        public string CurrencyUnitName { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string IssuingCountry { get; set; }
        public string CurrencyUnitSymbol { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class UpdateCurrencyUnitCommand : IRequest<ActionResponse<CurrencyUnitDTO>>
    {
        public int Id { get; set; }
        public string CurrencyUnitName { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string IssuingCountry { get; set; }
        public string CurrencyUnitSymbol { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
