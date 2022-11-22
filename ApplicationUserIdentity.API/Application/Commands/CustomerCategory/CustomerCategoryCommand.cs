using ApplicationUserIdentity.API.Models.CustomerViewModels;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.CustomerCategory
{
    public class CustomerCategoryCommand : IRequest<ActionResponse<CustomerCategoryDTO>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
