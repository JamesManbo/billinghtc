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

namespace OrganizationUnit.API.Application.Commands.User
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ActionResponse<string>>
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IStaticResourceService _staticResourceService;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IPictureRepository _pictureRepository;
        private readonly IUserIntegrationEventService _integrationEventLogService;

        public CreateUserCommandHandler(IUserService userService, IStaticResourceService staticResourceService,
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

        public async Task<ActionResponse<string>> Handle(CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var result = new ActionResponse<string>();
            //if(!string.IsNullOrEmpty(request.MobilePhoneNo))
            //{
            //    var checkExitPhoneNumber = _userService.CheckExitPhoneNumber(request.MobilePhoneNo, 0);
            //    if (checkExitPhoneNumber)
            //    {
            //        return ActionResponse<string>.Failed("Số điện thoại người dùng đã tồn tại",
            //            nameof(request.MobilePhoneNo));
            //    }
            //}

            //if(!string.IsNullOrEmpty(request.Email))
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

            //var checkExitIdNumber = _userService.CheckExitIdNumber(request.IdNo, request.Id);
            //if (!checkExitIdNumber)
            //{
            //    return ActionResponse<string>.Failed("Số CMT/Thẻ căn cước người dùng đã tồn tại",
            //        nameof(request.IdNo));
            //}

            if (!string.IsNullOrEmpty(request.TaxIdNo))
            {
                var checkExitTaxIdNo = _userService.CheckExitIdTaxNumber(request.TaxIdNo, request.Id);
                if (!checkExitTaxIdNo)
                {
                    return ActionResponse<string>.Failed("Mã số thuế đã tồn tại",
                        nameof(request.TaxIdNo));
                }
            }
            //var checkExitTaxIdNo = _userService.CheckExitIdTaxNumber(request.TaxIdNo, request.Id);
            //if (!checkExitTaxIdNo)
            //{
            //    return ActionResponse<string>.Failed("Mã số thuế đã tồn tại",
            //        nameof(request.TaxIdNo));
            //}

            var checkExistedCode = _userService.CheckExistedCode(request.Code, request.Id);
            if (!checkExistedCode)
            {
                return ActionResponse<string>.Failed("Mã đã tồn tại",
                    nameof(request.Code));
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

            if (request.Id == 0)
            {
                if (request.Avatar != null)
                {
                    var storedAvatarItem =
                        await _staticResourceService.PersistentImage(request.Avatar.TemporaryUrl);
                    var addUserAvatarResponse = await _pictureRepository.CreateAndSave(storedAvatarItem);
                    if (!addUserAvatarResponse.IsSuccess)
                        throw new OrganizationDomainException(addUserAvatarResponse.Message);

                    request.AvatarId = addUserAvatarResponse.Result.Id;
                }

                request.IdentityGuid = Guid.NewGuid().ToString();

                var userEntity =
                    request.MapTo<Domain.AggregateModels.UserAggregate.User>(_configAndMapper.MapperConfig);

                if (request.UserBankAccounts != null)
                {
                    foreach (var userBankAccount in request.UserBankAccounts)
                    {
                        userEntity.AddUserBankAccounts(userBankAccount);
                    }
                }

                if (request.UserContactInfos != null)
                {
                    foreach (var userContactInfo in request.UserContactInfos)
                    {
                        userContactInfo.UserId = userEntity.Id;
                        userEntity.AddUserContactInfos(userContactInfo);
                    }
                }

                if (request.OrganizationUnitIds?.Length > 0)
                {
                    foreach (var orgId in request.OrganizationUnitIds)
                    {
                        userEntity.AddToOrganizaton(orgId);
                    }
                }

                result.CombineResponse(await _userService.CreateAsync(userEntity, userEntity.Password));
                result.SetResult(request.IdentityGuid);

                //Event
                var customer = new CustomerIntegrationEvent()
                {
                    IdentityGuid = userEntity.ApplicationUserIdentityGuid,
                    HasUpdate = false
                };

                var partner = new PartnerIntegrationEvent()
                {
                    IdentityGuid = userEntity.IdentityGuid,
                    Address = userEntity.Address,
                    Email = userEntity.Email,
                    FaxNo = userEntity.FaxNo,
                    FullName = userEntity.FullName,
                    ShortName = userEntity.ShortName,
                    IdNo = userEntity.IdNo,
                    MobilePhoneNo = userEntity.MobilePhoneNo,
                    TaxIdNo = userEntity.TaxIdNo,
                    UserName = userEntity.UserName,
                    HasUpdate = true
                };

                var updateContractorIntegrationEvent
                    = new UpdateContractorIntegrationEvent(customer, partner);
                await _integrationEventLogService.AddAndSaveEventAsync(updateContractorIntegrationEvent);

                return result;
            }

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

            result.CombineResponse(await _userService.UpdateWithoutPasswordAsync(request));
            result.SetResult(request.IdentityGuid);

            return result;
        }
    }
}