using ApplicationUserIdentity.API.Proto;
using AutoMapper;
using CustomerApp.APIGateway.Models.CommonModels;
using CustomerApp.APIGateway.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class UserMapperConfigs : Profile
    {
        public UserMapperConfigs()
        {
            CreateMap<UserDTO, UserDTOGrpc>().ReverseMap();
            CreateMap<PictureDtoGrpc, PictureViewModel>().ReverseMap();
            CreateMap<AccountCommand, ApplicationUserCommandGrpc>().ReverseMap();
            CreateMap<ChangeInfoResponse, ChangeInfoResponseGrpc>().ReverseMap();
        }
    }
}
