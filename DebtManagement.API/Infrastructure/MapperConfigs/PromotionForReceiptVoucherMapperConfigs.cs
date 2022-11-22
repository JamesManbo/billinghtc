using AutoMapper;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class PromotionForReceiptVoucherMapperConfigs : Profile
    {
        public PromotionForReceiptVoucherMapperConfigs()
        {
            CreateMap<PromotionForReceiptVoucher, PromotionForReceiptVoucherCommand>().ReverseMap();
        }
    }
}
