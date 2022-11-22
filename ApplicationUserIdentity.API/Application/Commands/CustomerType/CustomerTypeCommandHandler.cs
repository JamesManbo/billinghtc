using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models.CustomerViewModels;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.CustomerType
{
    public class CustomerTypeCommandHandler : IRequestHandler<CustomerTypeCommand, ActionResponse<CustomerTypeDTO>>
    {
        private readonly ICustomerTypeRepository _CustomerTypeRepo;

        public CustomerTypeCommandHandler(ICustomerTypeRepository CustomerTypeRepo)
        {
            _CustomerTypeRepo = CustomerTypeRepo;

        }
        public async Task<ActionResponse<CustomerTypeDTO>> Handle(CustomerTypeCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<CustomerTypeDTO>();

            if (request.Id == 0)
            {
                commandResponse.CombineResponse(await _CustomerTypeRepo.CreateAndSave(request));
            }
            else
            {
                commandResponse.CombineResponse(await _CustomerTypeRepo.UpdateAndSave(request));
            }
            return commandResponse;
        }
    }
}
