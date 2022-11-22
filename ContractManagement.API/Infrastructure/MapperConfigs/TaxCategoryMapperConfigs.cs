using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using ContractManagement.Domain.Commands.TaxCategoryCommand;
using ContractManagement.Domain.Models;
using Global.Models.PagedList;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class TaxCategoryMapperConfigs : Profile
    {
        public TaxCategoryMapperConfigs()
        {
            CreateMap<TaxCategory, TaxCategoryDTO>().ReverseMap();
            CreateMap<TaxCategory, CUTaxCategoryCommand>().ReverseMap();
            CreateMap<CUTaxCategoryCommand, TaxCategoryDTO>().ReverseMap();
            CreateMap<TaxCategoryDTO, TaxCategoryGrpcDTO>().ReverseMap();
            CreateMap<IPagedList<TaxCategoryDTO>, TaxCategoryPageListGrpcDTO>()
                .ForMember(s => s.Subset, m => m.MapFrom(e => e.Subset));
        }
    }
}
