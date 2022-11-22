using AutoMapper;
using DebtManagement.Domain.Models;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.ReceiptVoucherModels;
using DebtManagement.API.Protos;
using Global.Models.PagedList;
using DebtManagement.Domain.Models.FilterModels;
using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Models.DebtModels.OutDebts;
using Global.Models.StateChangedResponse;
using Global.Models;
using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Commands.DebtCommand;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class ReceiptVoucherMapperConfigs : Profile
    {
        public ReceiptVoucherMapperConfigs()
        {
            CreateMap<UpdateReceiptVoucherCommand, ReceiptVoucher>();
            CreateMap<CreateReceiptVoucherLineTaxCommand, ReceiptVoucherLineTax>();
            CreateMap<CUReceiptVoucherDetailCommand, ReceiptVoucherDetail>();
            CreateMap<CuReceiptVoucherPaymentDetailCommand, ReceiptVoucherPaymentDetail>();

            CreateMap<VoucherTargetDTOGrpc, CUVoucherTargetCommand>();

            CreateMap<PaymentMethodGrpc, PaymentMethod>();
            CreateMap<OpeningDebtByReceiptVoucherGrpc, CuOpeningDebtPaymentCommand>();
            CreateMap<ReceiptVoucherPaymentDetailGrpc, CuReceiptVoucherPaymentDetailCommand>();
            CreateMap<ReceiptVoucherDTOGrpc, UpdateReceiptVoucherCommand>()
                .ForMember(d => d.Target, s => s.MapFrom(p => p.VoucherTarget));
            CreateMap<ReceiptVoucherDTOGrpc, CreateReceiptVoucherCommand>()
                .ForMember(d => d.Target, s => s.MapFrom(p => p.VoucherTarget));

            CreateMap<CUChannelPriceBusTableCommand, ChannelPriceBusTable>();
            CreateMap<CUBusTablePricingCalculatorCommand, BusTablePricingCalculator>();

            CreateMap<ChannelPriceBusTable, ChannelPriceBusTableDTO>();
            CreateMap<BusTablePricingCalculator, BusTablePricingCalculatorDTO>();

            CreateMap<ReceiptVoucher, ReceiptVoucherDTO>();
            CreateMap<ReceiptVoucher, ReceiptVoucherInsertBulkModel>();
            CreateMap<ReceiptVoucherLineTax, ReceiptVoucherLineTaxDTO>();
            CreateMap<ReceiptVoucherDetail, ReceiptVoucherDetailDTO>();

            CreateMap<ReceiptVoucherPaymentDetailDTO, ReceiptVoucherPaymentDetailGrpc>();
            CreateMap<OpeningDebtByReceiptVoucherModel, OpeningDebtByReceiptVoucherGrpc>();

            CreateMap<ReceiptVoucherDTO, ReceiptVoucherDTOGrpc>()
                .ForMember(d => d.VoucherTarget, m => m.MapFrom(e => e.Target));
            CreateMap<ReceiptVoucherDetailDTO, ReceiptVoucherDetailDTOGrpc>();
            CreateMap<VoucherTargetDTO, VoucherTargetDTOGrpc>();
            CreateMap<ReceiptVoucherGridDTO, ReceiptVoucherGridDTOGrpc>();
            CreateMap<ReceiptVoucherDetailGridDTO, ReceiptVoucherGridDetailDTOGrpc>();
            CreateMap<PaymentMethod, PaymentMethodGrpc>();
            CreateMap<ReceiptVoucherFilterModel, ReceiptVoucherFilterGrpc>().ReverseMap();
            CreateMap<ReceiptVoucherBothProjectNullFilterModel, ReceiptVoucherFilterGrpc>().ReverseMap();
            CreateMap<IPagedList<ReceiptVoucherGridDTO>, ReceiptVoucherPageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));


            CreateMap<CollectedVoucherFilter, CollectedVoucherFilterGrpc>().ReverseMap();
            CreateMap<CollectedVoucherDataDTOGrpc, CollectedVouchersDTO>().ReverseMap();

            CreateMap<ErrorGeneric, ErrorGrpc>();
            CreateMap<ActionResponse, ActionResponseGrpc>();

            CreateMap<ReceiptVoucherSharingRevenueDTO, ServiceSharingRevenue>().ReverseMap();
            CreateMap<ReceiptVoucherSharingRevenueDTO, ReceiptVoucherSharingRevenue>().ReverseMap();
        }
    }
}
