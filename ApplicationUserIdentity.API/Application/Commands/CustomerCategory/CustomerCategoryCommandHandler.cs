using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models.CustomerViewModels;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.CustomerCategory
{
    public class CustomerCategoryCommandHandler : IRequestHandler<CustomerCategoryCommand, ActionResponse<CustomerCategoryDTO>>
    {
        private readonly ICustomerCategoryRepository _customerCategoryRepo;

        public CustomerCategoryCommandHandler(ICustomerCategoryRepository customerCategoryRepo)
        {
            _customerCategoryRepo = customerCategoryRepo;

        }
        public async Task<ActionResponse<CustomerCategoryDTO>> Handle(CustomerCategoryCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<CustomerCategoryDTO>();

            if (request.Id == 0)
            {
                commandResponse.CombineResponse(await _customerCategoryRepo.CreateAndSave(request));
            }
            else
            {
                commandResponse.CombineResponse(await _customerCategoryRepo.UpdateAndSave(request));
            }
            return commandResponse;
        }
    }
}
