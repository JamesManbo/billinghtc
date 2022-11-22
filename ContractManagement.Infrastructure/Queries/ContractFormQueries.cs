using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Models;
using GenericRepository;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContractManagement.Infrastructure.Queries
{
    public interface IContractFormQueries : IQueryRepository
    {
        IEnumerable<SelectionItem> Autocomplete(RequestFilterModel requestFilterModel);
        IPagedList<ContractFormDTO> GetList(RequestFilterModel filterModel);
        IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel);
        ContractFormDTO Find(int id);
    }

    public class ContractFormQueries : QueryRepository<ContractForm, int>, IContractFormQueries
    {
        public ContractFormQueries(ContractDbContext context) : base(context)
        {
        }

        public IEnumerable<SelectionItem> Autocomplete(RequestFilterModel requestFilterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(requestFilterModel);
            dapperExecution.SqlBuilder.Select("t1.Name AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");
            if (!string.IsNullOrWhiteSpace(requestFilterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.Name LIKE @name", new { name = $"%{requestFilterModel.Keywords}%" })
                    .OrWhere("t1.Content LIKE @name", new { name = $"%{requestFilterModel.Keywords}%" });
            }

            var result = dapperExecution.ExecuteQuery().ToList();
            if (requestFilterModel.PropertyFilterModels.Any(a => a.Field == "value"))
            {
                var propertyFilter = requestFilterModel.PropertyFilterModels
                    .First(p => p.Field == "value");
                dapperExecution.SqlBuilder.AppendPredicate<string>("t1.Id", propertyFilter);
                var contractForm = dapperExecution.ExecuteQuery().FirstOrDefault();
                if (contractForm != null)
                {
                    result.Add(contractForm);
                }
            }

            return result;
        }

        public ContractFormDTO Find(int id)
        {
            var cached = new Dictionary<int, ContractFormDTO>();

            var dapperExecution = BuildByTemplate<ContractFormDTO>();
            dapperExecution.SqlBuilder.Select("pic.*");
            dapperExecution.SqlBuilder.LeftJoin("Pictures pic ON pic.Id = t1.DigitalSignatureId");

            dapperExecution.SqlBuilder.Where("t1.Id = @id", new { id });

            return dapperExecution.ExecuteScalarQuery<ContractFormDTO, PictureDTO>((contractFormDTO, pictureDTO) =>
            {
                if (!cached.TryGetValue(contractFormDTO.Id, out var contractForm))
                {
                    contractForm = contractFormDTO;
                    contractForm.DigitalSignature = pictureDTO;
                    cached.Add(contractFormDTO.Id, contractFormDTO);
                }
                return contractForm;
            }, "Id");
        }

        public IPagedList<ContractFormDTO> GetList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<ContractFormDTO>(filterModel);
            dapperExecution.SqlBuilder.Select("pic.*");
            dapperExecution.SqlBuilder.LeftJoin("Pictures pic ON pic.Id = t1.DigitalSignatureId AND pic.IsDeleted = 0");
            if (!string.IsNullOrWhiteSpace(filterModel.Keywords))
            {
                dapperExecution.SqlBuilder
                    .OrWhere("t1.Name LIKE @keywords", new { keywords = $"%{filterModel.Keywords}%" });
            }

            return dapperExecution.ExecutePaginateQuery<ContractFormDTO, PictureDTO>(
            (contractFormDTO, pictureDTO) =>
            {
                if (contractFormDTO != null && pictureDTO != null)
                {
                    contractFormDTO.DigitalSignature = pictureDTO;
                }

                return contractFormDTO;
            }, "Id");
        }

        public IEnumerable<SelectionItem> GetSelectionList(RequestFilterModel filterModel)
        {
            var dapperExecution = BuildByTemplate<SelectionItem>(filterModel);
            dapperExecution.SqlBuilder.Select("t1.Name AS Text");
            dapperExecution.SqlBuilder.Select("t1.Id AS Value");

            return dapperExecution.ExecuteQuery();
        }
    }
}
