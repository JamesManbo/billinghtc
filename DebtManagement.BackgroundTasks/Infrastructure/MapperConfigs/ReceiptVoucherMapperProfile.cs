using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.ContractModels;
using DebtManagement.Domain.Models.ReceiptVoucherModels;

namespace DebtManagement.BackgroundTasks.Infrastructure.MapperConfigs
{
    public class ReceiptVoucherMapperProfile : Profile
    {
        public ReceiptVoucherMapperProfile()
        {
            CreateMap<ReceiptVoucher, ReceiptVoucherInsertBulkModel>()
                .ForMember(s => s.Payment_Form, m => m.MapFrom(d => d.Payment.Form))
                .ForMember(s => s.Payment_Method, m => m.MapFrom(d => d.Payment.Method))
                .ForMember(s => s.Payment_Address, m => m.MapFrom(d => d.Payment.Address))
                .ForMember(s => s.Discount_Percent, m => m.MapFrom(d => d.Discount.Percent))
                .ForMember(s => s.Discount_Amount, m => m.MapFrom(d => d.Discount.Amount))
                .ForMember(s => s.Discount_Type, m => m.MapFrom(d => d.Discount.Type));

            CreateMap<ReceiptVoucherDetail, ReceiptVoucherDetailInsertBulkModel>();
            CreateMap<ReceiptVoucherLineTax, ReceiptVoucherTaxInsertBulkModel>()
                .ForMember(s => s.VoucherLineId, m => m.MapFrom(d => d.VoucherLineId == 0 ? null : d.VoucherLineId));

            CreateMap<ReceiptVoucherDebtHistory, DebtHistoryInsertBulkModel>()
                .ForMember(s => s.ReceiptVoucherId, m => m.MapFrom(d => d.ReceiptVoucherId == 0 ? null : d.ReceiptVoucherId));

            CreateMap<PromotionForContractDTO, PromotionForReceiptVoucher>();
        }
    }
}
