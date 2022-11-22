using System;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Proto;
using Global.Models.PagedList;
using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using ApplicationUserIdentity.API.Models.ContractDomainModels;
using ApplicationUserIdentity.API.Infrastructure.Helper;
using System.Security.Cryptography;
using System.Collections.Generic;
using ContractManagement.API.Protos;
using Google.Protobuf.Collections;
using System.Linq;

namespace ApplicationUserIdentity.API.Infrastructure.MapperConfigs
{
    public class ApplicationUserMappingConfigs : Profile
    {
        public ApplicationUserMappingConfigs()
        {
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<UserViewModel, ApplicationUser>();
            CreateMap<ApplicationUserClass, UserClassViewModel>().ReverseMap();

            CreateMap<ApplicationUser, RegisterViewModel>().ReverseMap();
            CreateMap<UserViewModel, UserDTOGrpc>().ReverseMap();
            CreateMap<UserRequestFilterModel, UsersInGroupRequestFilterModelGrpc>().ReverseMap();

            CreateMap<UserRequestFilterModel, RequestFilterGrpcModel>().ReverseMap();
            CreateMap<UserViewModel, CustomerModelGrpc>().ReverseMap();
            CreateMap<IPagedList<UserViewModel>, CustomerPageListGrpc>()
                .ForMember(d => d.Subset, s => s.MapFrom(d => d.Subset));

            CreateMap<FCMToken, RegisterFCMTokenCommandGrpc>().ReverseMap();
            CreateMap<FCMTokenDto, FcmTokenGrpc>().ReverseMap();

            CreateMap<ContractorGrpcDTO, ContractorDTO>().ReverseMap();
            CreateMap<ContractorPageListGrpcDTO, IPagedList<ContractorDTO>>().ForMember(d => d.Subset, s => s.MapFrom(d => d.Subset));
            CreateMap<RequestGetByMarketAreaIdsProjectIds, RequestGetByMarketAreaIdsProjectIdsGrpc>().ReverseMap();

            CreateMap<ContractorDTO, ApplicationUser>()
                .ForMember(d => d.ClassId, s => s.MapFrom(d => ApplicationUserClass.Copper.Id))
                .ForMember(d => d.IdentityGuid, s => s.MapFrom(d => d.ApplicationUserIdentityGuid))
                .ForMember(d => d.AccountingCustomerCode, s => s.MapFrom(m => m.AccountingCustomerCode))
                .ForMember(d => d.CustomerCode, s => s.MapFrom(d => d.ContractorCode))
                .ForMember(d => d.UserName, s => s.MapFrom(d => d.ContractorCode))
                .ForMember(d => d.Password, s => s.MapFrom(d => d.ContractorCode))
                .ForMember(d => d.ShortName, s => s.MapFrom(d => d.ContractorShortName))
                .ForMember(d => d.FirstName, s => s.MapFrom(d => d.ContractorFullName.GetFirstName()))
                .ForMember(d => d.LastName, s => s.MapFrom(d => d.ContractorFullName.GetLastName()))
                .ForMember(d => d.FullName, s => s.MapFrom(d => d.ContractorFullName))
                .ForMember(d => d.Gender, s => s.Ignore())
                .ForMember(d => d.MobilePhoneNo, s => s.MapFrom(d => d.ContractorPhone))
                .ForMember(d => d.FaxNo, s => s.MapFrom(d => d.ContractorFax))
                .ForMember(d => d.PasswordSalt, s => s.MapFrom(d => this.GeneratePasswordSalt()))
                .ForMember(d => d.SecurityStamp, s => s.MapFrom(d => Guid.NewGuid().ToString()))
                .ForMember(d => d.DateOfBirth, s => s.Ignore())
                .ForMember(d => d.Email, s => s.MapFrom(d => d.ContractorEmail))
                .ForMember(d => d.IdNo, s => s.MapFrom(d => d.ContractorIdNo))
                .ForMember(d => d.IdDateOfIssue, s => s.Ignore())
                .ForMember(d => d.IdIssuedBy, s => s.Ignore())
                .ForMember(d => d.TaxIdNo, s => s.MapFrom(d => d.ContractorTaxIdNo))
                .ForMember(d => d.RepresentativePersonName, s => s.MapFrom(d => d.Representative))
                .ForMember(d => d.RpPhoneNo, s => s.MapFrom(d => d.ContractorPhone))
                .ForMember(d => d.RpDateOfBirth, s => s.Ignore())
                .ForMember(d => d.RpJobPosition, s => s.Ignore())
                .ForMember(d => d.BusinessRegCertificate, s => s.Ignore())
                .ForMember(d => d.BrcDateOfIssue, s => s.Ignore())
                .ForMember(d => d.BrcIssuedBy, s => s.Ignore())
                .ForMember(d => d.Address, s => s.MapFrom(d => d.ContractorAddress))
                .ForMember(d => d.Ward, s => s.Ignore())
                .ForMember(d => d.WardIdentityGuid, s => s.Ignore())
                .ForMember(d => d.District, s => s.MapFrom(d => d.ContractorDistrict))
                .ForMember(d => d.DistrictIdentityGuid, s => s.MapFrom(d => d.ContractorDistrictId))
                .ForMember(d => d.Province, s => s.MapFrom(d => d.ContractorCity))
                .ForMember(d => d.ProvinceIdentityGuid, s => s.MapFrom(d => d.ContractorCityId))
                .ForMember(d => d.Country, s => s.MapFrom(d => "Việt Nam"))
                .ForMember(d => d.CountryIdentityGuid, s => s.MapFrom(d => "9999999999"))
                .ForMember(d => d.CustomerCategoryId, s => s.Ignore())
                .ForMember(d => d.CustomerTypeId, s => s.Ignore())
                .ForMember(d => d.CustomerStructureId, s => s.Ignore())
                .ForMember(d => d.BankName, s => s.Ignore())
                .ForMember(d => d.BankAccountNumber, s => s.Ignore())
                .ForMember(d => d.BankBranch, s => s.Ignore())
                .ForMember(d => d.IsEnterprise, s => s.MapFrom(d => d.IsEnterprise))
                .ForMember(d => d.IsEmailCertificated, s => s.MapFrom(d => false))
                .ForMember(d => d.IsPhoneNoCertificated, s => s.MapFrom(d => false))
                .ForMember(d => d.IsLocked, s => s.MapFrom(d => false))
                .ForMember(d => d.CustomerReviews, s => s.Ignore())
                .ForMember(d => d.IsPartner, s => s.MapFrom(d => d.IsPartner))
                .ForMember(d => d.UserIdentityGuid, s => s.Ignore());
        }
        private string GeneratePasswordSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }
        private class RepeatedFieldToListTypeConverter<TITemSource, TITemDest> : ITypeConverter<RepeatedField<TITemSource>, IEnumerable<TITemDest>>
        {
            public IEnumerable<TITemDest> Convert(RepeatedField<TITemSource> source, IEnumerable<TITemDest> destination, ResolutionContext context)
            {
                var result = destination?.ToList() ?? new List<TITemDest>();
                foreach (var item in source)
                {
                    result.Add(context.Mapper.Map<TITemDest>(item));
                }
                return result;
            }
        }
    }
}