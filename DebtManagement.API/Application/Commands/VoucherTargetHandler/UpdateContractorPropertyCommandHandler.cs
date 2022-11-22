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
    public class UpdateContractorPropertyCommandHandler : IRequestHandler<UpdateContractorPropertyCommand>
    {
        private readonly IVoucherTargetPropertyRepository _voucherTargetPropertyRepository;

        public UpdateContractorPropertyCommandHandler(IVoucherTargetPropertyRepository voucherTargetPropertyRepository)
        {
            this._voucherTargetPropertyRepository = voucherTargetPropertyRepository;
        }

        public Task<Unit> Handle(UpdateContractorPropertyCommand request, CancellationToken cancellationToken)
        {
            if (request.CategoryId.HasValue)
            {
                _voucherTargetPropertyRepository.UpdateAllCategory(request.CategoryId.Value,
                    request.CategoryName);
            }

            if (request.ClassId.HasValue)
            {
                _voucherTargetPropertyRepository.UpdateAllClass(request.ClassId.Value,
                    request.ClassName);
            }

            if (!string.IsNullOrEmpty(request.GroupId))
            {
                _voucherTargetPropertyRepository.UpdateAllGroup(request.GroupId,
                    request.OldGroupName,
                    request.NewGroupName);
            }

            if (!string.IsNullOrEmpty(request.IndustryId))
            {
                _voucherTargetPropertyRepository.UpdateAllIndustry(request.IndustryId,
                    request.OldIndustryName,
                    request.NewIndustryName);
            }

            if (request.StructureId.HasValue)
            {
                _voucherTargetPropertyRepository.UpdateAllStructure(request.StructureId.Value,
                    request.StructureName);
            }

            if (request.TypeId.HasValue)
            {
                _voucherTargetPropertyRepository.UpdateAllType(request.TypeId.Value,
                    request.TypeName);
            }

            return Task.FromResult(Unit.Value);
        }
    }
}
