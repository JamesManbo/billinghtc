using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models.Configs;
using ApplicationUserIdentity.API.Proto;
using ApplicationUserIdentity.API.Services.BLL;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Services.GRPC.Servers
{
    public class UserGrpcService : UserGrpc.UserGrpcBase
    {
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IUserQueries _accountQueries;

        private readonly IUserRepository _accountRepository;
        private readonly IMediator _mediator;

        public UserGrpcService(IMapper mapper, IOptions<AppSettings> appSettings, IUserQueries accountQueries,
            IUserRepository accountRepository,
            IMediator mediator)
        {
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _accountQueries = accountQueries;
            _accountRepository = accountRepository;
            _mediator = mediator;

        }

        public override Task<UserDTOGrpc> FindUserById(Int32Value request, ServerCallContext context)
        {
            var user = _accountQueries.Find(request.Value);
            return Task.FromResult(_mapper.Map<UserDTOGrpc>(user));
        }

        public override Task<UserDTOGrpc> FindUserByUId(StringValue request, ServerCallContext context)
        {
            var user = _accountQueries.Find(request.Value);
            return Task.FromResult(_mapper.Map<UserDTOGrpc>(user));
        }

        public override async Task<ChangeInfoResponseGrpc> ChangeInfoApplicationUser(ApplicationUserCommandGrpc request, ServerCallContext context)
        {
            var appUserEntity = await _accountRepository.GetByIdAsync(request.Id);

            if (string.IsNullOrEmpty(request.FullName))
            {
                return new ChangeInfoResponseGrpc { IsSuccess = false, Message = "Họ tên khách hàng là bắt buộc" };
            }

            if (string.IsNullOrEmpty(request.MobilePhoneNo))
            {
                return new ChangeInfoResponseGrpc { IsSuccess = false, Message = "Số điện thoại liên hệ là bắt buộc" };
            }
            if (request.MobilePhoneNo.Length!=10)
            {
                return new ChangeInfoResponseGrpc { IsSuccess = false, Message = "Số điện thoại liên hệ không hợp lệ" };
            }

            if (_accountRepository.CheckExitMobile(request.MobilePhoneNo, appUserEntity.Id))
            {
                return new ChangeInfoResponseGrpc { IsSuccess = false, Message = "Số điện thoại liên hệ đã tồn tại" };
            }
            
            if (string.IsNullOrEmpty(request.Address))
            {
                return new ChangeInfoResponseGrpc { IsSuccess = false, Message = "Địa chỉ là bắt buộc" };
            }



            appUserEntity.FullName = request.FullName;
            appUserEntity.MobilePhoneNo = request.MobilePhoneNo;
            appUserEntity.Address = request.Address;
            appUserEntity.Province = request.Province;
            appUserEntity.District = request.District;
            if (!string.IsNullOrEmpty(request.Email))
            {
                appUserEntity.Email = request.Email;
                if (_accountRepository.CheckExitEmail(request.Email, appUserEntity.Id))
                {
                    return new ChangeInfoResponseGrpc { IsSuccess = false, Message = "Địa chỉ mail đã tồn tại" };
                }
            }

            if (appUserEntity.CustomerCategoryId == 2)
            {
                if (string.IsNullOrEmpty(request.ProvinceIdentityGuid))
                {
                    return new ChangeInfoResponseGrpc { IsSuccess = false, Message = "Tỉnh/Thành Phố là bắt buộc" };
                }
                if (string.IsNullOrEmpty(request.DistrictIdentityGuid))
                {
                    return new ChangeInfoResponseGrpc { IsSuccess = false, Message = "Quận/Huyện là bắt buộc" };
                }
            }

            var actionResponse = await _accountRepository.UpdateAndSave(appUserEntity);

            return new ChangeInfoResponseGrpc { IsSuccess = actionResponse.IsSuccess, Message = actionResponse.Message };

        }
    }
}
