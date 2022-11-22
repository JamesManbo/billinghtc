using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.ServicePackagePriceCommand
{
    public class CUServicePackagePriceCommand : IRequest<ActionResponse<ServicePackagePriceDTO>>
    {
        public int Id { get; set; }
        public int ServicePackageId { get; set; }
        public decimal PriceBeforeTax { get; set; }
        public decimal PriceValue { get; set; }
        public int  ProjectId { get; set; }
        public int TaxCategoryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
    }
}
