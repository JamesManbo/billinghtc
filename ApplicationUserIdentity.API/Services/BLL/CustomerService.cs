
using ApplicationUserIdentity.API.Infrastructure;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Models.CustomerViewModels;
using System;
using System.Threading.Tasks;
using AutoMapper;
using ApplicationUserIdentity.API.Infrastructure.Queries;
using Global.Models.PagedList;
using ApplicationUserIdentity.API.Protos;
using ApplicationUserIdentity.API.Infrastructure.Validations;
using System.Security.Cryptography;
using ApplicationUserIdentity.API.Infrastructure.Helper;
using ApplicationUserIdentity.API.Models.Configs;
using Microsoft.Extensions.Options;

namespace ApplicationUserIdentity.API.Services.BLL
{
    public interface ICustomerService
    {
        Task<CreateCustomerResponseGrpc> CreateCustomer(UserViewModel cCustomerCommand);
        Task<IPagedList<UserClassViewModel>> GetCustomerClass();
        Task<IPagedList<UserGroupDTO>> GetCustomerGroup();
        Task<IPagedList<CustomerCategoryDTO>> GetCustomerCategory();
        Task<IPagedList<CustomerTypeDTO>> GetCustomerType();
        Task<IPagedList<CustomerStructureDTO>> GetCustomerStructure();
        Task<IPagedList<IndustryDTO>> GetIndustries();
    }
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _accountRepository;
        private readonly IUserUserGroupRepository _groupRepository;
        private readonly ApplicationUserDbContext _dbContext;
        private readonly ICustomerCategoryQueries _customerCategoryQueries;
        private readonly ICustomerTypeQueries _customerTypeQueries;
        private readonly IUserClassQueries _userClassQueries;
        private readonly IUserGroupQueriesRepository _userGroupQueries;
        private readonly ICustomerStructureQueries _customerStructureQueries;
        private readonly IIndustryQueries _industryQueries;
        private readonly AppSettings _appSettings;

        public CustomerService(ApplicationUserDbContext dbContext, IUserUserGroupRepository groupRepository, IUserRepository accountRepository, IMapper mapper
            , ICustomerCategoryQueries customerCategoryQueries
            , ICustomerTypeQueries customerTypeQueries
            , IUserClassQueries userClassQueries
            , ICustomerStructureQueries customerStructureQueries
            , IIndustryQueries industryQueries
            , IOptions<AppSettings> appSettings
            , IUserGroupQueriesRepository userGroupQueries)
        { 
            _dbContext = dbContext;
            _groupRepository = groupRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _customerCategoryQueries = customerCategoryQueries;
            _customerTypeQueries = customerTypeQueries;
            _userClassQueries = userClassQueries;
            _userGroupQueries = userGroupQueries;
            _customerStructureQueries = customerStructureQueries;
            _industryQueries = industryQueries;
            _appSettings = appSettings.Value;
        }
        public async Task<CreateCustomerResponseGrpc> CreateCustomer(UserViewModel cCustomerCommand)
        {
            var rs = new CreateCustomerResponseGrpc();

            var validator = new ApplicationUserValidator(false);//isEnterpriseValidation
            var validateResult = await validator.ValidateAsync(cCustomerCommand);
            if (!validateResult.IsValid)
            {
                rs.IsSuccess = false;
                rs.Message = validateResult.Errors[0]?.ErrorMessage;
                return rs;
            }

            if (_accountRepository.CheckExitUserName(cCustomerCommand.UserName))
            {
                rs.IsSuccess = false;
                rs.Message = "Tên tài khoản đã tồn tại ";
                return rs;
            }
            if (_accountRepository.CheckExitMobile(cCustomerCommand.MobilePhoneNo))
            {

                rs.IsSuccess = false;
                rs.Message = "Số điện thoại liên hệ đã tồn tại ";
                return rs;
                //BadRequest(new ErrorGeneric("Số điện thoại liên hệ đã tồn tại ",
                //    nameof(cCustomerCommand.MobilePhoneNo)));
            }

            //if (_accountRepository.CheckExitFaxNo(cCustomerCommand.FaxNo))
            //{

            //    rs.IsSuccess = false;
            //    rs.Message = "Số Fax đã tồn tại ";
            //    return rs;
            //    //  return BadRequest(new ErrorGeneric("Số Fax đã tồn tại ", nameof(cCustomerCommand.FaxNo)));
            //}

            if (_accountRepository.CheckExitEmail(cCustomerCommand.Email))
            {

                rs.IsSuccess = false;
                rs.Message = "Địa chỉ email đã tồn tại ";
                return rs;
                // return BadRequest(new ErrorGeneric("Địa chỉ email đã tồn tại ", nameof(cCustomerCommand.Email)));
            }

            if (_accountRepository.CheckExitIdNo(cCustomerCommand.IdNo))
            {
                rs.IsSuccess = false;
                rs.Message = "CMT/ Hộ chiếu đã tồn tại ";
                return rs;
            }

            // Binding auto assign property

            cCustomerCommand.IdentityGuid = Guid.NewGuid().ToString();
            cCustomerCommand.SecurityStamp = Guid.NewGuid().ToString();
            cCustomerCommand.Code = cCustomerCommand.UserName;
            if (!string.IsNullOrWhiteSpace(cCustomerCommand.Password))
            {
                //Security information 
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                cCustomerCommand.PasswordSalt = Convert.ToBase64String(salt);
                var encryptedPassword =
                    $"{cCustomerCommand.PasswordSalt}{cCustomerCommand.Password}".EncryptMD5(_appSettings.MD5CryptoKey);
                cCustomerCommand.Password = encryptedPassword;
            }

            try
            {
                var dbTransaction = await _dbContext.BeginTransactionAsync();
                var actionResponse = await _accountRepository.CreateAndSave(cCustomerCommand);
                if (cCustomerCommand.GroupIds != null && cCustomerCommand.GroupIds.Count > 0)
                {
                    foreach (var group in cCustomerCommand.GroupIds)
                    {
                        var groupModel = new ApplicationUserUserGroup
                        {
                            UserId = actionResponse.Result.Id,
                            GroupId = group
                        };

                        await _groupRepository.CreateAndSave(groupModel);
                    }
                }
                await _dbContext.CommitTransactionAsync(dbTransaction);
                if (actionResponse.IsSuccess)
                {
                    rs.IsSuccess = true;
                    rs.CustomerModel = _mapper.Map<CustomerModelGrpc>(actionResponse.Result);
                   return rs;
                }

                rs.IsSuccess = false;
                rs.Message = "Có lỗi xảy ra, vui lòng liên hệ quản trị viên";
                return rs;
            }
            catch (Exception ex)
            {
                _dbContext.RollbackTransaction();
                rs.IsSuccess = false;
                rs.Message = "Có lỗi xảy ra, vui lòng liên hệ quản trị viên";
                return rs;
            }
            finally
            {
                _dbContext.Dispose();
            }

        }
        public async Task<IPagedList<UserClassViewModel>> GetCustomerClass()
        {
            return await Task.FromResult(_userClassQueries.GetList(new Global.Models.Filter.RequestFilterModel() { Paging = false }));
        }
        public async Task<IPagedList<UserGroupDTO>> GetCustomerGroup()
        {
            return await Task.FromResult(_userGroupQueries.GetList(new Global.Models.Filter.RequestFilterModel() { Paging = false }));
        }
        public async Task<IPagedList<CustomerCategoryDTO>> GetCustomerCategory()
        {
            return await Task.FromResult(_customerCategoryQueries.GetList(new Global.Models.Filter.RequestFilterModel() { Paging = false}));
        }
        public async Task<IPagedList<CustomerTypeDTO>> GetCustomerType()
        {
            return await Task.FromResult(_customerTypeQueries.GetList(new Global.Models.Filter.RequestFilterModel() { Paging = false }));
        }

        public async Task<IPagedList<CustomerStructureDTO>> GetCustomerStructure()
        {
            return await Task.FromResult(_customerStructureQueries.GetList(new Global.Models.Filter.RequestFilterModel() { Paging = false }));
        }

        public async Task<IPagedList<IndustryDTO>> GetIndustries()
        {
            return await Task.FromResult(_industryQueries.GetList(new Global.Models.Filter.RequestFilterModel() { Paging = false }));
        }
    }
}
