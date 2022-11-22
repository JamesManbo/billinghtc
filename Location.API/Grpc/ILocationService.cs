using Location.API.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Location.API.Grpc
{
    public interface ILocationService
    {
        Task<LocationGrpcDTO> Create(CreateLocationGrpcCommand createLocationGrpcCommand);
    }
}
