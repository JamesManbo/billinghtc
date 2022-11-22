using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Protos;
using DebtManagement.Domain.Models.ContractModels;
using DebtManagement.Domain.Models.ReceiptVoucherModels;
using DebtManagement.Domain.Models.ReportModels;
using Google.Protobuf.Collections;

namespace DebtManagement.API.Infrastructure.MapperConfigs
{
    public class ServiceChannelMapperConfigs : Profile
    {
        public ServiceChannelMapperConfigs()
        {
            CreateMap<InstallationAddressGrpc, InstallationAddress>();
            CreateMap<ChannelAddressModelGrpc, ChannelAddressModel>();
        }
    }
}
