using System.Threading.Tasks;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Models.CustomerViewModels;
using ApplicationUserIdentity.API.Protos;
using AutoMapper;

namespace ApplicationUserIdentity.API.Infrastructure.MapperConfigs
{
    public class CustomerMappingConfigs : Profile
    {
        public CustomerMappingConfigs()
        {
          
            
            CreateMap<UserViewModel, CreateCustomerCommandGrpc>().ForMember(d => d.CustomerCategoryCode, s => s.MapFrom(d => d.CustomerCategoryName)).ReverseMap();
            CreateMap<UserViewModel, CustomerModelGrpc>().ReverseMap();

            CreateMap<CustomerModelGrpc, CustomerDTO>().ReverseMap();
            CreateMap<CustomerModelGrpc, ApplicationUser>().ReverseMap();
            CreateMap<Task<CustomerModelGrpc>, Task<CustomerDTO>>().ReverseMap();

        }
    }
}