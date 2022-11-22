using AutoMapper;
using DebtManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.PaymentVoucherModels;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class PaymentVoucherMapperConfigs : Profile
    {
        public PaymentVoucherMapperConfigs()
        {
            CreateMap<PaymentVoucherPaymentDetail, PaymentVoucherPaymentDetailDTO>();
            CreateMap<PaymentVoucherLineTax, PaymentVoucherLineTaxDTO>();
            CreateMap<PaymentVoucherDetail, PaymentVoucherDetailDTO>();

            CreateMap<PaymentVoucher, PaymentVoucherDTO>().ReverseMap();
            CreateMap<UpdatePaymentVoucherCommand, PaymentVoucher>();
            CreateMap<CUPaymentVoucherDetailCommand, PaymentVoucherDetail>();
        }
    }
}
