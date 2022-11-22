using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Infrastructure.Queries;
using Global.Models.Filter;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ContractManagement.API.Grpc.Servers
{
    public class TaxCategoryService : TaxCategoryServiceGrpc.TaxCategoryServiceGrpcBase
    {
        private ITaxCategoryQueries _taxCategory;
        private IMapper _mapper;

        public TaxCategoryService(ITaxCategoryQueries taxCategory, IMapper mapper)
        {
            _taxCategory = taxCategory;
            _mapper = mapper;
        }


        public override Task<TaxCategoryListGrpcDTO> GetAll(Empty request, ServerCallContext context)
        {
            var result = new TaxCategoryListGrpcDTO();
            var taxCategories = _taxCategory.GetAll();
            result.TaxCategoryDtos.AddRange(_mapper.Map<IEnumerable<TaxCategoryGrpcDTO>>(taxCategories));
            return Task.FromResult(result);
        }

        public override Task<TaxCategoryPageListGrpcDTO> GetTaxCategories(RequestFilterGrpc request, ServerCallContext context)
        {
            var lstProject = _taxCategory.GetList(_mapper.Map<RequestFilterModel>(request));
            return Task.FromResult(_mapper.Map<TaxCategoryPageListGrpcDTO>(lstProject));
        }
    }
}
