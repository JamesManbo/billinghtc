
using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using Global.Models.Filter;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.CustomerModels;
using StaffApp.APIGateway.Models.RequestModels;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class CustomerMapperProfile : Profile
    {
        public CustomerMapperProfile()
        {
            CreateMap<CCustomerCommandGrpc, CreateCustomerCommandGrpc>().ReverseMap();
            CreateMap<CustomerModelGrpc, CustomerDTO>().ReverseMap();

            CreateMap<CustomerClassDtoGrpc, CustomerClassModelGrpc>().ReverseMap();
            CreateMap(typeof(CustomerClassPageListGrpc), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));

            CreateMap<CustomerGroupDtoGrpc, CustomerGroupModelGrpc>().ReverseMap();
            CreateMap(typeof(CustomerGroupPageListGrpc), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));

            CreateMap<CustomerCategoryDtoGrpc, CustomerCategoryModelGrpc>().ReverseMap();
            CreateMap(typeof(CustomerCategoryPageListGrpc), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));

            CreateMap<CustomerTypeDtoGrpc, CustomerTypeModelGrpc>().ReverseMap();
            CreateMap(typeof(CustomerTypePageListGrpc), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));

            CreateMap<CustomerStructDtoGrpc, CustomerStructModelGrpc>().ReverseMap();
            CreateMap(typeof(CustomerStructPageListGrpc), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
            
            CreateMap<IndustryDto, IndustryModelGrpc>().ReverseMap();
            CreateMap(typeof(IndustryPageListGrpc), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));

            CreateMap<CreateCustomerDTO, CreateCustomerResponseGrpc>().ReverseMap();

            CreateMap<RequestFilterModel, RequestFilterGrpcModel>().ReverseMap();
            CreateMap(typeof(CustomerPageListGrpc), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));
        }
    }
}
