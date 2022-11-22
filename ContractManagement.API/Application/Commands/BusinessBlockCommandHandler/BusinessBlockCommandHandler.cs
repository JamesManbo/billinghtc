using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.BusinessBlockRepostory;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.BusinessBlockCommandHandler
{
    public class BusinessBlockCommandHandler : IRequestHandler<BusinessBlockCommand, ActionResponse<BusinessBlockDTO>>
    {
        private readonly IBusinessBlockRepostory _businessBlockRepository;
        private readonly IBusinessBlockQueries _businessBlockQueries;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public BusinessBlockCommandHandler(IBusinessBlockRepostory businessBlockRepository, IBusinessBlockQueries businessBlockQueries, IWrappedConfigAndMapper configAndMapper)
        {
            _businessBlockRepository = businessBlockRepository;
            _businessBlockQueries = businessBlockQueries;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<BusinessBlockDTO>> Handle(BusinessBlockCommand request, CancellationToken cancellationToken)
        {
            var actionResponse = new ActionResponse<BusinessBlockDTO>();
            var checkExitBusinessBlockName = _businessBlockQueries.CheckExistName(0, request.BusinessBlockName);
            if (checkExitBusinessBlockName)//Tồn tại
            {
                if (!checkExitBusinessBlockName)
                {
                    actionResponse.AddError("Tên khối kinh doanh quản lý đã tồn tại", nameof(request.BusinessBlockName));
                }
                return actionResponse;
            }

            if (request.Id == 0)
            {
                var insertResponse = await _businessBlockRepository.CreateAndSave(request);
                actionResponse.SetResult(insertResponse.Result.MapTo<BusinessBlockDTO>(_configAndMapper.MapperConfig));
                actionResponse.CombineResponse(insertResponse);
            }
            else
            {
                var updateResponse = await _businessBlockRepository.UpdateAndSave(request);
                actionResponse.SetResult(actionResponse.Result.MapTo<BusinessBlockDTO>(_configAndMapper.MapperConfig));
                actionResponse.CombineResponse(updateResponse);
            }

            return actionResponse;
        }
    }
}
