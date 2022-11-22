using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DebtManagement.Domain.AggregatesModel.BaseVoucher;
using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Models;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class VoucherTargetMapperConfigs : Profile
    {
        public VoucherTargetMapperConfigs()
        {
            CreateMap<CUVoucherTargetCommand, VoucherTarget>();
            CreateMap<VoucherTarget, VoucherTargetDTO>();
        }
    }
}
