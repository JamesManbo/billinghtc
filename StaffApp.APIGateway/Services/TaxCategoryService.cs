using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.TaxCategoryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface ITaxCategoryService
    {
        Task<IPagedList<TaxCategoryDTO>> GetList(RequestFilterModel filterModel);
        Task<IEnumerable<TaxCategoryDTO>> GetAll();
    }
    public class TaxCategoryService : GrpcCaller, ITaxCategoryService
    {
        private readonly IMapper _mapper;
        public TaxCategoryService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public Task<IEnumerable<TaxCategoryDTO>> GetAll()
        {
            return Call<IEnumerable<TaxCategoryDTO>>(async channel =>
            {
                var client = new TaxCategoryServiceGrpc.TaxCategoryServiceGrpcClient(channel);

                var lstSupportGrpc = await client.GetAllAsync(new Empty());

                return _mapper.Map<IEnumerable<TaxCategoryDTO>>(lstSupportGrpc.TaxCategoryDtos);
            });
        }

        public async Task<IPagedList<TaxCategoryDTO>> GetList(RequestFilterModel filterModel)
        {
            return await Call<IPagedList<TaxCategoryDTO>>(async channel =>
            {
                var client = new TaxCategoryServiceGrpc.TaxCategoryServiceGrpcClient(channel);
                var request = _mapper.Map<RequestFilterGrpc>(filterModel);

                var lstSupportGrpc = await client.GetTaxCategoriesAsync(request);

                return _mapper.Map<IPagedList<TaxCategoryDTO>>(lstSupportGrpc);
            });
        }

    }
}
