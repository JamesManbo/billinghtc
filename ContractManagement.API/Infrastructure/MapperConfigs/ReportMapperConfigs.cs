using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.FilterModels.ReportsModel;
using ContractManagement.Domain.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class ReportMapperConfigs : Profile
    {
        public ReportMapperConfigs()
        {
            CreateMap<OutContractInfoForReport, ReportMasterCustomerNationwideBusinessDTO>().ReverseMap();

            CreateMap<InContractInfoForReport, ReportMasterCustomerNationwideBusinessDTO>().ReverseMap();

            CreateMap<ServicePackageForMasterReport, ReportMasterCustomerNationwideBusinessDTO>().ReverseMap();


            CreateMap<ReportEquipmentInProjectRaw, EquipmentReportDTO>().ReverseMap();
            CreateMap<ReportEquipmentInProjectRaw, ReportEquipmentInProjectDTO>().ReverseMap();

            CreateMap<ContractStatusReportFilter, ContractStatusReportFilterGrpc>().ReverseMap();
            CreateMap<ContractStatusReportModel, ContractStatusReportModelGrpc>().ReverseMap();
        }
    }
}
