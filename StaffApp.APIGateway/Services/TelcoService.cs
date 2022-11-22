using AutoMapper;
using ContractManagement.API.Protos;
using Global.Configs.MicroserviceRouterConfig;
using Global.Models.Filter;
using Global.Models.PagedList;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.ServicesModels;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Services
{
    public interface ITelcoService
    {
        Task<IPagedList<ServiceDTO>> GetList(RequestFilterModel filterModel);
        Task<ServiceDTO> GetDetail(int id);
    }
    public class TelcoService : GrpcCaller, ITelcoService
    {
        private readonly IMapper _mapper;
        public TelcoService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcContract)
        {
            _mapper = mapper;
        }

        public async Task<ServiceDTO> GetDetail(int id)
        {
            return await Call<ServiceDTO>(async channel =>
            {
                var client = new ServiceGrpc.ServiceGrpcClient(channel);

                var serviceGrpc = await client.GetDetailAsync(new Int32Value {Value = id });
                //var totalRecords = filterModel.Paging ? _queryRepository.GetScalarByTemplate<int>(ExecutionCountingTemplate) : subSetResult.Count();

                return _mapper.Map<ServiceDTO>(serviceGrpc);
            });
        }

        public async Task<IPagedList<ServiceDTO>> GetList(RequestFilterModel filterModel)
        {
            return await Call<IPagedList<ServiceDTO>>(async channel =>
            {
                var client = new ServiceGrpc.ServiceGrpcClient(channel);
                var request = _mapper.Map<RequestFilterGrpc>(filterModel);

                var lstServiceGrpc =  await client.GetServicesAsync(request);
                //var totalRecords = filterModel.Paging ? _queryRepository.GetScalarByTemplate<int>(ExecutionCountingTemplate) : subSetResult.Count();

                return _mapper.Map<IPagedList<ServiceDTO>>(lstServiceGrpc);
            });
        }
    }
}
