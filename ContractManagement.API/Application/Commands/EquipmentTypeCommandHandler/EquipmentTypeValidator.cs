using ContractManagement.Domain.Commands.EquipmentTypeCommand;
using FluentValidation;

namespace ContractManagement.API.Application.Commands.EquipmentTypeCommandHandler
{

    public class CreateEquipmentTypeValidator : AbstractValidator<CreateEquipmentTypeCommand>
    {
        public CreateEquipmentTypeValidator()
        {
            RuleFor(equipment => equipment.Name).NotNull().WithMessage("Tên thiết bị người dùng là bắt buộc");
            RuleFor(equipment => equipment.Name).MinimumLength(3).WithMessage("Tên thiết bị quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(equipment => equipment.Name).MaximumLength(256).WithMessage("Tên thiết bị quá dài(tối đa 256 ký tự)");
            RuleFor(equipment => equipment.Code).NotNull().WithMessage("Mã thiết bị người dùng là bắt buộc");
            RuleFor(equipment => equipment.Code).MinimumLength(3).WithMessage("Mã thiết bị quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(equipment => equipment.Code).MaximumLength(256).WithMessage("Mã thiết bị quá dài(tối đa 256 ký tự)");
            RuleFor(equipment => equipment.Code).Matches("^[a-zA-Z0-9][a-zA-Z0-9-_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu, ký tự gạch ngang(-) và gạch dưới(_)");
            RuleFor(equipment => equipment.UnitOfMeasurementId).GreaterThan(0).WithMessage("Đơn vị đo là bắt buộc");
            RuleFor(equipment => equipment.Price).GreaterThanOrEqualTo(0).WithMessage("Giá trước thuế phải lớn hơn 0");
        }
    }
    public class UpdateEquipmentTypeValidator : AbstractValidator<UpdateEquipmentTypeCommand>
    {
        public UpdateEquipmentTypeValidator()
        {
            RuleFor(equipment => equipment.Id).NotNull().GreaterThan(0).WithMessage("Thiết bị người dùng không tồn tại"); 
            RuleFor(Equipment => Equipment.Name).NotEmpty().WithMessage("Tên thiết bị người dùng là bắt buộc");
            RuleFor(equipment => equipment.Name).MinimumLength(3).WithMessage("Tên thiết bị quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(equipment => equipment.Name).MaximumLength(256).WithMessage("Tên thiết bị quá dài(tối đa 256 ký tự)");
            RuleFor(equipment => equipment.Code).NotEmpty().WithMessage("Mã thiết bị người dùng là bắt buộc");
            RuleFor(equipment => equipment.Code).MinimumLength(3).WithMessage("Mã thiết bị quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(equipment => equipment.Code).MaximumLength(256).WithMessage("Mã thiết bị quá dài(tối đa 256 ký tự)");
            RuleFor(equipment => equipment.Code).Matches("^[a-zA-Z0-9][a-zA-Z0-9-_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu, ký tự gạch ngang(-) và gạch dưới(_)");
            RuleFor(equipment => equipment.UnitOfMeasurementId).GreaterThan(0).WithMessage("Đơn vị đo là bắt buộc");
            RuleFor(equipment => equipment.Price).GreaterThanOrEqualTo(0).WithMessage("Giá trước thuế phải lớn hơn 0");
        }
    }
}
