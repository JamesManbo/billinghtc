using DebtManagement.Domain.Commands.VoucherTargetCommand;
using DebtManagement.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.VoucherTargetHandler
{
    public class RemoveContractorPropertyCommandHandler : IRequestHandler<RemoveContractorPropertyCommand>
    {
        private readonly IVoucherTargetPropertyRepository _voucherTargetPropertyRepository;

        public RemoveContractorPropertyCommandHandler(IVoucherTargetPropertyRepository voucherTargetPropertyRepository)
        {
            this._voucherTargetPropertyRepository = voucherTargetPropertyRepository;
        }

        public async Task<Unit> Handle(RemoveContractorPropertyCommand request, CancellationToken cancellationToken)
        {
            if (request.CategoryId.HasValue)
            {
                _voucherTargetPropertyRepository.RemoveAllCategory(request.CategoryId.Value);
            }

            if (request.ClassId.HasValue)
            {
                _voucherTargetPropertyRepository.RemoveAllClass(request.ClassId.Value);
            }

            if (request.GroupId.HasValue)
            {
                _voucherTargetPropertyRepository.RemoveAllGroup(request.GroupId.Value.ToString(), request.GroupName);
            }

            if (request.IndustryId.HasValue)
            {
                _voucherTargetPropertyRepository.RemoveAllIndustry(request.IndustryId.Value);
            }

            if (request.StructureId.HasValue)
            {
                _voucherTargetPropertyRepository.RemoveAllStructure(request.StructureId.Value);
            }

            if (request.TypeId.HasValue)
            {
                _voucherTargetPropertyRepository.RemoveAllType(request.TypeId.Value);
            }

            return await Task.FromResult(Unit.Value);
        }
    }
}
