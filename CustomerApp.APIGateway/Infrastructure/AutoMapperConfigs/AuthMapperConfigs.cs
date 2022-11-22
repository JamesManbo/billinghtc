using ApplicationUserIdentity.API.Proto;
using AutoMapper;
using CustomerApp.APIGateway.Models.AuthModels;
using CustomerApp.APIGateway.Models.FCMModels;
using CustomerApp.APIGateway.Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class AuthMapperConfigs : Profile
    {
        public AuthMapperConfigs()
        {
            CreateMap<LoginRequest, LoginRequestGrpc>().ReverseMap();
            CreateMap<LoginResultDTO, LoginResultGrpc>().ReverseMap();

            CreateMap<ChangePasswordRequest, ChangePasswordRequestGrpc>().ReverseMap();
            CreateMap<ChangePasswordResultDTO, ChangePasswordResultGrpc>().ReverseMap();
            CreateMap<ForgotPaswordResult, ForgotPasswordResultGrpc>().ReverseMap();
            CreateMap<ChangePasswordResultDTO, ChangePasswordResultGrpc>().ReverseMap();
            CreateMap<ChangePasswordByUserNameRequest, ChangePasswordGrpcCommand>().ReverseMap();

            CreateMap<RegisterFcmTokenCommand, RegisterFCMTokenCommandGrpc>().ReverseMap();
        }
    }
}
