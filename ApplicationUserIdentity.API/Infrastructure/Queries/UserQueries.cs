using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Grpc.Clients.Location;
using ApplicationUserIdentity.API.Infrastructure.Helper;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Models.Configs;
using ApplicationUserIdentity.API.Models.LocationDomainModels;
using Dapper;
using GenericRepository;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using Microsoft.Extensions.Options;

namespace ApplicationUserIdentity.API.Infrastructure.Queries
{
    public interface IUserQueries : IQueryRepository
    {
        IPagedList<UserViewModel> GetList(UserRequestFilterModel requestFilterModel);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel requestFilterModel);
        IEnumerable<UserViewSimpleModel> Autocomplete(RequestFilterModel requestFilterModel);
        UserViewModel Find(int id);
        UserViewModel Find(string id);
        UserViewModel FindByUserName(string userName);
        int GetOrderNumberByNow();
        string GenerateUserCode(string[] groupCodes, string categoryCode, string typeCode);
        Task<string> GenerateCustomerCode(string provinceIdentityGuid, string[] industryCodes, string shortName);
        IEnumerable<UserViewSimpleModel> GetAllPaymentTargetByParentId(string parentPaymentTargetId);
        string GetCustomerCategoryCode(int? customerCategoryId);
        string GetCustomerTypeCode(int? customerTypeId);
        string GetCustomerGroupCode(int? groupId);
        string GenerateCustomerCodeIndividual(int? customerCategoryId, int? isEnterPrise, int? groupId);
        HashSet<string> GetAllUserName();

    }

    public class UserQueries : QueryRepository<ApplicationUser, int>, IUserQueries
    {
        private readonly AppSettings _appSettings;
        private readonly ILocationGrpcService _locationGrpcService;

        public UserQueries(ApplicationUserDbContext context,
            IOptions<AppSettings> appSettings, ILocationGrpcService locationGrpcService) : base(context)
        {
            _appSettings = appSettings.Value;
            _locationGrpcService = locationGrpcService;
            RestrictByOrganization(1);
        }

        public IPagedList<UserViewModel> GetList(UserRequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<UserViewModel>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("t6.Name AS CustomerTypeName");
            dapperExecution.SqlBuilder.Select("t2.ClassName AS ClassName");
            
            dapperExecution.SqlBuilder.Select("t3.*");
            dapperExecution.SqlBuilder.LeftJoin("ApplicationUserClasses t2 ON t2.Id = t1.ClassId AND t2.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin("Pictures t3 ON t1.AvatarId = t3.Id");
            if (requestFilterModel.GroupId.HasValue && requestFilterModel.GroupId > 0)
            {
                dapperExecution.SqlBuilder.InnerJoin(
                    "ApplicationUserUserGroups t4 ON t1.Id = t4.UserId AND t4.IsActive = TRUE AND t4.IsDeleted = FALSE");
                dapperExecution.SqlBuilder.Where("t4.GroupId = @groupId", new { requestFilterModel.GroupId });
            }

            if (requestFilterModel.IndustryId.HasValue && requestFilterModel.IndustryId > 0)
            {
                dapperExecution.SqlBuilder.InnerJoin(
                    "ApplicationUserIndustries t5 ON t1.Id = t5.UserId AND t5.IsActive = TRUE AND t5.IsDeleted = FALSE");
                dapperExecution.SqlBuilder.Where("t5.IndustryId = @industryId", new { requestFilterModel.IndustryId });
            }

            dapperExecution.SqlBuilder.LeftJoin("CustomerTypes t6 ON t6.Id = t1.CustomerTypeId AND t6.IsDeleted = FALSE"); 

            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                      .OrWhere("t1.FullName LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                      .OrWhere("t1.Address LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                      .OrWhere("t1.CustomerCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                      .OrWhere("t1.UserName LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                      .OrWhere("t1.Email LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                      .OrWhere("t1.MobilePhoneNo LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                      .OrWhere("t1.FaxNo LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" });
            }
            if (!string.IsNullOrEmpty(requestFilterModel.MarketAreaIds) || !string.IsNullOrEmpty(requestFilterModel.ProjectIds))
            {
                if (!string.IsNullOrEmpty(requestFilterModel.IdentityGuids))
                {
                    var identityGuids = requestFilterModel.IdentityGuids.Split(",");
                    dapperExecution.SqlBuilder.Where("(t1.IdentityGuid IN @identityGuids)", new { identityGuids });
                }
                else return new PagedList<UserViewModel>(requestFilterModel.Skip, requestFilterModel.Take, 0) { };
            }

            if (requestFilterModel.PropertyFilterModels.Any(a => a.Field.Contains("MobilePhoneNo")))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels.FirstOrDefault(p => p.Field.Contains("MobilePhoneNo"));
                var mobilePhoneNo = propertyFilter.FilterValue;
                dapperExecution.SqlBuilder.Where(" (t1.MobilePhoneNo LIKE @mobilePhoneNo)", new { mobilePhoneNo = $"%{mobilePhoneNo}%" });
            }

            if (requestFilterModel.PropertyFilterModels.Any(a => a.Field.Contains("Email")))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels.FirstOrDefault(p => p.Field.Contains("Email"));
                var email = propertyFilter.FilterValue;
                dapperExecution.SqlBuilder.Where(" (t1.Email LIKE @email)", new { email = $"%{email}%" });
            }

            if (requestFilterModel.Filters != null)
            {
                foreach (string item in requestFilterModel.Filters.Split("|"))
                {
                    string sqlWhere = "";
                    string[] filter = item.Split("::");
                    string colName = filter[0].ToUpperFirstLetter();
                    if (colName.Contains("TimeLine"))
                    {
                        colName = "t2." + colName;
                    }
                    else
                    {
                        colName = "t1." + colName;
                    }
                    string value = filter[1];
                    switch (filter[2])
                    {
                        case "contains":
                            sqlWhere = colName + " LIKE '%" + value + "%'";
                            dapperExecution.SqlBuilder.Where(sqlWhere);
                            break;
                        case "doesnotcontain":
                            dapperExecution.SqlBuilder.Where("@colName NOT LIKE @value", new { colName, value });
                            break;
                        case "gte":
                            sqlWhere = colName + " >= " + "'" + value + "'";
                            dapperExecution.SqlBuilder.Where(sqlWhere);
                            break;
                        case "lte":
                            sqlWhere = colName + " <= " + "'" + value + "'";
                            dapperExecution.SqlBuilder.Where(sqlWhere);
                            break;
                    }
                }
            }

            return dapperExecution.ExecutePaginateQuery<UserViewModel, PictureViewModel>((user, avatar) =>
            {
                user.Avatar = avatar;
                return user;
            }, "Id");
        }

        public IEnumerable<UserViewSimpleModel> Autocomplete(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<UserViewSimpleModel>(requestFilterModel);

            dapperExecution.SqlBuilder.Select(
                "CONCAT_WS('', t1.FullName, '(', t1.CustomerCode, ')', ', SĐT: ', t1.MobilePhoneNo, ', Đ/c: ', t1.Address) AS `Label`");

            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.FullName LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.Address LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.CustomerCode LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.UserName LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.Email LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.MobilePhoneNo LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.FaxNo LIKE @keywords", new { keywords = $"%{requestFilterModel.Keywords}%" });
            }
            if (requestFilterModel.PropertyFilterModels.Any(a => a.Field == "ParentTargetId"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels.FirstOrDefault(p => p.Field == "ParentTargetId");
                var parentTargetId = propertyFilter.FilterValue;
                dapperExecution.SqlBuilder.Where(" (t1.IdentityGuid = @parentTargetId OR t1.ParentId = @parentTargetId )", new { parentTargetId });
            }
            var result = dapperExecution.ExecuteQuery().ToList();
            if (requestFilterModel.PropertyFilterModels.Any(a => a.Field == "value"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "value");
                dapperExecution.SqlBuilder.AppendPredicate<string>("t1.IdentityGuid", propertyFilter);
                var userViewModel = dapperExecution.ExecuteQuery().FirstOrDefault();
                if (userViewModel != null)
                {
                    result.Add(userViewModel);
                }
            }

            return result;
        }

        public UserViewModel Find(int id)
        {
            return this.FindByDynamicId(id);
        }

        public UserViewModel Find(string id)
        {
            return this.FindByDynamicId(id);
        }

        public int GetOrderNumberByNow()
        {
            return WithConnection(conn =>
                conn.QueryFirst<int>(
                    "SELECT (COUNT(1) + 1) FROM ApplicationUsers WHERE IsDeleted = FALSE"));
        }

        private UserViewModel FindByDynamicId(object id)
        {
            var cache = new Dictionary<int, UserViewModel>();
            var dapperExecution = BuildByTemplate<UserViewModel>();

            dapperExecution.SqlBuilder.Select("t4.ClassName");
            dapperExecution.SqlBuilder.Select("t8.Name AS CustomerCategoryName");
            dapperExecution.SqlBuilder.Select("t9.Name AS CustomerStructureName");
            dapperExecution.SqlBuilder.Select("t10.Name AS CustomerTypeName");
            dapperExecution.SqlBuilder.Select("t2.Id");
            dapperExecution.SqlBuilder.Select("t2.Name");
            dapperExecution.SqlBuilder.Select("t2.FileName");
            dapperExecution.SqlBuilder.Select("t2.FilePath");
            dapperExecution.SqlBuilder.Select("t2.Size");
            dapperExecution.SqlBuilder.Select("t2.Extension");
            dapperExecution.SqlBuilder.Select("t2.RedirectLink");

            dapperExecution.SqlBuilder.Select("t3.Id");
            dapperExecution.SqlBuilder.Select("t3.GroupId");
            dapperExecution.SqlBuilder.Select("t6.GroupName");

            dapperExecution.SqlBuilder.Select("t5.Id");
            dapperExecution.SqlBuilder.Select("t5.IndustryId");
            dapperExecution.SqlBuilder.Select("t7.Name");

            if (id is int)
            {
                dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            }
            else
            {
                // Tìm kiếm chứa(contains) thay cho bằng vì nguyên nhân chưa rõ ở 
                // các bản ghi thêm vào bằng insert bulk
                dapperExecution.SqlBuilder.Where("t1.IdentityGuid LIKE @id", new { id = $"%{id}%" });
            }

            dapperExecution.SqlBuilder.LeftJoin("Pictures t2 ON t1.AvatarId = t2.Id AND t2.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin(
                "ApplicationUserUserGroups t3 ON t1.Id = t3.UserId AND t3.IsActive = TRUE AND t3.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin(
                "ApplicationUserClasses t4 ON t1.ClassId = t4.Id AND t4.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "ApplicationUserIndustries t5 ON t1.Id = t5.UserId AND t5.IsActive = TRUE AND t5.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "ApplicationUserGroups t6 ON t3.GroupId = t6.Id AND t6.IsActive = TRUE AND t6.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "Industries t7 ON t5.IndustryId = t7.Id AND t7.IsActive = TRUE AND t7.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "CustomerCategories t8 ON t1.CustomerCategoryId = t8.Id AND t8.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "CustomerStructure t9 ON t1.CustomerStructureId = t9.Id AND t9.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.LeftJoin(
                "CustomerTypes t10 ON t1.CustomerTypeId = t10.Id AND t10.IsDeleted = FALSE");


            var userModel = dapperExecution.ExecuteQuery<UserViewModel, PictureViewModel, UserUserGroupViewModel, UserIndustryViewModel>(
                    (user, avatar, group, industry) =>
                    {
                        if (!cache.TryGetValue(user.Id, out var userViewModel))
                        {
                            userViewModel = user;
                            cache.Add(user.Id, userViewModel);
                        }

                        if (group != null && userViewModel.GroupIds.All(e => e != group.GroupId))
                        {
                            userViewModel.GroupIds.Add(group.GroupId);
                            userViewModel.GroupNames.Add(group.GroupName);
                        }

                        if (industry != null && userViewModel.IndustryIds.All(e => e != industry.IndustryId))
                        {
                            userViewModel.IndustryIds.Add(industry.IndustryId);
                            userViewModel.CustomerIndustryNames.Add(industry.Name);
                        }

                        userViewModel.Avatar = avatar;
                        return userViewModel;
                    }, "Id,Id,Id,Id")
                .Distinct().FirstOrDefault();

            if (userModel != null && !string.IsNullOrWhiteSpace(userModel.Password))
            {
                userModel.Password = userModel.Password.DecryptMD5(_appSettings.MD5CryptoKey)
                    .Substring(userModel.PasswordSalt.Length);
            }

            return userModel;
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("t1.FullName AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            dapperExecution.SqlBuilder.Where("t1.IsActive = TRUE");
            return dapperExecution.ExecuteQuery();
        }

        public string GenerateUserCode(string[] groupCodes, string categoryCode, string typeCode)
        {
            var fi = GetOrderNumberByNow().ToString("D6");
            var strGroupCode = (groupCodes != null && groupCodes.Length > 0) ? string.Join("_", groupCodes) + "_" : "";
            var strCategoryCode = string.IsNullOrEmpty(categoryCode) ? "" : categoryCode + "_";
            var strTypeCode = string.IsNullOrEmpty(typeCode) ? "" : typeCode + "_";


            return string.Format("{0}{1}{2}{3}", strCategoryCode, strTypeCode, strGroupCode, fi).ToUpper();
        }

        public UserViewModel FindByUserName(string userName)
        {
            var cache = new Dictionary<int, UserViewModel>();
            var dapperExecution = BuildByTemplate<UserViewModel>();

            dapperExecution.SqlBuilder.Select("t4.ClassName");
            dapperExecution.SqlBuilder.Select("t2.Id");
            dapperExecution.SqlBuilder.Select("t2.Name");
            dapperExecution.SqlBuilder.Select("t2.FileName");
            dapperExecution.SqlBuilder.Select("t2.FilePath");
            dapperExecution.SqlBuilder.Select("t2.Size");
            dapperExecution.SqlBuilder.Select("t2.Extension");
            dapperExecution.SqlBuilder.Select("t2.RedirectLink");

            dapperExecution.SqlBuilder.Select("t3.Id");
            dapperExecution.SqlBuilder.Select("t3.GroupId");
            dapperExecution.SqlBuilder.Where("t1.UserName = @userName", new { userName });

            dapperExecution.SqlBuilder.LeftJoin("Pictures t2 ON t1.AvatarId = t2.Id AND t2.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin(
                "ApplicationUserUserGroups t3 ON t1.Id = t3.UserId AND t3.IsActive = TRUE AND t3.IsDeleted = FALSE");
            dapperExecution.SqlBuilder.LeftJoin(
                "ApplicationUserClasses t4 ON t1.ClassId = t4.Id AND t4.IsDeleted = FALSE");


            var userModel = dapperExecution.ExecuteQuery<UserViewModel, PictureViewModel, UserUserGroupViewModel>(
                    (user, avatar, group) =>
                    {
                        if (!cache.TryGetValue(user.Id, out var userViewModel))
                        {
                            userViewModel = user;
                            cache.Add(user.Id, userViewModel);
                        }

                        if (group != null && userViewModel.GroupIds.All(e => e != group.GroupId))
                        {
                            userViewModel.GroupIds.Add(group.GroupId);
                        }

                        userViewModel.Avatar = avatar;
                        return userViewModel;
                    }, "Id,Id,Id")
                .Distinct().FirstOrDefault();

            if (userModel != null && !string.IsNullOrWhiteSpace(userModel.Password))
            {
                userModel.Password = userModel.Password.DecryptMD5(_appSettings.MD5CryptoKey)
                    .Substring(userModel.PasswordSalt.Length);
            }

            return userModel;
        }

        public async Task<string> GenerateCustomerCode(string provinceIdentityGuid, string[] industryCodes, string shortName)
        {
            LocationDTO locationModel = null;
            if (!string.IsNullOrEmpty(provinceIdentityGuid))
            {
                locationModel = await _locationGrpcService.GetByLocationId(provinceIdentityGuid);
            }

            string strIndustryCode = (industryCodes != null && industryCodes.Length > 0)
                ? string.Join("_", industryCodes)
                : "";

            var result = string.Format("{0}{1}{2}{3}",
                string.IsNullOrEmpty(locationModel?.CountryCode) ? string.Empty : $"_{locationModel.Code}",
                string.IsNullOrEmpty(locationModel?.Code) ? string.Empty : $"_{locationModel.Code}",
                string.IsNullOrEmpty(strIndustryCode) ? string.Empty : $"_{strIndustryCode}",
                string.IsNullOrEmpty(shortName) ? string.Empty : $"_{shortName}")
            .ToUpper();

            if (string.IsNullOrEmpty(result)) return string.Empty;

            return result.Substring(1);
        }

        public IEnumerable<UserViewSimpleModel> GetAllPaymentTargetByParentId(string parentPaymentTargetId)
        {
            var dapperExecution = BuildByTemplate<UserViewSimpleModel>();
            dapperExecution.SqlBuilder.Select("CONCAT_WS('', t1.FullName, '(', t1.CustomerCode, ')', ', SĐT: ', t1.MobilePhoneNo, ', Đ/c: ', t1.Address) AS `Label`");
            dapperExecution.SqlBuilder.Where("(@parentPaymentTargetId = '' OR (t1.IdentityGuid = @parentPaymentTargetId OR t1.ParentId = @parentPaymentTargetId))", new { parentPaymentTargetId });
            return dapperExecution.ExecuteQuery();
        }

        public string GetCustomerCategoryCode(int? customerCategoryId)
        {
            return WithConnection(conn =>
                conn.QueryFirstOrDefault<string>(
                    "SELECT Code FROM CustomerCategories WHERE IsDeleted = FALSE AND Id = @customerCategoryId", new { customerCategoryId }));
        }
        public string GetCustomerTypeCode(int? customerTypeId)
        {
            return WithConnection(conn =>
                conn.QueryFirstOrDefault<string>(
                    "SELECT Code FROM CustomerTypes WHERE IsDeleted = FALSE AND Id = @customerTypeId", new { customerTypeId }));
        }

        public string GetCustomerGroupCode(int? groupId)
        {
            return WithConnection(conn =>
                conn.QueryFirstOrDefault<string>(
                    "SELECT GroupCode FROM ApplicationUserGroups WHERE IsDeleted = FALSE AND Id = @groupId", new { groupId }));
        }

        public string GenerateCustomerCodeIndividual(int? customerCategoryId = null, int? isEnterPrise = null, int? customerGroupId = null)
        {
            var customerCode = "";
            var customerCategoryCode = "";
            var customerTypeCode = "";
            var customerGroupCode = "";

            var index = WithConnection(conn =>
                conn.QueryFirstOrDefault<int>(
                    "SELECT Id FROM ApplicationUsers WHERE IsDeleted = FALSE ORDER BY Id DESC LIMIT 1"));

            var indexFormat = String.Format("{0, 0:D6}", index + 1);

            customerTypeCode = isEnterPrise == 0 ? "DN" : "CN";
            if (customerCategoryId > 0)
            {
                customerCategoryCode = this.GetCustomerCategoryCode(customerCategoryId);
                customerCode = $"{customerCategoryCode}_{customerTypeCode}_{customerGroupCode}_{indexFormat}";
            }
            
            if (customerGroupId > 0)
            {
                customerGroupCode = this.GetCustomerGroupCode(customerGroupId);
                customerCode = $"{customerCategoryCode}_{customerTypeCode }_{customerGroupCode}_{indexFormat}";
            }

            return $"{(string.IsNullOrEmpty(customerCategoryCode) ? "" :  customerCategoryCode + "_")}{(string.IsNullOrEmpty(customerTypeCode) ? "" : customerTypeCode + "_")}{(string.IsNullOrEmpty(customerGroupCode) ? "" : customerGroupCode + "_")}{indexFormat}";
        }

        public HashSet<string> GetAllUserName()
        {
            var dapperExecution = BuildByTemplateWithoutSelect<string>();
            dapperExecution.SqlBuilder.Select("t1.UserName");
            return dapperExecution.ExecuteQuery().ToHashSet();
        }
    }
}