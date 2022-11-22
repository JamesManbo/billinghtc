using AutoMapper;
using DebtManagement.API.Protos;
using Global.Models.PagedList;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.CuReceiptVoucherCommands;
using StaffApp.APIGateway.Models.DebtModels;
using StaffApp.APIGateway.Models.ReceiptVoucherModels;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Models.TaxCategoryModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.AutoMapperConfigs
{
    public class ReceiptVoucherMapperProfile : Profile
    {
        public ReceiptVoucherMapperProfile()
        {
            CreateMap<VoucherTargetDTOGrpc, VoucherTargetDTO>();

            CreateMap<PaymentMethodGrpc, PaymentMethod>();

            CreateMap<TaxCategoryDTOGrpc, ReceiptVoucherTaxDTO>();

            CreateMap<ReceiptVoucherPaymentDetailGrpc, ReceiptVoucherPaymentDetailDTO>();
            CreateMap<OpeningDebtByReceiptVoucherGrpc, OpeningDebtByReceiptVoucherModel>();

            CreateMap<ReceiptVoucherDTOGrpc, ReceiptVoucherDTO>().ReverseMap();

            CreateMap<ReceiptVoucherDetailDTO, ReceiptVoucherDetailDTOGrpc>().ReverseMap();
            CreateMap<ReceiptVoucherGridDTO, ReceiptVoucherGridDTOGrpc>().ReverseMap();
            CreateMap<ReceiptVoucherDetailGridDTO, ReceiptVoucherGridDetailDTOGrpc>().ReverseMap();
            CreateMap<ReceiptVoucherFilterModel, ReceiptVoucherFilterGrpc>().ReverseMap();

            CreateMap<Money, decimal>().ConvertUsing(s => s == null ? 0m : Convert.ToDecimal(s.Value));
            CreateMap<decimal, Money>().ConvertUsing(s => new Money()
            {
                Value = s.ToString(CultureInfo.InvariantCulture),
                FormatValue = s == 0 ? "0" : string.Format("{0:#,0}", s),
                CurrencyCode = "VNĐ"
            });

            CreateMap<MoneyDTO, Money>().ReverseMap();

            CreateMap<PaymentMethod, PaymentMethodGrpc>().ReverseMap();

            CreateMap(typeof(ReceiptVoucherPageListGrpcDTO), typeof(IPagedList<>)).ConvertUsing(typeof(PageListMapperConverter<>));


            CreateMap<CollectedVoucherFilterGrpc, CollectedVoucherFilterModel>().ReverseMap();

            CreateMap<CollectedVoucherDataDTOGrpc, CollectedVoucherDTO>().ReverseMap();
            CreateMap<CollectedVoucherPageListGrpcDTO, List<CollectedVoucherDTO>>().ReverseMap(); ;


            CreateMap<VoucherTargetDTO, CUVoucherTargetCommand>();
            CreateMap<ReceiptVoucherTaxDTO, CreateReceiptVoucherTaxCommand>();
            CreateMap<ReceiptVoucherDetailDTO, CUReceiptVoucherDetailCommand>();

            CreateMap<ReceiptVoucherPaymentDetailDTO, CuReceiptVoucherPaymentDetailCommand>();
            CreateMap<OpeningDebtByReceiptVoucherModel, CuOpeningDebtPaymentCommand>();

            CreateMap<ReceiptVoucherDTO, CuReceiptVoucherCommand>()
                .ForMember(d => d.Target, s => s.MapFrom(d => d.VoucherTarget))
                .ForMember(d => d.ReceiptLines, s => s.MapFrom(d => d.ReceiptLines))
                .ForMember(d => d.ReceiptVoucherTaxes, s => s.MapFrom(d => d.ReceiptVoucherTaxes))
                .ForMember(d => d.OpeningDebtPayments, s => s.MapFrom(d => d.OpeningDebtPayments))
                .ForMember(d => d.IncurredDebtPayments, s => s.MapFrom(d => d.IncurredDebtPayments));

        }
    }
}
