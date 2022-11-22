using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;

namespace ContractManagement.Domain.Commands.TaxCategoryCommand
{
    public class CUTaxCategoryCommand : IRequest<ActionResponse<TaxCategoryDTO>>
    {
        public int Id { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public float TaxValue { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string ExplainTax { get; set; }
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
    }
}
