using Dapper;
using GenericRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using GenericRepository.DapperSqlBuilder;
using System.Threading.Tasks;

namespace GenericRepository
{
    public interface IQueryRepository
    {
        void RestrictByOrganization(int level = 0);
        void RestrictByRole();
        TResult GetScalarByTemplate<TResult>(SqlBuilder.Template sqlBuilderTemplate);
        TResult GetScalarByTemplate<TFirst, TSecond, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TResult> resp, string splitOnColumn = null);
        TResult GetScalarByTemplate<TFirst, TSecond, TThird, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TResult> resp, string splitOnColumn = null);
        TResult GetScalarByTemplate<TFirst, TSecond, TThird, TFourth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TResult> resp, string splitOnColumn = null);
        TResult GetScalarByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> resp, string splitOnColumn = null);
        TResult GetScalarByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> resp, string splitOnColumn = null);
        TResult GetScalarByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> resp, string splitOnColumn = null);
        IEnumerable<TResult> GetByTemplate<TResult>(SqlBuilder.Template sqlBuilderTemplate);
        IEnumerable<TResult> GetByTemplate<TResult>(SqlBuilder.Template sqlBuilderTemplate, Type[] types, Func<object[], TResult> map, string splitOnColumn = "Id");
        IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TResult> resp, string splitOnColumn = null);
        IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TThird, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TResult> resp, string splitOnColumn = null);
        IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TThird, TFourth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TResult> resp, string splitOnColumn = null);
        IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> resp, string splitOnColumn = null);
        IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> resp, string splitOnColumn = null);
        IEnumerable<TResult> GetByTemplate<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> resp, string splitOnColumn = null);
        
        Task<TResult> GetScalarByTemplateAsync<TResult>(SqlBuilder.Template sqlBuilderTemplate);
        Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TResult> resp, string splitOnColumn = null);
        Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TThird, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TResult> resp, string splitOnColumn = null);
        Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TThird, TFourth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TResult> resp, string splitOnColumn = null);
        Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> resp, string splitOnColumn = null);
        Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> resp, string splitOnColumn = null);
        Task<TResult> GetScalarByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> resp, string splitOnColumn = null);
        Task<IEnumerable<TResult>> GetByTemplateAsync<TResult>(SqlBuilder.Template sqlBuilderTemplate);
        Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TResult> resp, string splitOnColumn = null);
        Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TThird, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TResult> resp, string splitOnColumn = null);
        Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TThird, TFourth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TResult> resp, string splitOnColumn = null);
        Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult> resp, string splitOnColumn = null);
        Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult> resp, string splitOnColumn = null);
        Task<IEnumerable<TResult>> GetByTemplateAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(SqlBuilder.Template sqlBuilderTemplate, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult> resp, string splitOnColumn = null);
        
    }
}
