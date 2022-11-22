using ApplicationUserIdentity.API.Infrastructure;
using ApplicationUserIdentity.API.Infrastructure.Helper;
using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Infrastructure.Validations;
using ApplicationUserIdentity.API.IntegrationEvents;
using ApplicationUserIdentity.API.IntegrationEvents.EventModels;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Models.Configs;
using ApplicationUserIdentity.API.Models.NotificationModels;
using ApplicationUserIdentity.API.Services.GRPC.Clients;
using ApplicationUserIdentity.API.Services.GRPC.StaticResource;
using Global.Models;
using Global.Models.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationUsersController : CustomBaseController
    {
        private readonly IUserQueries _accountQueries;
        private readonly IUserRepository _accountRepository;
        private readonly IUserUserGroupRepository _groupRepository;
        private readonly AppSettings _appSettings;
        private readonly IStaticResourceService _staticResourceService;
        private readonly IPictureRepository _pictureRepository;
        private readonly ApplicationUserDbContext _dbContext;
        private readonly ILogger<ApplicationUsersController> _logger;
        private readonly IApplicationUserIntegrationEventService _integrationEventLogService;
        private readonly IUserClassQueries _userClassQueries;
        private readonly IApplicationUserIndustryRepository _applicationUserIndustryRepository;
        private readonly IContractorGrpcService _contractorGrpcService;

        public ApplicationUsersController(IUserQueries accountQueries,
            IUserRepository accountRepository,
            IUserUserGroupRepository groupRepository,
            IOptions<AppSettings> appSettings,
            IStaticResourceService staticResourceService,
            IPictureRepository pictureRepository,
            ApplicationUserDbContext dbContext,
            ILogger<ApplicationUsersController> logger,
            IUserClassQueries userClassQueries,
            IApplicationUserIntegrationEventService integrationEventLogService,
            IContractorGrpcService contractorGrpcService,
            IApplicationUserIndustryRepository applicationUserIndustryRepository)
        {
            _accountQueries = accountQueries;
            _accountRepository = accountRepository;
            _groupRepository = groupRepository;
            _appSettings = appSettings.Value;
            _staticResourceService = staticResourceService;
            _pictureRepository = pictureRepository;
            _dbContext = dbContext;
            _logger = logger;
            _integrationEventLogService = integrationEventLogService;
            _userClassQueries = userClassQueries;
            _contractorGrpcService = contractorGrpcService;
            _applicationUserIndustryRepository = applicationUserIndustryRepository;
        }

        // GET: api/ApplicationUsers
        [HttpGet]
        public ActionResult<IEnumerable<UserViewSimpleModel>> GetApplicationUsers(
            [FromQuery] UserRequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_accountQueries.GetSelectionList(filterModel));
            }

            if (filterModel.Type == RequestType.Autocomplete)
            {
                return Ok(_accountQueries.Autocomplete(filterModel));
            }

            return Ok(_accountQueries.GetList(filterModel));
        }

        // GET: api/ApplicationUsers/5
        [HttpGet("{id}")]
        public IActionResult GetApplicationUser(int id)
        {
            var applicationUser = _accountQueries.Find((int)id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return Ok(applicationUser);
        }

        [HttpGet]
        [Route("GetListByMarketAreaIdsProjectIds")]
        public async Task<ActionResult<IEnumerable<UserViewSimpleModel>>> GetListByMarketAreaIdsProjectIdsAsync(
            [FromQuery] UserRequestFilterModel filterModel)
        {
            return Ok(await NotificationHelper.GetListApplicationUser(filterModel, _contractorGrpcService, _accountQueries));
        }

        // GET: api/ApplicationUsers/5
        [HttpGet("universal/{id}")]
        public IActionResult GetApplicationUserByUid(string id)
        {
            var applicationUser = _accountQueries.Find(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return Ok(applicationUser);
        }

        // PUT: api/ApplicationUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationUser(int id, UserViewModel applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return BadRequest();
            }

            var validator = new ApplicationUserValidator(applicationUser.IsEnterprise);
            var validateResult = await validator.ValidateAsync(applicationUser);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            if (!string.IsNullOrWhiteSpace(applicationUser.Password))
            {
                var encryptedPassword =
                    $"{applicationUser.PasswordSalt}{applicationUser.Password}".EncryptMD5(_appSettings.MD5CryptoKey);
                applicationUser.Password = encryptedPassword;
            }

            if (applicationUser.IsEnterprise)
            {
                //if (_accountRepository.CheckExitMobile(applicationUser.MobilePhoneNo, id))
                //{
                //    return BadRequest(new ErrorGeneric("Số điện thoại liên hệ đã tồn tại ",
                //        nameof(applicationUser.MobilePhoneNo)));
                //}

                if (_accountRepository.CheckExitFaxNo(applicationUser.FaxNo, id))
                {
                    return BadRequest(new ErrorGeneric("Số Fax đã tồn tại ", nameof(applicationUser.FaxNo)));
                }

                //if (_accountRepository.CheckExitEmail(applicationUser.Email, id))
                //{
                //    return BadRequest(new ErrorGeneric("Địa chỉ email đã tồn tại ", nameof(applicationUser.Email)));
                //}

                if (_accountRepository.CheckExitIdNo(applicationUser.TaxIdNo, id))
                {
                    return BadRequest(new ErrorGeneric("Mã số thuế đã tồn tại ", nameof(applicationUser.TaxIdNo)));
                }

                if (_accountRepository.CheckExitBusinessRegCertificate(applicationUser.BusinessRegCertificate, id))
                {
                    return BadRequest(new ErrorGeneric("Số đăng ký kinh doanh đã tồn tại ",
                        nameof(applicationUser.BusinessRegCertificate)));
                }

                if (_accountRepository.CheckExitPartner(applicationUser.UserIdentityGuid, id))
                {
                    var applicationUserEntity = await _accountRepository.FindByUserIdentityGuidAsync(applicationUser.UserIdentityGuid);
                    return BadRequest(new ErrorGeneric($"Đối tác đã tồn tại ở tài khoản {applicationUserEntity.UserName}", nameof(applicationUser.UserIdentityGuid)));
                }
            }

            if (!applicationUser.IsEnterprise)
            {
                if (_accountRepository.CheckExitUserName(applicationUser.UserName, id))
                {
                    return BadRequest(new ErrorGeneric("Tên tài khoản đã tồn tại ", nameof(applicationUser.UserName)));
                }

                //if (_accountRepository.CheckExitMobile(applicationUser.MobilePhoneNo, id))
                //{
                //    return BadRequest(new ErrorGeneric("Số điện thoại liên hệ đã tồn tại ",
                //        nameof(applicationUser.MobilePhoneNo)));
                //}

                //if (_accountRepository.CheckExitEmail(applicationUser.Email, id))
                //{
                //    return BadRequest(new ErrorGeneric("Địa chỉ mail đã tồn tại ", nameof(applicationUser.Email)));
                //}

                if (_accountRepository.CheckExitIdNo(applicationUser.IdNo, id))
                {
                    return BadRequest(new ErrorGeneric("CMT/ Hộ chiếu đã tồn tại ", nameof(applicationUser.IdNo)));
                }
            }

            try
            {
                var dbTransaction = await _dbContext.BeginTransactionAsync();
                if (applicationUser.Avatar == null)
                {
                    applicationUser.AvatarId = null;
                }
                else
                {
                    if (!applicationUser.AvatarId.HasValue)
                    {
                        var storedAvatarItem =
                            await _staticResourceService.PersistentImage(applicationUser.Avatar.TemporaryUrl);
                        var addUserAvatarResponse = _pictureRepository.Create(storedAvatarItem);
                        if (!addUserAvatarResponse.IsSuccess)
                        {
                            return BadRequest(addUserAvatarResponse);
                        }

                        applicationUser.AvatarId = addUserAvatarResponse.Result.Id;
                    }
                    else if (applicationUser.Avatar.IsUpdating)
                    {
                        var storedAvatarItem =
                            await _staticResourceService.PersistentImage(applicationUser.Avatar.TemporaryUrl);
                        storedAvatarItem.Id = applicationUser.AvatarId.Value;
                        var addUserAvatarResponse = await _pictureRepository.UpdateAndSave(storedAvatarItem);
                        if (!addUserAvatarResponse.IsSuccess)
                        {
                            return BadRequest(addUserAvatarResponse);
                        }
                    }
                }

                if (!applicationUser.IsPartner)
                {
                    applicationUser.UserIdentityGuid = string.Empty;
                }

                var appUserEntity = await _accountRepository.GetByIdAsync(id);

                applicationUser.CustomerCode = appUserEntity.CustomerCode;
                var actionResponse = await _accountRepository.UpdateAndSave(applicationUser);

                await _groupRepository.DeleteAllMapGroupUserByUserId(applicationUser.Id);
                if (applicationUser.GroupIds != null && applicationUser.GroupIds.Count > 0)
                {
                    foreach (var t in applicationUser.GroupIds)
                    {
                        var groupModel = new ApplicationUserUserGroup
                        {
                            UserId = applicationUser.Id,
                            GroupId = t
                        };

                        _groupRepository.Create(groupModel);
                    }
                }

                await _applicationUserIndustryRepository.DeleteAllMapIndustryByUserId(applicationUser.Id);
                if (applicationUser.IndustryIds != null && applicationUser.IndustryIds.Count > 0)
                {
                    foreach (var industryId in applicationUser.IndustryIds)
                    {
                        var industryModel = new ApplicationUserIndustry
                        {
                            UserId = applicationUser.Id,
                            IndustryId = industryId
                        };

                        _applicationUserIndustryRepository.Create(industryModel);
                    }
                }

                if (actionResponse.IsSuccess)
                {
                    // Event
                    var customer = new CustomerIntegrationEvent()
                    {
                        IdentityGuid = actionResponse.Result.IdentityGuid,
                        AccountingCustomerCode = actionResponse.Result.AccountingCustomerCode,
                        Address = actionResponse.Result.Address,
                        Email = actionResponse.Result.Email,
                        FaxNo = actionResponse.Result.FaxNo,
                        FullName = actionResponse.Result.FullName,
                        IdNo = actionResponse.Result.IdNo,
                        MobilePhoneNo = actionResponse.Result.MobilePhoneNo,
                        TaxIdNo = actionResponse.Result.TaxIdNo,
                        UserName = actionResponse.Result.UserName,
                        ShortName = actionResponse.Result.ShortName,
                        HasUpdate = true,
                        ContractorStructureId = applicationUser.CustomerStructureId,
                        ContractorCategoryId = applicationUser.CustomerCategoryId,
                        ContractorGroupIds = string.Join(',', applicationUser.GroupIds),
                        ContractorGroupNames = applicationUser.GroupNames != null
                            ? string.Join(",", applicationUser.GroupNames)
                            : string.Empty,
                        ContractorClassId = applicationUser.ClassId,
                        ContractorClassName = applicationUser.ClassName,
                        ContractorTypeId = applicationUser.CustomerTypeId,
                        ContractorTypeName = applicationUser.CustomerTypeName,
                        ContractorIndustryIds = string.Join(',', applicationUser.IndustryIds),
                        ContractorIndustryNames = applicationUser.CustomerIndustryNames != null
                            ? string.Join(",", applicationUser.CustomerIndustryNames)
                            : string.Empty,
                        ContractorStructureName = applicationUser.CustomerStructureName,
                        ContractorCategoryName = applicationUser.CustomerCategoryName
                    };

                    var partner = new PartnerIntegrationEvent()
                    {
                        IdentityGuid = actionResponse.Result.UserIdentityGuid,
                        HasUpdate = false
                    };

                    var updateContractorIntegrationEvent
                        = new UpdateContractorIntegrationEvent()
                        {
                            Customer = customer,
                            Partner = partner
                        };
                    await _integrationEventLogService.AddAndSaveEventAsync(updateContractorIntegrationEvent);
                    await _integrationEventLogService.PublishEventsThroughEventBusAsync(dbTransaction.TransactionId);

                    await _dbContext.CommitTransactionAsync(dbTransaction);

                    return Ok(actionResponse);
                }

                return BadRequest(actionResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _dbContext.RollbackTransaction();
                return BadRequest();
            }
            finally
            {
                _dbContext.Dispose();
            }
        }

        // POST: api/ApplicationUsers
        [HttpPost]
        public async Task<IActionResult> PostApplicationUser(UserViewModel applicationUser)
        {
            var validator = new ApplicationUserValidator(applicationUser.IsEnterprise);
            var validateResult = await validator.ValidateAsync(applicationUser);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            if (_accountRepository.CheckExitUserName(applicationUser.UserName))
            {
                return BadRequest(new ErrorGeneric("Tên tài khoản đã tồn tại ", nameof(applicationUser.UserName)));
            }

            if (_accountRepository.CheckExitUserCode(applicationUser.CustomerCode))
            {
                return BadRequest(new ErrorGeneric("Mã khách hàng đã tồn tại, vui lòng nhập một mã khác",
                    nameof(applicationUser.CustomerCode)));
            }

            //if (_accountRepository.CheckExitMobile(applicationUser.MobilePhoneNo))
            //{
            //    return BadRequest(new ErrorGeneric("Số điện thoại liên hệ đã tồn tại ",
            //        nameof(applicationUser.MobilePhoneNo)));
            //}

            //if (_accountRepository.CheckExitEmail(applicationUser.Email))
            //{
            //    return BadRequest(new ErrorGeneric("Địa chỉ mail đã tồn tại ", nameof(applicationUser.Email)));
            //}

            if (applicationUser.IsEnterprise)
            {
                if (_accountRepository.CheckExitFaxNo(applicationUser.FaxNo))
                {
                    return BadRequest(new ErrorGeneric("Số fax đã tồn tại ", nameof(applicationUser.FaxNo)));
                }

                if (_accountRepository.CheckExitIdNo(applicationUser.TaxIdNo))
                {
                    return BadRequest(new ErrorGeneric("Mã số thuế đã tồn tại ", nameof(applicationUser.TaxIdNo)));
                }

                if (_accountRepository.CheckExitBusinessRegCertificate(applicationUser.BusinessRegCertificate))
                {
                    return BadRequest(new ErrorGeneric("Số đăng ký kinh doanh đã tồn tại ",
                        nameof(applicationUser.BusinessRegCertificate)));
                }

                if (_accountRepository.CheckExitPartner(applicationUser.UserIdentityGuid))
                {
                    var applicationUserEntity = await _accountRepository.FindByUserIdentityGuidAsync(applicationUser.UserIdentityGuid);
                    return BadRequest(new ErrorGeneric($"Đối tác đã tồn tại ở tài khoản {applicationUserEntity.UserName}", nameof(applicationUser.UserIdentityGuid)));
                }
            }
            else
            {
                if (_accountRepository.CheckExitIdNo(applicationUser.IdNo))
                {
                    return BadRequest(new ErrorGeneric("CMT/ Hộ chiếu đã tồn tại ", nameof(applicationUser.IdNo)));
                }
            }

            // Binding auto assign property
            applicationUser.IdentityGuid = Guid.NewGuid().ToString();
            applicationUser.SecurityStamp = Guid.NewGuid().ToString();
            applicationUser.Code = applicationUser.UserName;

            if (!string.IsNullOrWhiteSpace(applicationUser.Password))
            {
                //Security information 
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                applicationUser.PasswordSalt = Convert.ToBase64String(salt);
                var encryptedPassword =
                    $"{applicationUser.PasswordSalt}{applicationUser.Password}".EncryptMD5(_appSettings.MD5CryptoKey);
                applicationUser.Password = encryptedPassword;
            }

            var dbTransaction = await _dbContext.BeginTransactionAsync();

            try
            {
                if (applicationUser.Avatar == null)
                {
                    applicationUser.AvatarId = null;
                }
                else
                {
                    var storedAvatarItem =
                        await _staticResourceService.PersistentImage(applicationUser.Avatar.TemporaryUrl);
                    var addUserAvatarResponse = await _pictureRepository.CreateAndSave(storedAvatarItem);
                    if (addUserAvatarResponse.IsSuccess)
                    {
                        applicationUser.AvatarId = addUserAvatarResponse.Result.Id;
                    }
                    else
                    {
                        return BadRequest(addUserAvatarResponse);
                    }
                }

                var actionResponse = await _accountRepository.CreateAndSave(applicationUser);

                if (applicationUser.GroupIds != null && applicationUser.GroupIds.Count > 0)
                {
                    foreach (var group in applicationUser.GroupIds)
                    {
                        var groupModel = new ApplicationUserUserGroup
                        {
                            UserId = actionResponse.Result.Id,
                            GroupId = group
                        };

                        _groupRepository.Create(groupModel);
                    }
                }

                //await _applicationUserIndustryRepository.DeleteAllMapIndustryByUserId(applicationUser.Id);
                if (applicationUser.IndustryIds != null && applicationUser.IndustryIds.Count > 0)
                {
                    foreach (var industryId in applicationUser.IndustryIds)
                    {
                        var industryModel = new ApplicationUserIndustry
                        {
                            UserId = actionResponse.Result.Id,
                            IndustryId = industryId
                        };

                        _applicationUserIndustryRepository.Create(industryModel);
                    }
                }


                if (actionResponse.IsSuccess)
                {
                    //Event
                    var customer = new CustomerIntegrationEvent()
                    {
                        IdentityGuid = actionResponse.Result.IdentityGuid,
                        AccountingCustomerCode = actionResponse.Result.AccountingCustomerCode,
                        Email = actionResponse.Result.Email,
                        FaxNo = actionResponse.Result.FaxNo,
                        FullName = actionResponse.Result.FullName,
                        FirstName = actionResponse.Result.FirstName,
                        LastName = actionResponse.Result.LastName,
                        IdNo = actionResponse.Result.IdNo,
                        MobilePhoneNo = actionResponse.Result.MobilePhoneNo,
                        TaxIdNo = actionResponse.Result.TaxIdNo,
                        UserName = actionResponse.Result.UserName,
                        CustomerCode = actionResponse.Result.CustomerCode,
                        ShortName = actionResponse.Result.ShortName,
                        Address = actionResponse.Result.Address,

                        City = actionResponse.Result.Province,
                        CityId = actionResponse.Result.ProvinceIdentityGuid,
                        District = actionResponse.Result.District,
                        DistrictId = actionResponse.Result.DistrictIdentityGuid,
                        HasUpdate = true,
                        ContractorStructureId = applicationUser.CustomerStructureId,
                        ContractorCategoryId = applicationUser.CustomerCategoryId,
                        ContractorGroupIds = string.Join(',', applicationUser.GroupIds),
                        ContractorGroupNames = applicationUser.GroupNames != null
                            ? string.Join(",", applicationUser.GroupNames)
                            : string.Empty,
                        ContractorClassId = applicationUser.ClassId,
                        ContractorClassName = applicationUser.ClassName,
                        ContractorTypeId = applicationUser.CustomerTypeId,
                        ContractorTypeName = applicationUser.CustomerTypeName,
                        ContractorIndustryIds = string.Join(',', applicationUser.IndustryIds),
                        ContractorIndustryNames = applicationUser.CustomerIndustryNames != null
                            ? string.Join(",", applicationUser.CustomerIndustryNames)
                            : string.Empty,
                        ContractorStructureName = applicationUser.CustomerStructureName,
                        ContractorCategoryName = applicationUser.CustomerCategoryName
                    };

                    var partner = new PartnerIntegrationEvent()
                    {
                        IdentityGuid = actionResponse.Result.UserIdentityGuid,
                        HasUpdate = false
                    };

                    var updateContractorIntegrationEvent
                        = new UpdateContractorIntegrationEvent()
                        {
                            Customer = customer,
                            Partner = partner
                        };
                    await _integrationEventLogService.AddAndSaveEventAsync(updateContractorIntegrationEvent);
                    await _integrationEventLogService.PublishEventsThroughEventBusAsync(dbTransaction.TransactionId);

                    await _dbContext.CommitTransactionAsync(dbTransaction);

                    return Ok(actionResponse);
                }

                return BadRequest(actionResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _dbContext.RollbackTransaction();
                return BadRequest();
            }
            finally
            {
                _dbContext.Dispose();
            }
        }

        // DELETE: api/ApplicationUsers/5
        [HttpDelete("{id}")]
        public IActionResult DeleteApplicationUser(int id)
        {
            var applicationUser = _accountQueries.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            var deleteResponse = _accountRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }

        [HttpGet("GetOrderNumberByNow")]
        public IActionResult GetOrderNumberByNow()
        {
            return Ok(_accountQueries.GetOrderNumberByNow());
        }

        [Route("GenerateCustomerCode")]
        [HttpGet]
        public async Task<IActionResult> GenerateCustomerCode(string provinceIdentityGuid, string industryCodes, string shortName)
        {
            return Ok(await _accountQueries
                .GenerateCustomerCode(provinceIdentityGuid, (!string.IsNullOrWhiteSpace(industryCodes) ? industryCodes.Split(',') : null), shortName));
        }

        [Route("getAllPaymentTargetByParentId")]
        [HttpGet]
        public IActionResult GetAllPaymentTargetByParentId(string parentPaymentTargetId)
        {
            if (string.IsNullOrWhiteSpace(parentPaymentTargetId))
            {
                return BadRequest();
            }
            return Ok(_accountQueries.GetAllPaymentTargetByParentId(parentPaymentTargetId));
        }

        [Route("GenerateCustomerCodeIndividual")]
        [HttpGet]
        public IActionResult GenerateCustomerCodeIndividual(int? customerCategoryId = null, int? isEnterPrise = null, int? customerGroupId = null)
        {
            return Ok(_accountQueries.GenerateCustomerCodeIndividual(customerCategoryId, isEnterPrise, customerGroupId));

        }
    }
}