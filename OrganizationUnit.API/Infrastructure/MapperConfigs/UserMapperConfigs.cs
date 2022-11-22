using AutoMapper;
using Global.Models.Filter;
using OrganizationUnit.API.Application.Commands.User;
using OrganizationUnit.API.Protos.Users;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.Commands.User;
using OrganizationUnit.Domain.Models.FCM;
using OrganizationUnit.Domain.Models.User;

namespace OrganizationUnit.API.Infrastructure.MapperConfigs
{
    public class UserMapperConfigs : Profile
    {
        public UserMapperConfigs()
        {
            CreateMap<User, CreateUserCommand>()
                .ReverseMap()
                .ForMember(u => u.UserBankAccounts, opt => opt.Ignore());
            CreateMap<User, UpdateUserCommand>()
                .ReverseMap()
                .ForMember(u => u.UserBankAccounts, opt => opt.Ignore());
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, CreateUserCommand>().ReverseMap();
            CreateMap<UserDTO, UpdateUserCommand>().ReverseMap();
            CreateMap<FCMTokenDto, FcmTokenGrpc>().ReverseMap();
            CreateMap<UserBankAccount, CUUserBankAccountCommand>().ReverseMap();
            CreateMap<UserRequestFilterModel, UserRequestFilterModelGrpc>().ReverseMap();
            CreateMap<RequestFilterModel, UserRequestFilterModelGrpc>().ReverseMap();
            CreateMap<UserDTO, UserDtoGrpc>().ReverseMap();
        }
    }
}
