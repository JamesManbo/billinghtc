using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using GenericRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace ApplicationUserIdentity.API.Infrastructure.Queries
{
    public interface IUserClassQueries : IQueryRepository
    {
        IPagedList<UserClassViewModel> GetList(RequestFilterModel requestFilterModel);
        IPagedList<UserClassViewModel> GetAllList();
        IEnumerable<SelectionItem> GetSelectionList();
        UserClassViewModel Find(int id);
        bool CheckExistName(int id, string name);
        bool CheckExistCode(int id, string code);
        int GetMinClassId();
    }

    public class UserClassQueries : QueryRepository<ApplicationUserClass, int>, IUserClassQueries
    {
        private readonly ApplicationUserDbContext _context;
        public UserClassQueries(ApplicationUserDbContext context) : base(context)
        {
            _context = context;
        }

        public IPagedList<UserClassViewModel> GetList(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<UserClassViewModel>(requestFilterModel);
            return dapperExecution.ExecutePaginateQuery();
           
        }
        public IPagedList<UserClassViewModel> GetAllList()
        {
            var dapperExecution = BuildByTemplate<UserClassViewModel>();
            return dapperExecution.ExecutePaginateQuery();
            
        }

        public IEnumerable<SelectionItem> GetSelectionList()
        {
            var dapperExecution = BuildByTemplateWithoutSelect<SelectionItem>();
            dapperExecution.SqlBuilder.Select("t1.ClassName AS `Text`");
            dapperExecution.SqlBuilder.Select("t1.ClassCode AS `Code`");
            dapperExecution.SqlBuilder.Select("t1.Id AS `Value`");
            return dapperExecution.ExecuteQuery();
        }

        public UserClassViewModel Find(int id)
        {
            var dapperExecution = BuildByTemplate<UserClassViewModel>();
            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });
            return dapperExecution.ExecuteScalarQuery();
        }

        public bool CheckExistName(int id, string name)
        {
            var dapperExecution = BuildByTemplate<UserClassViewModel>();
            dapperExecution.SqlBuilder.Where("t1.ClassName = @name", new { name });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public bool CheckExistCode(int id, string code)
        {
            var dapperExecution = BuildByTemplate<UserClassViewModel>();
            dapperExecution.SqlBuilder.Where("t1.ClassCode = @code", new { code });
            dapperExecution.SqlBuilder.Where("t1.Id <> @id", new { id });
            var existOb = dapperExecution.ExecuteQuery();
            if (existOb != null && existOb.GetEnumerator().MoveNext()) return true;
            return false;
        }

        public int GetMinClassId() //Lấy ra hạng nhỏ nhất
        {
            var minConditionStartPoint = _context.ApplicationUserClass.Select(x => x.ConditionStartPoint).Min();
            var minClassId = _context.ApplicationUserClass.Where(x => x.IsDeleted == false && x.ConditionStartPoint == minConditionStartPoint).Select(t => t.Id).FirstOrDefault();
            return minClassId;
        }
    }
}
