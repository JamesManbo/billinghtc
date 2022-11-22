using System.Threading.Tasks;
using AutoMapper;
using Global.Models.Filter;
using Grpc.Core;
using Location.API.Reponsitory;
using Location.API.Protos;

namespace Location.API.Grpc
{
    public interface ISupportLocationService
    {

    }
    public class SupportLocationService : SupportLocationServiceGrpc.SupportLocationServiceGrpcBase, ISupportLocationService
    {
        private ISupportLocationQueries _locationQueries;
        private IMapper _mapper;

        public SupportLocationService(ISupportLocationQueries locationQueries, IMapper mapper)
        {
            _locationQueries = locationQueries;
            _mapper = mapper;
        }

        public override async Task<SupportLocationPageListGrpcDTO> GetSupportLocation(RequestFilterGrpc request, ServerCallContext context)
        {
            var lstProject = await _locationQueries.GetList(_mapper.Map<RequestFilterModel>(request));
            return await Task.FromResult(_mapper.Map<SupportLocationPageListGrpcDTO>(lstProject));
        }
    }
}
