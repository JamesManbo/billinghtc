using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemUserIdentity.API.Proto;
using AutoMapper;
using StaffApp.APIGateway.Models;
using StaffApp.APIGateway.Models.AuthModels;
using StaffApp.APIGateway.Models.FCMModels;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class AuthMapperProfile : Profile
    {
        public AuthMapperProfile()
        {
            CreateMap<UserGrpcDTO, UserDTO>();
            CreateMap<LoginRequest, LoginGrpcCommand>();
            CreateMap<SignInResultDTO, SignInResultGrpc>().ReverseMap();


            CreateMap<DraftChangePasswordRequest, ChangePasswordGrpcCommand>().ReverseMap();
            CreateMap<ChangePasswordResultGrpcDTO, ChangePasswordResult>().ReverseMap();
            CreateMap<ForgotPaswordResult, ForgotPasswordResultGrpc>().ReverseMap();
            CreateMap<DraftUserError, UserError>().ReverseMap();
            CreateMap<PictureDTO, PictureGrpcDto>().ReverseMap();

            CreateMap<RegisterFcmTokenCommand, RegisterFCMTokenCommandGrpc>().ReverseMap();
        }
    }
}
