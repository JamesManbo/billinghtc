using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using GenericRepository;
using Global.Configs.SystemArgument;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using Microsoft.Extensions.Configuration;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.Models.Common;
using OrganizationUnit.Domain.Models.ConfigurationSettingUser;
using OrganizationUnit.Domain.Models.FCM;
using OrganizationUnit.Domain.Models.OrganizationUnit;
using OrganizationUnit.Domain.Models.Permission;
using OrganizationUnit.Domain.Models.User;

namespace OrganizationUnit.Infrastructure.Queries
{
    public interface IUserQueries : IQueryRepository
    {
        string GetEmailsOfServiceProvider();
        UserDTO FindById(int id);
        UserDTO FindById(string id);
        UserDTO FindByUserName(string userName);
        IEnumerable<UserDTO> GetManagementUserByOrgUnit(string organizationUnitCode);
        IPagedList<UserGridDTO> GetList(UserRequestFilterModel filterModel);
        IEnumerable<SelectionItem> GetSelectionList();
        IEnumerable<UserDTO> Autocomplete(RequestFilterModel filterModel);
        IEnumerable<SelectionItem> GetListTypeSelection(bool? isPartner, string organizationUnitId, bool isNotBelongToOrgUnit = false);
        IEnumerable<PermissionDTO> GetPermissionsOfUser(int userId);
        IEnumerable<UserBankAccountDTO> GetBankById(int id);
        IEnumerable<SelectionItem> GetAllUserByRoleSelection(string sRole);
        IEnumerable<UserDTO> GetAllUserByRole(string roleCode);
        IEnumerable<UserDTO> GetAllUserByDepartment(string departmentCode, string keyword = null);
        IEnumerable<SelectionItem> GetAllUserFilter(string sRole, string filterValue);
        IEnumerable<SelectionItem> GetUserByIds(int[] sListIds);
        List<FCMTokenDto> GetUserFCMTokens(string uids);
        List<FCMTokenDto> GetListTokenByRoleUser(string roleCode);
        List<FCMTokenDto> GetListTokenByDepartment(string departmentCode);
    }

    public class UserQueries : QueryRepository<User, int>, IUserQueries
    {
        private readonly IConfiguration _config;
        private readonly IOrganizationUnitQueryRepository _organizationUnitQueryRepository;
        public UserQueries(OrganizationUnitDbContext organizationUnitDbContext,
            IConfiguration config,
            IOrganizationUnitQueryRepository organizationUnitQueryRepository) : base(organizationUnitDbContext)
        {
            _config = config;
            this._organizationUnitQueryRepository = organizationUnitQueryRepository;

            RestrictByOrganization(1);
        }

        public List<FCMTokenDto> GetListTokenByRoleUser(string roleCode)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<FCMTokenDto>();
            dapperExecution.SqlBuilder.Select("t1.`FullName` AS `Receiver`");
            dapperExecution.SqlBuilder.Select("t2.`Token` AS `Token`");
            dapperExecution.SqlBuilder.Select("t2.`Platform` AS `Platform`");
            dapperExecution.SqlBuilder.Select("t2.`ReceiverId` AS `ReceiverId`");
            dapperExecution.SqlBuilder.Select("t2.`Id` AS `Id`");

            dapperExecution.SqlBuilder.Where("t4.RoleCode = @code", new { code = roleCode });

            dapperExecution.SqlBuilder.InnerJoin(
                "UserRoles t3 ON t3.UserId = t1.Id AND t3.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.InnerJoin(
                "Roles t4 ON t4.Id = t3.RoleId");

            dapperExecution.SqlBuilder.LeftJoin(
                "FCMTokens t2 ON t1.IdentityGuid = t2.ReceiverId AND t2.IsDeleted = FALSE");

            return dapperExecution.ExecuteQuery().ToList();
        }

        public List<FCMTokenDto> GetUserFCMTokens(string uids)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<FCMTokenDto>();
            dapperExecution.SqlBuilder.Select("t1.`FullName` AS `Receiver`");
            dapperExecution.SqlBuilder.Select("t2.`Token` AS `Token`");
            dapperExecution.SqlBuilder.Select("t2.`Platform` AS `Platform`");
            dapperExecution.SqlBuilder.Select("t2.`ReceiverId` AS `ReceiverId`");
            dapperExecution.SqlBuilder.Select("t2.`Id` AS `Id`");
            dapperExecution.SqlBuilder.LeftJoin(
                "FCMTokens t2 ON t1.IdentityGuid = t2.ReceiverId", new { uids });

            dapperExecution.SqlBuilder.Where("FIND_IN_SET(t1.IdentityGuid, @uids) > 0");
            dapperExecution.SqlBuilder.Where("t2.IsDeleted = FALSE");

            return dapperExecution.ExecuteQuery().ToList();
        }

        public IPagedList<UserGridDTO> GetList(UserRequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<UserGridDTO>(filterModel);
            dapperExecution.SqlBuilder.LeftJoin("OrganizationUnitUsers oum ON oum.UserId = t1.Id");
            dapperExecution.SqlBuilder.LeftJoin("OrganizationUnits t2 ON oum.OrganizationUnitId = t2.Id AND t2.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.Select("t2.Id AS OrganizationUnitId");
            dapperExecution.SqlBuilder.Select("t2.Name AS OrganizationUnitName");

            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.FullName LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.Code LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.UserName LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.Email LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.Address LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.MobilePhoneNo LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.FaxNo LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" });
            }

            if (filterModel.IsPartner)
            {
                dapperExecution.SqlBuilder.Where("t1.IsPartner = @isPartner", new { isPartner = filterModel.IsPartner });
            }

            if (filterModel.Assigned.HasValue && filterModel.RoleId > 0)
            {
                if (filterModel.Assigned.Value)
                {
                    dapperExecution.SqlBuilder.InnerJoin("UserRoles ur ON t1.Id = ur.UserId AND ur.IsDeleted = FALSE");
                    dapperExecution.SqlBuilder.Where("ur.RoleId = @roleId", new { filterModel.RoleId });
                }
                else
                {
                    dapperExecution.SqlBuilder.LeftJoin("UserRoles ur ON t1.Id = ur.UserId AND ur.IsDeleted = FALSE AND ur.RoleId = @roleId", new { filterModel.RoleId });
                    dapperExecution.SqlBuilder.Where("ur.RoleId IS NULL");
                }
            }

            if (filterModel.Any("MobilePhoneNo"))
            {
                var mobilePhoneNo = filterModel.Get("MobilePhoneNo");
                dapperExecution.SqlBuilder.Where("t1.MobilePhoneNo LIKE @mobilePhoneNo", new { mobilePhoneNo = $"%{mobilePhoneNo}%" });
            }

            if (filterModel.Any("Email"))
            {
                var email = filterModel.Get("Email");
                dapperExecution.SqlBuilder.Where("t1.Email LIKE @email", new { email = $"%{email}%" });
            }

            if (filterModel.Any("RootOrgUnitId"))
            {
                var departmentFamily = _organizationUnitQueryRepository.GetAllChildById(
                        filterModel.Get<int>("RootOrgUnitId"));
                if (departmentFamily != null && departmentFamily.Any())
                {
                    var departmentFamilyIds = departmentFamily.Select(c => c.Id).ToArray();
                    dapperExecution.SqlBuilder.Where("t1.OrganizationUnitId IN @departments", new { departments = departmentFamilyIds });
                }
            }

            var cachedResult = new Dictionary<int, UserGridDTO>();
            return dapperExecution.ExecutePaginateQuery<UserGridDTO, int?, string>((user, ouId, ouName) =>
            {
                if (!cachedResult.TryGetValue(user.Id, out var result))
                {
                    result = user;
                    cachedResult.Add(user.Id, result);
                }

                if (ouId.HasValue && !result.OrganizationUnitIds.Contains(ouId.Value))
                {
                    result.OrganizationUnitIds.Add(ouId.Value);
                    result.OrganizationUnitNames +=
                        $"{(string.IsNullOrEmpty(result.OrganizationUnitNames) ? "" : ", ")}{ouName}";
                }

                return result;
            }, splitOnColumn: "OrganizationUnitId,OrganizationUnitName");
        }

        public IEnumerable<SelectionItem> GetSelectionList()
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("CONCAT_WS(' ', t1.`FullName`, t1.Email) AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.`Id` AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.`UserName` AS `Code`");
            dapperExecution.SqlBuilder.Select("t1.`IdentityGuid` AS `GlobalValue`");
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<SelectionItem> GetListTypeSelection(bool? isPartner = null, string organizationUnitCode = "", bool isNotBelongToOrgUnit = false)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();

            dapperExecution.SqlBuilder.Select("CONCAT_WS(' ', t1.`FullName`, t1.Email) AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.`Id` AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.`UserName` AS `Code`");
            dapperExecution.SqlBuilder.Select("t1.`IdentityGuid` AS `GlobalValue`");

            if (string.IsNullOrEmpty(organizationUnitCode))
            {
                if (isNotBelongToOrgUnit)
                {
                    dapperExecution.SqlBuilder.LeftJoin("OrganizationUnitUsers oum ON oum.UserId = t1.Id");
                    dapperExecution.SqlBuilder.Where("oum.OrganizationUnitId IS NULL");
                }
            }
            else
            {
                var departments = this._organizationUnitQueryRepository.GetAllChildByCode(organizationUnitCode);
                if (departments == null || departments.Count() == 0) return default;

                var organizationUnitIds = departments.Select(d => d.Id).ToArray();
                if (isNotBelongToOrgUnit)
                {
                    dapperExecution.SqlBuilder.LeftJoin("OrganizationUnitUsers oum ON t1.Id = oum.UserId");
                    dapperExecution.SqlBuilder.Where("(oum.OrganizationUnitId IS NULL OR oum.OrganizationUnitId IN @organizationUnitIds)", new { organizationUnitIds });
                }
                else
                {
                    dapperExecution.SqlBuilder.InnerJoin("OrganizationUnitUsers oum ON t1.Id = oum.UserId");
                    dapperExecution.SqlBuilder.Where("oum.OrganizationUnitId IN @organizationUnitIds", new { organizationUnitIds });
                }
            }

            if (isPartner.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.IsPartner = @isPartner", new { isPartner });
            }

            return dapperExecution.ExecuteQuery().GroupBy(d => d.Value).Select(d => d.First());
        }

        public IEnumerable<UserDTO> Autocomplete(RequestFilterModel filterModel)
        {
            var cached = new Dictionary<int, UserDTO>();
            var dapperExecution = BuildByTemplate<UserDTO>(filterModel);
            dapperExecution.SqlBuilder.LeftJoin("UserBankAccounts uba ON uba.UserId = t1.Id AND uba.IsDeleted = 0");
            dapperExecution.SqlBuilder.Select(
                "CONCAT_WS('', t1.`FullName`, ', SĐT: ', t1.`MobilePhoneNo`, ', Đ/c: ', t1.`Address`) AS `Label`");
            dapperExecution.SqlBuilder.Select("uba.*");
            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.FullName LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.Code LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.UserName LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.Email LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.Address LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.MobilePhoneNo LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" })
                    .OrWhere("t1.FaxNo LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" });
            }

            //return dapperExecution.ExecuteQuery();
            return dapperExecution.ExecuteQuery<UserDTO, UserBankAccountDTO>((user, userBankAccount) =>
            {
                if (!cached.TryGetValue(user.Id, out var userModel))
                {
                    userModel = user;
                    cached.Add(user.Id, user);
                }

                if (userBankAccount != null && userModel.UserBankAccounts.All(a => a.Id != userBankAccount.Id))
                {
                    userModel.UserBankAccounts.Add(userBankAccount);
                }

                return userModel;
            }, "Id");
        }

        private UserDTO Find(int? id = null, string uid = null, string userName = null)
        {
            var cached = new Dictionary<int, UserDTO>();
            var dapperExecution = BuildByTemplate<UserDTO>();
            dapperExecution.SqlBuilder.Select("pic.*");
            dapperExecution.SqlBuilder.Select("uba.*");
            dapperExecution.SqlBuilder.Select("uci.*");
            dapperExecution.SqlBuilder.Select("t5.*");

            dapperExecution.SqlBuilder.Select("og.Id AS Id");
            dapperExecution.SqlBuilder.Select("og.TreePath AS TreePath");
            dapperExecution.SqlBuilder.Select("og.Code AS Code");
            dapperExecution.SqlBuilder.Select("oum.PositionLevel AS PositionLevel");

            dapperExecution.SqlBuilder.LeftJoin("Pictures pic ON pic.Id = t1.AvatarId AND pic.IsDeleted = 0");
            dapperExecution.SqlBuilder.LeftJoin("UserBankAccounts uba ON uba.UserId = t1.Id AND uba.IsDeleted = 0");
            dapperExecution.SqlBuilder.LeftJoin("OrganizationUnitUsers oum ON t1.Id = oum.UserId");
            dapperExecution.SqlBuilder.LeftJoin("OrganizationUnits og ON og.Id = oum.OrganizationUnitId AND og.IsDeleted = 0");
            dapperExecution.SqlBuilder.LeftJoin("ContactInfos uci ON uci.UserId = t1.Id AND uci.IsDeleted = 0");
            dapperExecution.SqlBuilder.LeftJoin("ConfigurationPersonalAccounts t5 ON t5.UserId = t1.Id AND t5.IsDeleted = 0");

            if (id.HasValue)
            {
                dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            }
            else if (!string.IsNullOrEmpty(uid))
            {
                dapperExecution.SqlBuilder.Where("t1.IdentityGuid = @uid", new { uid });
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                dapperExecution.SqlBuilder.Where("t1.UserName = @userName", new { userName });
            }

            var cachedResult = new Dictionary<int, UserDTO>();
            var query = dapperExecution.ExecuteQuery<UserDTO,
                PictureDto,
                UserBankAccountDTO,
                UserContactInfoDTO,
                ConfigurationSettingUserDto,
                UserOrganizationSimpleModel>((user, picture, userBankAccount, userContactInfo, userConfig, userOrg) =>
            {
                if (!cached.TryGetValue(user.Id, out var result))
                {
                    result = user;
                    cached.Add(user.Id, result);
                }

                result.Avatar = picture;

                if (userBankAccount != null && result.UserBankAccounts.All(a => a.Id != userBankAccount.Id))
                {
                    result.UserBankAccounts.Add(userBankAccount);
                }

                if (userContactInfo != null && result.UserContactInfos.All(a => a.Id != userContactInfo.Id))
                {
                    result.UserContactInfos.Add(userContactInfo);
                }

                if (userOrg != null)
                {
                    if (userOrg.PositionLevel > 0)
                    {
                        if (!string.IsNullOrEmpty(userOrg.TreePath))
                        {
                            if (result.MngOrganizationUnitPaths?.Count == 0)
                            {
                                result.MngOrganizationUnitPaths = new List<string>() { userOrg.TreePath };
                            }
                            else if (result.MngOrganizationUnitPaths.IndexOf(userOrg.TreePath) < 0)
                            {
                                result.MngOrganizationUnitPaths.Add(userOrg.TreePath);
                            }
                        }
                    }
                    else
                    {
                        if (userOrg.Id > 0 && !result.OrganizationUnitIds.Contains(userOrg.Id))
                        {
                            result.OrganizationUnitIds.Add(userOrg.Id);
                        }

                        if (!string.IsNullOrEmpty(userOrg.TreePath) &&
                            result.OrganizationUnitPaths.IndexOf(userOrg.TreePath) < 0)
                        {
                            result.OrganizationUnitPaths.Add(userOrg.TreePath);
                        }

                        if (!string.IsNullOrEmpty(userOrg.Code) &&
                            result.OrganizationUnits.IndexOf(userOrg.Code) < 0)
                        {
                            result.OrganizationUnits.Add(userOrg.Code);
                        }
                    }
                }

                result.ConfigurationAccount = userConfig;

                return result;
            }, "Id,Id,Id,Id,Id");

            return query.FirstOrDefault();
        }

        public UserDTO FindById(int id)
        {
            return this.Find(id: id);
        }

        public UserDTO FindById(string uid)
        {
            return this.Find(uid: uid);
        }

        public UserDTO FindByUserName(string userName)
        {
            return this.Find(userName: userName);
        }

        public IEnumerable<UserBankAccountDTO> GetBankById(int id)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<UserBankAccountDTO>();
            dapperExecution.SqlBuilder.Select("uba.Id");
            dapperExecution.SqlBuilder.Select("uba.BankName");
            dapperExecution.SqlBuilder.Select("uba.BankAccountNumber");
            dapperExecution.SqlBuilder.Select("uba.BankBranch");
            dapperExecution.SqlBuilder.InnerJoin("UserBankAccounts uba ON uba.UserId = t1.Id AND uba.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<PermissionDTO> GetPermissionsOfUser(int userId)
        {
            return WithConnection(conn => conn.Query<PermissionDTO>("GetPermissionOfUser",
                new { userId },
                commandType: CommandType.StoredProcedure));
        }

        public IEnumerable<SelectionItem> GetAllUserByRoleSelection(string sRole)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("t1.`FullName` AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.`Id` AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.`IdentityGuid` AS `GlobalValue`");

            dapperExecution.SqlBuilder.InnerJoin("UserRoles AS ur ON ur.UserId = t1.Id");
            dapperExecution.SqlBuilder.InnerJoin("Roles AS r ON ur.RoleId = r.Id");

            //dapperExecution.SqlBuilder.Where("ur.IsActive = 1");
            dapperExecution.SqlBuilder.Where("ur.IsDeleted = 0");
            dapperExecution.SqlBuilder.Where("r.RoleCode = @sRole", new { sRole });

            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<SelectionItem> GetAllUserFilter(string sRole, string filterValue)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("t1.`FullName` AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.`Id` AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.`IdentityGuid` AS `GlobalValue`");

            dapperExecution.SqlBuilder.InnerJoin("UserRoles AS ur ON ur.UserId = t1.Id");
            dapperExecution.SqlBuilder.InnerJoin("Roles AS r ON ur.RoleId = r.Id");

            dapperExecution.SqlBuilder.Where("ur.IsDeleted = 0");
            dapperExecution.SqlBuilder.Where("r.RoleCode = @sRole", new { sRole });

            dapperExecution.SqlBuilder.Where("t1.FullName LIKE @keywords", new { keywords = $"%{filterValue}%" });
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<SelectionItem> GetUserByIds(int[] sListIds)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>();
            dapperExecution.SqlBuilder.Select("t1.`FullName` AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.`Id` AS `Value`");
            dapperExecution.SqlBuilder.Select("t1.`IdentityGuid` AS `GlobalValue`");
            dapperExecution.SqlBuilder.Where("t1.Id IN @sListIds", new { sListIds });
            return dapperExecution.ExecuteQuery();
        }

        public string GetEmailsOfServiceProvider()
        {
            var departmentCodes = _config.GetSection("DepartmentCode").Get<DepartmentCode>();
            var childrenDepartments = this._organizationUnitQueryRepository.GetAllChildByCode(departmentCodes.ServiceProviderDepartmentCode);
            if (childrenDepartments == null || childrenDepartments.Count() == 0) return "";

            var organizationUnitIds = childrenDepartments.Select(c => c.Id).ToArray();

            var dapperExecution = BuildByTemplate<string>();
            dapperExecution.SqlBuilder.Select("t1.Email");

            dapperExecution.SqlBuilder.InnerJoin("OrganizationUnitUsers oum ON oum.UserId = t1.Id");

            dapperExecution.SqlBuilder.Where(@"t1.IsDeleted = FALSE 
                AND oum.OrganizationUnitId IN @organizationUnitIds", new { organizationUnitIds });

            var userEmails = dapperExecution.ExecuteQuery()?.Distinct();

            return string.Join(",", userEmails);
        }

        public List<FCMTokenDto> GetListTokenByDepartment(string departmentCode)
        {
            var departments = this._organizationUnitQueryRepository.GetAllChildByCode(departmentCode);
            if (departments == null || departments.Count() == 0) return default;

            var dapperExecution = BuildByTemplateWithoutSelect<FCMTokenDto>();
            dapperExecution.SqlBuilder.Select("t1.`FullName` AS `Receiver`");
            dapperExecution.SqlBuilder.Select("t2.`Token` AS `Token`");
            dapperExecution.SqlBuilder.Select("t2.`Platform` AS `Platform`");
            dapperExecution.SqlBuilder.Select("t2.`ReceiverId` AS `ReceiverId`");
            dapperExecution.SqlBuilder.Select("t2.`Id` AS `Id`");

            dapperExecution.SqlBuilder.InnerJoin("OrganizationUnitUsers oum ON oum.UserId = t1.Id");

            dapperExecution.SqlBuilder.Where("t1.OrganizationUnitId IN @organizationUnits",
                new { organizationUnits = departments.Select(c => c.Id).ToArray() });

            dapperExecution.SqlBuilder.LeftJoin(
                "FCMTokens t2 ON t1.IdentityGuid = t2.ReceiverId AND t2.IsDeleted = FALSE");

            return dapperExecution.ExecuteQuery().GroupBy(d => d.Id).Select(d => d.First()).ToList();
        }

        public IEnumerable<UserDTO> GetManagementUserByOrgUnit(string organizationUnitCode)
        {
            var departments = this._organizationUnitQueryRepository.GetAllChildByCode(organizationUnitCode);
            if (departments == null || departments.Count() == 0) return default;
            var treeIds = departments.Select(c => c.Id).ToArray();
            var dapperExecution = BuildByTemplate<UserDTO>();
            dapperExecution.SqlBuilder.InnerJoin("OrganizationUnits t2 ON t2.ManagementUserId = t1.Id");
            dapperExecution.SqlBuilder.Where("t2.Id IN @treeIds", new { treeIds });
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<UserDTO> GetAllUserByRole(string roleCode)
        {
            var dapperExecution = BuildByTemplate<UserDTO>();
            dapperExecution.SqlBuilder.InnerJoin("UserRoles AS ur ON ur.UserId = t1.Id");
            dapperExecution.SqlBuilder.InnerJoin("Roles AS r ON ur.RoleId = r.Id");

            //dapperExecution.SqlBuilder.Where("ur.IsActive = 1");
            dapperExecution.SqlBuilder.Where("ur.IsDeleted = 0");
            dapperExecution.SqlBuilder.Where("r.RoleCode = @roleCode", new { roleCode });

            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<UserDTO> GetAllUserByDepartment(string departmentCode, string keyword = null)
        {
            var departments = this._organizationUnitQueryRepository.GetAllChildByCode(departmentCode);
            if (departments == null || departments.Count() == 0) return default;

            var dapperExecution = BuildByTemplate<UserDTO>();

            dapperExecution.SqlBuilder.InnerJoin("OrganizationUnitUsers oum ON oum.UserId = t1.Id");
            dapperExecution.SqlBuilder.Where("oum.OrganizationUnitId IN @organizationUnits",
                new { organizationUnits = departments.Select(c => c.Id).ToArray() });

            if (!string.IsNullOrEmpty(keyword))
            {
                dapperExecution.SqlBuilder.Where("(t1.FullName LIKE @keyword OR t1.UserName LIKE @keyword)", new { keyword = $"%{keyword}%" });
            }

            return dapperExecution.ExecuteQuery().GroupBy(d => d.Id).Select(d => d.First());
        }
    }
}