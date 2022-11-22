using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models.CustomerViewModels;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.CustomerStructure
{
    public class CustomerStructureCommandHandler : IRequestHandler<CustomerStructureCommand, ActionResponse<CustomerStructureDTO>>
    {
        private readonly ICustomerStructureRepository _customerStructureRepo;

        public CustomerStructureCommandHandler(ICustomerStructureRepository customerStructureRepo)
        {
            _customerStructureRepo = customerStructureRepo;
        }

        public async Task<ActionResponse<CustomerStructureDTO>> Handle(CustomerStructureCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<CustomerStructureDTO>();

            if (request.Id == 0)
            {
                commandResponse.CombineResponse(await _customerStructureRepo.CreateAndSave(request));
            }
            else
            {
                commandResponse.CombineResponse(await _customerStructureRepo.UpdateAndSave(request));
            }

            return commandResponse;
        }
    }
}
