using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Services.GRPC.Clients;
using Global.Models.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.NotificationModels
{
    public class NotificationHelper
    {
        public static async Task<IPagedList<UserViewModel>> GetListApplicationUser(UserRequestFilterModel filterModel, IContractorGrpcService _contractorGrpcService, IUserQueries _accountQueries)
        {
            if (filterModel != null && filterModel.PropertyFilterModels != null && filterModel.PropertyFilterModels.Count > 0)
            {
                var marketAreaIds = "";
                var projectIds = "";
                if (filterModel.PropertyFilterModels.Any(f => f.Field == "MarketArea" && f.Operator == "eq"))
                {
                    marketAreaIds = string.Join(",", filterModel.PropertyFilterModels.Where(f => f.Field == "MarketArea" && f.Operator == "eq").Select(s => s.FilterValue).ToList());
                    filterModel.MarketAreaIds = marketAreaIds;
                }
                if (filterModel.PropertyFilterModels.Any(f => f.Field == "Project" && f.Operator == "eq"))
                {
                    projectIds = string.Join(",", filterModel.PropertyFilterModels.Where(f => f.Field == "Project" && f.Operator == "eq").Select(s => s.FilterValue).ToList());
                    filterModel.ProjectIds = projectIds;
                }

                if (!string.IsNullOrEmpty(marketAreaIds) || !string.IsNullOrEmpty(projectIds))
                {
                    var contractors = await _contractorGrpcService.GetListByMarketIdsProjectIds(new RequestGetByMarketAreaIdsProjectIds()
                    {
                        MarketAreaIds = filterModel.MarketAreaIds,
                        ProjectIds = filterModel.ProjectIds
                    });

                    if (contractors != null && contractors.TotalItemCount > 0)
                    {
                        filterModel.IdentityGuids = string.Join(",", contractors.Subset.Select(s => s.IdentityGuid).ToArray());

                    }
                }
            }
            return _accountQueries.GetList(filterModel);
        }
    }
}
