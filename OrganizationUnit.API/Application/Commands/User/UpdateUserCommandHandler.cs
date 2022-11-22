using System;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using OrganizationUnit.API.Infrastructure.Services;
using System.Threading;
using System.Threading.Tasks;
using Global.Models;
using OrganizationUnit.API.Grpc.StaticResource;
using OrganizationUnit.API.Models;
using OrganizationUnit.Domain.Exceptions;
using OrganizationUnit.Infrastructure.Repositories.PictureRepository;
using OrganizationUnit.API.Application.IntegrationEvents.Events.ApplicationUserEvents;
using OrganizationUnit.API.Application.IntegrationEvents;
using OrganizationUnit.Infrastructure.Repositories;
using OrganizationUnit.Domain.Commands.User;

namespace OrganizationUnit.API.Application.Commands.User
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ActionResponse<string>>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IStaticResourceService _staticResourceService;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IPictureRepository _pictureRepository;
        private readonly IUserIntegrationEventService _integrationEventLogService;

        public UpdateUserCommandHandler(IUserService userService, IStaticResourceService staticResourceService,
            IWrappedConfigAndMapper configAndMapper, IPictureRepository pictureRepository, IUserIntegrationEventService integrationEventLogService,
            IUserRepository userRepository)
        {
            _userService = userService;
            _staticResourceService = staticResourceService;
            _configAndMapper = configAndMapper;
            _pictureRepository = pictureRepository;
            _integrationEventLogService = integrationEventLogService;
            _userRepository = userRepository;
        }

        public async Task<ActionResponse<string>> Handle(UpdateUserCommand request,
            CancellationToken cancellationToken)
        {
            var result = new ActionResponse<string>();
            //if (!string.IsNullOrEmpty(request.MobilePhoneNo))
            //{
            //    var checkExitPhoneNumber = _userService.CheckExitPhoneNumber(request.MobilePhoneNo, request.Id);
            //    if (checkExitPhoneNumber)
            //    {
            //        return ActionResponse<string>.Failed("Số điện thoại người dùng đã tồn tại",
            //            nameof(request.MobilePhoneNo));
            //    }
            //}

            //if (!string.IsNullOrEmpty(request.Email))
            //{
            //    var checkExitEmail = _userService.CheckExitEmail(request.Email, request.Id);
            //    if (checkExitEmail)
            //    {
            //        return ActionResponse<string>.Failed("Email người dùng đã tồn tại ", nameof(request.Email));
            //    }
            //}

            if (!string.IsNullOrEmpty(request.IdNo))
            {
                var checkExitIdNumber = _userService.CheckExitIdNumber(request.IdNo, request.Id);
                if (!checkExitIdNumber)
                {
                    return ActionResponse<string>.Failed("Số CMT/Thẻ căn cước người dùng đã tồn tại",
                        nameof(request.IdNo));
                }
            }

            var checkExitTaxIdNo = _userService.CheckExitIdTaxNumber(request.TaxIdNo, request.Id);
            if (!checkExitTaxIdNo)
            {
                return ActionResponse<string>.Failed("Mã số thuế đã tồn tại",
                    nameof(request.TaxIdNo));
            }

            var checkExitCustomer = _userService.CheckExitCustomer(request.ApplicationUserIdentityGuid, request.Id);
            if (!checkExitCustomer)
            {
                var user = await _userRepository.FindByApplicationUserIdentityGuidAsync(request.ApplicationUserIdentityGuid);
                return ActionResponse<string>.Failed($"Khách hàng đã tồn tại ở đối tác {user.Code}",
                    nameof(request.ApplicationUserIdentityGuid));
            }

            request.FullName = string.IsNullOrWhiteSpace(request.FullName)
                ? $"{request.FirstName} {request.LastName}"
                : request.FullName;

            if (request.Avatar != null && !request.AvatarId.HasValue)
            {
                var storedAvatarItem =
                    await _staticResourceService.PersistentImage(request.Avatar.TemporaryUrl);
                var addUserAvatarResponse = await _pictureRepository.CreateAndSave(storedAvatarItem);
                if (!addUserAvatarResponse.IsSuccess)
                    throw new OrganizationDomainException(addUserAvatarResponse.Message);

                request.AvatarId = addUserAvatarResponse.Result.Id;
            }

            if (request.AvatarId.HasValue && request.Avatar != null && request.Avatar.IsUpdating)
            {
                var storedAvatarItem =
                    await _staticResourceService.PersistentImage(request.Avatar.TemporaryUrl);
                storedAvatarItem.Id = request.AvatarId.Value;
                var addUserAvatarResponse = await _pictureRepository.UpdateAndSave(storedAvatarItem);
                if (!addUserAvatarResponse.IsSuccess)
                    throw new OrganizationDomainException(addUserAvatarResponse.Message);
            }

            if (!request.IsCustomer)
            {
                request.ApplicationUserIdentityGuid = string.Empty;
            }

            var userEntity = await _userService.GetByIdAsync(request.Id);

            userEntity.Update(request);

            //if (!string.IsNullOrWhiteSpace(request.Password))
            //{
            //    result.CombineResponse(await _userService.UpdateAsync(userEntity, request.Password));
            //}
            //else
            //{
            result.CombineResponse(await _userService.UpdateWithoutPasswordAsync(userEntity));
            //}

            //Event
            var customer = new CustomerIntegrationEvent()
            {
                IdentityGuid = request.ApplicationUserIdentityGuid,
                HasUpdate = false
            };

            var partner = new PartnerIntegrationEvent()
            {
                IdentityGuid = userEntity.IdentityGuid,
                Address = userEntity.Address,
                Email = userEntity.Email,
                FaxNo = userEntity.FaxNo,
                ShortName = userEntity.ShortName,
                FullName = userEntity.FullName,
                IdNo = userEntity.IdNo,
                MobilePhoneNo = userEntity.MobilePhoneNo,
                TaxIdNo = userEntity.TaxIdNo,
                UserName = userEntity.UserName,
                HasUpdate = true
            };

            var updateContractorIntegrationEvent
                    = new UpdateContractorIntegrationEvent(customer, partner);
            await _integrationEventLogService.AddAndSaveEventAsync(updateContractorIntegrationEvent);


            result.SetResult(request.IdentityGuid);
            return result;
        }
    }
}