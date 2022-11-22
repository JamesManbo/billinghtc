using System.Collections.Generic;
using CachingLayer.Interceptor;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Models;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IServicesQueries : IQueryRepository
    {
        IPagedList<ServiceDTO> GetList(RequestFilterModel requestFilterModel);
        [Cache]
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel requestFilterModel);
        [Cache]
        IEnumerable<ServiceDTO> GetAll(RequestFilterModel requestFilterModel = null);
        IEnumerable<ServiceDTO> GetAllNotAvailableStartAndEndPoint();
        ServiceDTO Find(int id);
        bool CheckExistName(int id, string name);
        bool CheckExistCode(int id, string code);
        IEnumerable<string> FindByIds(int[] ids);
    }

    public class ServicesQueries : QueryRepository<Service, int>, IServicesQueries
    {
        public ServicesQueries(ContractDbContext context) : base(context)
        {
        }

        public bool CheckExistCode(int id, string code)
        {
            var dapperExecution = BuildByTemplate<ServiceDTO>();
            dapperExecution.SqlBuilder.Where("t1.ServiceCode = @code", new { code });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public bool CheckExistName(int id, string name)
        {
            var dapperExecution = BuildByTemplate<ServiceDTO>();
            dapperExecution.SqlBuilder.Where("t1.ServiceName = @name", new { name });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public IEnumerable<ServiceDTO> GetAll(RequestFilterModel requestFilterModel = null)
        {
            if (requestFilterModel != null)
            {
                requestFilterModel.Paging = false;
            }
            var dapperExecution = BuildByTemplate<ServiceDTO>(requestFilterModel);

            return dapperExecution.ExecuteQuery();
        }
        public IEnumerable<ServiceDTO> GetAllNotAvailableStartAndEndPoint()
        {
            var dapperExecution = BuildByTemplate<ServiceDTO>();

            dapperExecution.SqlBuilder.Where("t1.HasStartAndEndPoint = FALSE");

            return dapperExecution.ExecuteQuery();
        }

        public ServiceDTO Find(int id)
        {
            var dapperExecution = BuildByTemplate<ServiceDTO>();
            dapperExecution.SqlBuilder.Select("pic.*");
            dapperExecution.SqlBuilder.LeftJoin("Pictures pic ON pic.Id = t1.AvatarId AND pic.IsDeleted = 0");
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery<ServiceDTO, PictureDTO>((service, picture) =>
            {
                service.Avatar = picture;
                return service;
            }, "Id");
        }

        public IPagedList<ServiceDTO> GetList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<ServiceDTO>(requestFilterModel);
            dapperExecution.SqlBuilder.LeftJoin("ServiceGroups t2 ON t1.GroupId = t2.Id");
            dapperExecution.SqlBuilder.LeftJoin("Pictures t3 ON t3.Id = t1.AvatarId AND t3.IsDeleted = FALSE");

            dapperExecution.SqlBuilder.Select("t2.GroupName");

            dapperExecution.SqlBuilder.Select("t3.Id");
            dapperExecution.SqlBuilder.Select("t3.Name");
            dapperExecution.SqlBuilder.Select("t3.FileName");
            dapperExecution.SqlBuilder.Select("t3.FilePath");
            dapperExecution.SqlBuilder.Select("t3.Size");
            dapperExecution.SqlBuilder.Select("t3.RedirectLink");

            var serviceModels = dapperExecution.ExecutePaginateQuery<ServiceDTO, PictureDTO>(
            (service, avatar) =>
            {
                if (service != null && avatar != null)
                {
                    service.Avatar = avatar;
                }

                return service;
            }, "Id,Id");

            return serviceModels;
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.ServiceName AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            return dapperExecution.ExecuteQuery();
        }

        public IEnumerable<string> FindByIds(int[] ids)
        {
            var dapperExecution = BuildByTemplateWithoutSelect<string>();
            dapperExecution.SqlBuilder.Select("t1.ServiceCode");
            dapperExecution.SqlBuilder.Where("t1.Id IN @ids", new { ids });
            return dapperExecution.ExecuteQuery();
        }
    }
}