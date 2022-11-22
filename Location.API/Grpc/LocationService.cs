using AutoMapper;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Location.API.Commands;
using Location.API.Proto;
using Location.API.Reponsitory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Location.API.Grpc
{
    public class LocationService : LocationGrpc.LocationGrpcBase, ILocationService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILocationQueries _locationQueries;

        public LocationService(IMediator mediator, IMapper mapper, ILocationQueries locationQueries)
        {
            _mediator = mediator;
            _mapper = mapper;
            _locationQueries = locationQueries;
        }

        public async Task<LocationGrpcDTO> Create(CreateLocationGrpcCommand createLocationGrpcCommand)
        {
            var command = _mapper.Map<CULocationCommand>(createLocationGrpcCommand);
            var response = await _mediator.Send(command);
            return _mapper.Map<LocationGrpcDTO>(response);
        }

        public override async Task<LocationGrpcDTO> CreateDraftLocation(CreateLocationGrpcCommand request, ServerCallContext context)
        {
            var command = _mapper.Map<CULocationCommand>(request);
            var response = await _mediator.Send(command);
            return _mapper.Map<LocationGrpcDTO>(response);
        }

        public override async Task<ListLocationGrpcDTO> GetListByLevel(Int32Value request, ServerCallContext context)
        {
            var locations = await _locationQueries.GetLocationByLevel(request.Value);
            var result = new ListLocationGrpcDTO();
            if (locations != null && locations.Any())
            {
                foreach (var item in locations)
                {
                    result.LocationGrpcDTOs.Add(_mapper.Map<LocationGrpcDTO>(item));
                }
            }

            return result;
        }

        public override async Task<LocationGrpcDTO> GetByLocationId(StringValue request, ServerCallContext context)
        {
            var location = await _locationQueries.GetByLocationId(request.Value);
            return _mapper.Map<LocationGrpcDTO>(location);
        }
        public override async Task<LocationGrpcDTO> GetByLocationCode(StringValue request, ServerCallContext context)
        {
            var location = await _locationQueries.GetByLocationCode(request.Value);
            return _mapper.Map<LocationGrpcDTO>(location);
        }
    }
}
