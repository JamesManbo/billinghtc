using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Commands.ContractFormCommand;
using ContractManagement.Domain.Models;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ContractFormMapperConfigs : Profile
    {
        public ContractFormMapperConfigs()
        {
            CreateMap<CUContractFormCommand, ContractForm>().ForMember(e => e.DigitalSignature, m => m.Ignore());
            CreateMap<ContractForm, ContractFormDTO>().ReverseMap();
            CreateMap<ContractFormGrpcDTO, ContractFormDTO>().ReverseMap();
            CreateMap<IPagedList<ContractFormDTO>, ContractFormPageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
        }
    }
}
