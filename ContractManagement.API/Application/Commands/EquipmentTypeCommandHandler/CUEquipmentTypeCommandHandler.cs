using System.Threading;
using System.Threading.Tasks;
using ContractManagement.Domain.Commands.EquipmentTypeCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories.EquipmentTypeRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ContractManagement.API.Application.Commands.EquipmentTypeCommandHandler
{
    public class CreateEquipmentTypeCommandHandler : IRequestHandler<CreateEquipmentTypeCommand, ActionResponse<EquipmentTypeDTO>>
    {
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public CreateEquipmentTypeCommandHandler(IEquipmentTypeRepository equipmentTypeRepository, IWrappedConfigAndMapper configAndMapper)
        {
            _equipmentTypeRepository = equipmentTypeRepository;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<EquipmentTypeDTO>> Handle(CreateEquipmentTypeCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<EquipmentTypeDTO>();

            //if (!_equipmentTypeRepository.CheckExistEquipmentTypeName(request.Name))
            //{
            //    commandResponse.AddError("Tên thiết bị đã tồn tại ", nameof(request.Name));
            //    return commandResponse;
            //}

            if (!_equipmentTypeRepository.CheckExistEquipmentTypeCode(request.Code))
            {
                commandResponse.AddError("Mã thiết bị đã tồn tại", nameof(request.Code));
                return commandResponse;
            }

            var createEquipmentTypeResp = await _equipmentTypeRepository.CreateAndSave(request);
            commandResponse.CombineResponse(createEquipmentTypeResp);

            if (createEquipmentTypeResp.IsSuccess)
            {
                commandResponse.SetResult(createEquipmentTypeResp.Result.MapTo<EquipmentTypeDTO>(_configAndMapper.MapperConfig));
                return commandResponse;
            }

            return commandResponse;
        }
    }
    public class UpdateEquipmentTypeCommandHandler : IRequestHandler<UpdateEquipmentTypeCommand, ActionResponse<EquipmentTypeDTO>>
    {
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public UpdateEquipmentTypeCommandHandler(IEquipmentTypeRepository equipmentTypeRepository, IWrappedConfigAndMapper configAndMapper)
        {
            _equipmentTypeRepository = equipmentTypeRepository;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<EquipmentTypeDTO>> Handle(UpdateEquipmentTypeCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<EquipmentTypeDTO>();

            if (!_equipmentTypeRepository.CheckExistEquipmentTypeCode(request.Code))
            {
                commandResponse.AddError("Mã thiết bị đã tồn tại", nameof(request.Code));
                return commandResponse;
            }

            var updateEquipmentTypeResp = await _equipmentTypeRepository.UpdateAndSave(request);
            commandResponse.CombineResponse(updateEquipmentTypeResp);

            if (updateEquipmentTypeResp.IsSuccess)
            {
                commandResponse.SetResult(updateEquipmentTypeResp.Result.MapTo<EquipmentTypeDTO>(_configAndMapper.MapperConfig));
                return commandResponse;
            }

            return commandResponse;
        }
    }
}
