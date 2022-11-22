using AutoMapper;
using ContractManagement.API.Application.IntegrationEvents.Events;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.ApplicationUsers;
using ContractManagement.Domain.Models.Organizations;
using ContractManagement.Domain.Models.OutContracts;
using ContractManagement.Infrastructure.Queries;
using Global.Models.PagedList;
using OrganizationUnit.API.Protos.Organizations;
using System.Collections.Generic;
using System.Linq;

namespace ContractManagement.API.Infrastructure.MapperConfigs
{
    public class OutContractMapperConfigs : Profile
    {
        public OutContractMapperConfigs()
        {
            CreateMap<Contractor, ContractorDTO>();
            CreateMap<ImportContractor, ContractorDTO>();

            //TODO Outlet channel point logic changes
            CreateMap<OutContract, OutContractDTO>();

            CreateMap<OutContract, AddedNewServicePackageIntegrationEvent>()
                .ForMember(d => d.Id, s => s.Ignore())
                .ForMember(s => s.OutContractId, d => d.MapFrom(e => e.Id));

            CreateMap<CUContractorCommand, Contractor>();
            CreateMap<CUContractorHTCCommand, Contractor>();
            //CreateMap<CreateContractCommand, CreateContractGrpcCommand>().ReverseMap();
            CreateMap<CreateContractCommand, OutContract>();
            //GRPC
            CreateMap<PaymentMethod, PaymentMethodGrpc>().ReverseMap();
            CreateMap<ContractTimeLine, ContractTimeLineGrpc>().ReverseMap();
            CreateMap<Address, AddressGrpc>().ReverseMap();
            CreateMap<InstallationAddress, InstallationAddressGrpc>().ReverseMap();
            CreateMap<BillingTimeLine, BillingTimeLineGrpc>().ReverseMap();
            CreateMap<ContractorDTO, ContractorGrpcDTO>().ForMember(s => s.ContractCodes, m => m.MapFrom(d => d.ContractCodes));
            CreateMap<AttachmentFileDTO, AttachmentFileGrpcDTO>().ReverseMap();
            CreateMap<ContractOfTaxDTO, ContractOfTaxGrpcDTO>().ReverseMap();

            CreateMap<ApplicationUserDTO, CUContractorCommand>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.ContractorShortName, m => m.MapFrom(e => e.ShortName))
                .ForMember(d => d.ContractorCode, m => m.MapFrom(e => e.CustomerCode))
                .ForMember(d => d.ContractorUserName, m => m.MapFrom(e => e.UserName))
                .ForMember(d => d.ContractorFullName, m => m.MapFrom(e => e.FullName))
                .ForMember(d => d.ContractorPhone, m => m.MapFrom(e => e.MobilePhoneNo))
                .ForMember(d => d.ContractorEmail, m => m.MapFrom(e => e.Email))
                .ForMember(d => d.ContractorFax, m => m.MapFrom(e => e.FaxNo))
                .ForMember(d => d.ContractorIdNo, m => m.MapFrom(e => e.IdNo))
                .ForMember(d => d.ContractorTaxIdNo, m => m.MapFrom(e => e.TaxIdNo))
                .ForMember(d => d.IsEnterprise, m => m.MapFrom(e => e.IsEnterprise))
                .ForMember(d => d.ContractorCity, m => m.MapFrom(e => e.Province))
                .ForMember(d => d.ContractorCityId, m => m.MapFrom(e => e.ProvinceIdentityGuid))
                .ForMember(d => d.ContractorDistrict, m => m.MapFrom(e => e.District))
                .ForMember(d => d.ContractorDistrictId, m => m.MapFrom(e => e.DistrictIdentityGuid))
                .ForMember(d => d.ContractorAddress, m => m.MapFrom(e => e.Address))
                .ForMember(d => d.IsPartner, m => m.MapFrom(e => e.IsPartner))
                .ForMember(d => d.ApplicationUserIdentityGuid, m => m.MapFrom(e => e.IdentityGuid));

            CreateMap<UserDTO, CUContractorCommand>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.ContractorShortName, m => m.MapFrom(e => e.ShortName))
                .ForMember(d => d.ContractorCode, m => m.MapFrom(e => e.AccountingCustomerCode))
                .ForMember(d => d.ContractorUserName, m => m.MapFrom(e => e.UserName))
                .ForMember(d => d.ContractorFullName, m => m.MapFrom(e => e.FullName))
                .ForMember(d => d.ContractorPhone, m => m.MapFrom(e => e.MobilePhoneNo))
                .ForMember(d => d.ContractorEmail, m => m.MapFrom(e => e.Email))
                .ForMember(d => d.ContractorFax, m => m.MapFrom(e => e.FaxNo))
                .ForMember(d => d.ContractorIdNo, m => m.MapFrom(e => e.IdNo))
                .ForMember(d => d.ContractorTaxIdNo, m => m.MapFrom(e => e.TaxIdNo))
                .ForMember(d => d.IsEnterprise, m => m.MapFrom(e => e.IsEnterprise))
                .ForMember(d => d.ContractorAddress, m => m.MapFrom(e => e.Address))
                .ForMember(d => d.ApplicationUserIdentityGuid, m => m.MapFrom(e => e.ApplicationUserIdentityGuid))
                .ForMember(d => d.UserIdentityGuid, m => m.MapFrom(e => e.IdentityGuid));

            CreateMap<OutputChannelFilterModel, OutputChannelPointRequestGrpc>().ReverseMap();
            CreateMap<ContractEquipment, ContractEquipmentDTO>().ReverseMap();
            CreateMap<ContractEquipmentDTO, OutContractEquipmentGrpcDTO>().ReverseMap();
            CreateMap<OutContractDTO, ContractGrpcDTO>();
            CreateMap<InstallationAddress, AddressGrpc>().ReverseMap();

            CreateMap<ContractorByProjectIdsFilterModel, RequestGetContractorsByProjectIdsGrpc>().ReverseMap();
            CreateMap<ContractorByMarketAreaIdsProjectIdsFilterModel, RequestGetByMarketAreaIdsProjectIdsGrpc>().ReverseMap();
            CreateMap<IPagedList<ContractorDTO>, ContractorPageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));

            CreateMap<RequestGetContractsGrpc, ContactsFilterModel>().ReverseMap();
            CreateMap<IPagedList<OutContractGridDTO>, ContractPageListGrpcDTO>().ForMember(s => s.Subset, m => m.MapFrom(d => d.Subset));
            CreateMap<ContractorDTO, ContractorGrpcDTO>().ReverseMap();
            CreateMap<OutContractGridDTO, ContractGrpcDTO>()
                .ForMember(d => d.ServicePackages, m => m.MapFrom(e => e.ServicePackages))
                .ReverseMap();

            CreateMap<OutContractSimpleDTO, OutContractSimpleGrpcDTO>().ReverseMap();

            CreateMap<OutputChannelPoint, ImportOutputChannelPoint>()
                .ForMember(
                    d => d.InstallationAddress_City, m => m.MapFrom(e => e.InstallationAddress.City)
                ).ForMember(
                    d => d.InstallationAddress_CityId, m => m.MapFrom(e => e.InstallationAddress.CityId)
                ).ForMember(
                    d => d.InstallationAddress_District, m => m.MapFrom(e => e.InstallationAddress.District)
                ).ForMember(
                    d => d.InstallationAddress_DistrictId, m => m.MapFrom(e => e.InstallationAddress.DistrictId)
                ).ForMember(
                    d => d.InstallationAddress_Street, m => m.MapFrom(e => e.InstallationAddress.Street)
                ).ForMember(
                    d => d.InstallationAddress_Building, m => m.MapFrom(e => e.InstallationAddress.Building)
                ).ForMember(
                    d => d.InstallationAddress_Floor, m => m.MapFrom(e => e.InstallationAddress.Floor)
                ).ForMember(
                    d => d.InstallationAddress_RoomNumber, m => m.MapFrom(e => e.InstallationAddress.RoomNumber)
                );

            CreateMap<OutContract, ImportOutContract>()
                .ForMember(d => d.Payment_Form, m => m.MapFrom(s => s.Payment.Form))
                .ForMember(d => d.Payment_Method, m => m.MapFrom(s => s.Payment.Method))
                .ForMember(d => d.Payment_Address, m => m.MapFrom(s => s.Payment.Address))
                .ForMember(d => d.TimeLine_RenewPeriod, m => m.MapFrom(s => s.TimeLine.RenewPeriod))
                .ForMember(d => d.TimeLine_PaymentPeriod, m => m.MapFrom(s => s.TimeLine.PaymentPeriod))
                .ForMember(d => d.TimeLine_Expiration, m => m.MapFrom(s => s.TimeLine.Expiration))
                .ForMember(d => d.TimeLine_Liquidation, m => m.MapFrom(s => s.TimeLine.Liquidation))
                .ForMember(d => d.TimeLine_Effective, m => m.MapFrom(s => s.TimeLine.Effective))
                .ForMember(d => d.TimeLine_Signed, m => m.MapFrom(s => s.TimeLine.Signed))

                .ForMember(d => d.ServicePackages, m => m.MapFrom(e => e.ServicePackages));
            //.ForMember(d => d.OutContractOfTaxes, m => m.MapFrom(e => e.OutContractOfTaxes));

            CreateMap<ContractTotalByCurrency, ImportContractTotalByCurrency>();

            CreateMap<List<ContractorDTO>, ListContractorGrpcDTO>()
                .ForMember(d => d.Contractors, s => s.MapFrom(m => m));

            CreateMap<ContractTotalByCurrency, ContractTotalByCurrencyDTO>();
            CreateMap<OrganizationUnitGrpcDTO, OrganizationUnitDTO>();
            CreateMap<ContractContentDTO, ContractContentDTOGrpc>().ReverseMap();
            CreateMap<ContractEquipment, ImportContractEquipment>();
        }
    }
}