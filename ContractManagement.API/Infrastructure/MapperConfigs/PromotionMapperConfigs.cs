using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.Commands.PromotionCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.ReceiptVouchers;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class PromotionMapperConfigs : Profile
    {
        public PromotionMapperConfigs() 
        {
            CreateMap<Promotion, PromotionDTO>().ReverseMap();
            CreateMap<Promotion, CUPromotionCommand>().ReverseMap();
            CreateMap<CUPromotionCommand, PromotionDTO>().ReverseMap();
            CreateMap<PromotionDTO, Promotion>().ReverseMap();
            CreateMap<PromotionProductDTO, PromotionProduct>().ReverseMap();
            CreateMap<PromotionDetailDTO, PromotionDetail>().ReverseMap();
            CreateMap<AvailablePromotionDto, PromotionDTOGrpc>().ReverseMap();

            CreateMap<PromotionForContract, PromotionForReceiptVoucherDTO>().ReverseMap();
            CreateMap<PromotionForContract, PromotionForContractDTO>().ReverseMap();

        }
    }
}
