using AutoMapper;
using CustomerApp.APIGateway.Models.CommonModels;
using CustomerApp.APIGateway.Models.OutContract;
using CustomerApp.APIGateway.Models.ReceiptVoucherModels;
using CustomerApp.APIGateway.Models.RequestModels;
using DebtManagement.API.Protos;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ReceiptVoucherMapperProfile : Profile
    {
        public ReceiptVoucherMapperProfile()
        {
            CreateMap<ReceiptVoucherFilterModel, ReceiptVoucherFilterGrpc>().ReverseMap();

            CreateMap<ReceiptVoucherDetailDTO, ReceiptVoucherDetailDTOGrpc>().ReverseMap();
            CreateMap<MoneyDTO, Money>().ReverseMap();
            CreateMap<PaymentMethodDTO, PaymentMethodGrpc>().ReverseMap();

            CreateMap<TaxCategoryDTO, TaxCategoryDTOGrpc>().ReverseMap();

            CreateMap<ReceiptVoucherPaymentDetailDTO, ReceiptVoucherPaymentDetailGrpc>().ReverseMap();
            CreateMap<OpeningDebtByReceiptVoucherModel, OpeningDebtByReceiptVoucherGrpc>().ReverseMap();

            CreateMap<ReceiptVoucherDTO, ReceiptVoucherDTOGrpc>().ReverseMap();
            CreateMap<VoucherTargetDTO, VoucherTargetDTOGrpc>().ReverseMap();

            CreateMap<ReceiptVoucherGridDTO, ReceiptVoucherGridDTOGrpc>().ReverseMap();
            CreateMap<ReceiptVoucherDetailGridDTO, ReceiptVoucherGridDetailDTOGrpc>().ReverseMap();

            CreateMap(typeof(ReceiptVoucherPageListGrpcDTO), typeof(IPagedList<>))
                .ConvertUsing(typeof(PageListMapperConverter<>));
        }
    }
}
