using System;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Infrastructure.Helper;
using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.Configs;
using ApplicationUserIdentity.API.Models.Otp;
using ApplicationUserIdentity.API.Proto;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ApplicationUserIdentity.API.Services.GRPC.Servers
{
    public class AuthenticationGrpcService : AuthenticationGrpc.AuthenticationGrpcBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IUserQueries _userQueries;
        private readonly IFCMTokenQueries _fcmTokenQueries;
        private readonly IFCMTokenRepository _fcmTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly IOtpRepository _otpRepository;

        public AuthenticationGrpcService(IUserRepository userRepository,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IUserQueries userQueries,
            IFCMTokenQueries fcmTokenQueries,
            IConfiguration configuration,
            IOtpRepository otpRepository,
            IFCMTokenRepository fcmTokenRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _userQueries = userQueries;
            _fcmTokenQueries = fcmTokenQueries;
            _fcmTokenRepository = fcmTokenRepository;
            _otpRepository = otpRepository;
            _configuration = configuration;
        }

        public async override Task<LoginResultGrpc> Login(LoginRequestGrpc request, ServerCallContext context)
        {
            var user = await _userRepository.FindByUserNameAsync(request.UserName);
            var rs = new LoginResultGrpc();
            if (user == null)
            {
                rs.Message = "Tài khoản không tồn tại.";
                return rs;
            }
            var encryptedPassword = $"{user.PasswordSalt}{request.Password}".EncryptMD5(_appSettings.MD5CryptoKey);
            if (encryptedPassword != user.Password)
            {
                rs.Message = "Mật khẩu không đúng, vui lòng thử lại.";
                return rs;
            }

            rs.Succeeded = true;
            rs.Token = _userRepository.GenerateJwtToken(user);

            return rs;
        }

        public async override Task<ChangePasswordResultGrpc> ChangePassword(ChangePasswordRequestGrpc request, ServerCallContext context)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            var rs = new ChangePasswordResultGrpc();
            if (user == null)
            {
                rs.Message = "Tài khoản không tồn tại.";
                return rs;
            }

            var encryptedOldPassword = $"{user.PasswordSalt}{request.OldPassword}".EncryptMD5(_appSettings.MD5CryptoKey);
            if (encryptedOldPassword != user.Password)
            {
                rs.Message = "Mật khẩu cũ không đúng.";
                return rs;
            }

            var encryptedNewPassword = $"{user.PasswordSalt}{request.NewPassword}".EncryptMD5(_appSettings.MD5CryptoKey);
            user.Password = encryptedNewPassword;
            var actionResponse = await _userRepository.UpdateAndSave(user);

            rs.Succeeded = true;
            rs.Message = "Đổi mật khẩu thành công";
            return rs;
        }

        public override async Task<ChangePasswordResultGrpc> ChangePasswordWithoutPass(ChangePasswordGrpcCommand request, ServerCallContext context)
        {
            var user = await _userRepository.FindByUserNameAsync(request.UserName);

            if (user == null)
            {
                return new ChangePasswordResultGrpc() { Message = "Tài khoản không tồn tại.", Succeeded = false };
            }

            var encryptedNewPassword = $"{user.PasswordSalt}{request.NewPassword}".EncryptMD5(_appSettings.MD5CryptoKey);
            user.Password = encryptedNewPassword;
            var actionResponse = await _userRepository.UpdateAndSave(user);

            return new ChangePasswordResultGrpc() { Message = "Đổi mật khẩu thành công.", Succeeded = true };
        }

        public override async Task<ForgotPasswordResultGrpc> ForgotPassword(StringValue request, ServerCallContext context)
        {
            var account =  _userQueries.FindByUserName(request.Value);

            if (account == null)
                return new ForgotPasswordResultGrpc { Succeeded = false, Message = "Tài khoản không tồn tại" };

            if (string.IsNullOrEmpty(account.MobilePhoneNo))
                return new ForgotPasswordResultGrpc { Succeeded = false, Message = "Tài khoản chưa đăng ký số điện thoại" };

            var otp = OtpHelper.GenOTP(6);
            var currentDate = DateTime.Now;
            var otpExpiredTime = _configuration.GetValue<int>("OtpExpiredSeconds");
            var obj = new OtpEntity()
            {
                CreatedDate = currentDate,
                ExpiredDate = currentDate.AddSeconds(otpExpiredTime),
                Otp = otp,
                Phone = account.MobilePhoneNo
            };
            var createOtpRs = await _otpRepository.CreateAndSave(obj);
            if (createOtpRs.IsSuccess)
            {
                //var sendOtpRs = await _notificationGrpcService.SendSms(account.MobilePhoneNo, otp);
                return new ForgotPasswordResultGrpc { Succeeded = true, Message = String.Format("Otp đã được gửi tới số điện thoại {0}", account.MobilePhoneNo.Substring(account.MobilePhoneNo.Length - 3).PadLeft(account.MobilePhoneNo.Length, '*')), DateExpired = _mapper.Map<Timestamp>(obj.ExpiredDate) };
            }

            return new ForgotPasswordResultGrpc { Succeeded = false, Message = "Có lỗi" };
        }

        public override async Task<RegisterFCMTokenResponseGrpc> RegisterFCMToken(RegisterFCMTokenCommandGrpc request, ServerCallContext context)
        {
            var existRecord = _fcmTokenQueries.FindByReceiverIdAndFcmToken(request.ReceiverId, request.Token);
            if (existRecord == null)
            {
                await _fcmTokenRepository.CreateAndSave(_mapper.Map<FCMToken>(request));
            }
            return new RegisterFCMTokenResponseGrpc();
        }

        public override async Task<UnRegisterFCMTokenResponseGrpc> UnRegisterFCMToken(StringValue request, ServerCallContext context)
        {
            var existRecord = await _fcmTokenRepository.GetByToken(request.Value);

            if (existRecord != null)
            {
                await _fcmTokenRepository.RemoveAndSave(existRecord);
            }
            return new UnRegisterFCMTokenResponseGrpc();
        }
    }
}
