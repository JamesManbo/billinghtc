using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Models;
using ContractManagement.Utility;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IChannelGroupQueries : IQueryRepository
    {
        bool CheckExistCode(int id, string code);
        bool CheckExistName(int id, string name);
        IEnumerable<ChannelGroupDTO> GetAll(RequestFilterModel requestFilterModel = null);
        ChannelGroupDTO Find(int id);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel);
        IPagedList<ChannelGroupDTO> GetList(RequestFilterModel filterModel);
        string GenerateChannelGroupName(string serviceCode, string contractorIdGuid, string contractorName, int channelGroupType);
        IEnumerable<int> CountChannelGroups(int channelGroupType, string contractorIdGuid);

    }
    public class ChannelGroupQueries : QueryRepository<ChannelGroups, int>, IChannelGroupQueries
    {
        private readonly ContractDbContext _context;
        public ChannelGroupQueries(ContractDbContext context) : base(context)
        {
            _context = context;
        }

        public bool CheckExistCode(int id, string code)
        {
            var dapperExecution = BuildByTemplate<ChannelGroupDTO>();
            dapperExecution.SqlBuilder.Where("t1.ChannelGroupCode = @code", new { code });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public bool CheckExistName(int id, string name)
        {
            var dapperExecution = BuildByTemplate<ChannelGroupDTO>();
            dapperExecution.SqlBuilder.Where("t1.ChannelGroupName = @name", new { name });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public IEnumerable<ChannelGroupDTO> GetAll(RequestFilterModel requestFilterModel = null)
        {
            var dapperExecution = requestFilterModel != null
                ? BuildByTemplate<ChannelGroupDTO>(requestFilterModel)
                : BuildByTemplate<ChannelGroupDTO>();

            return dapperExecution.ExecuteQuery();
        }

        public ChannelGroupDTO Find(int id)
        {
            var cached = new Dictionary<int, ChannelGroupDTO>();
            var dapperExecution = BuildByTemplate<ChannelGroupDTO>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.ChannelGroupName AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                   .OrWhere("t1.ContractorIdGuid LIKE @name", new { name = $"%{filterModel.Keywords}%" });
            }

            return dapperExecution.ExecuteQuery();
        }

        public IPagedList<ChannelGroupDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<ChannelGroupDTO>(filterModel);
            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                   .OrWhere("t1.ChannelGroupCode LIKE @name", new { name = $"%{filterModel.Keywords}%" })
                   .OrWhere("t1.ChannelGroupCode LIKE @name", new { name = $"%{filterModel.Keywords}%" })
                   .OrWhere("t1.ContractorIdGuid LIKE @name", new { name = $"%{filterModel.Keywords}%" });
            }

            return dapperExecution.ExecutePaginateQuery();
        }

        //public IEnumerable<string> FindByIds(int[] ids)
        //{
        //    var dapperExecution = BuildTemplateWithoutSelect<string>();
        //    dapperExecution.SqlBuilder.Select("t1.ServiceCode");
        //    dapperExecution.SqlBuilder.Where("t1.Id IN @ids", new { ids });
        //    return dapperExecution.ExecuteQuery();
        //}

        public IEnumerable<int> CountChannelGroups(int channelGroupType, string contractorIdGuid)
        {
            var dapperExecution = BuildByTemplate<int>();
            dapperExecution.SqlBuilder.Select("COUNT(t1.Id)");
            dapperExecution.SqlBuilder.Where("t1.ChannelGroupType = @channelGroupType", new { channelGroupType = channelGroupType });
            dapperExecution.SqlBuilder.Where("t1.ContractorIdGuid = @contractorIdGuid", new { contractorIdGuid = contractorIdGuid });
            dapperExecution.SqlBuilder.Where("t1.IsDeleted = FALSE");
            return dapperExecution.ExecuteQuery();
        }

        public string GenerateChannelGroupName(string serviceCode, string contractorIdGuid, string contractorName, int channelGroupType)
        {
            //đếm xem có bao nhiêu chùm kênh theo KH và loại chùm kênh
            string GU = channelGroupType == 1 ? "GU" : "G"; //là Uplink
            string MMYY = DateTime.Now.ToString("MMyy");
            var coutChannelGroups = CountChannelGroups(channelGroupType, contractorIdGuid).FirstOrDefault();
            string channelGroupName = $"{serviceCode}_{contractorName.GetAcronym()}_{MMYY}_{GU}{coutChannelGroups + 1}";
            return channelGroupName;
        }
    }
}
