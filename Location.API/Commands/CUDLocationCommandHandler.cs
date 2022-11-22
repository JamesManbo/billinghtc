using AutoMapper;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Location.API.Model;
using Location.API.Reponsitory;

using MediatR;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Location.API.Commands
{
    public class CUDLocationCommandHandler : IRequestHandler<CULocationCommand, LocationDTO>
    {
        private readonly ILocationQueries _locationQueries;

        private readonly IMapper _mapper;

        public CUDLocationCommandHandler(ILocationQueries locationQueries, IMapper mapper)
        {
            _locationQueries = locationQueries;
            _mapper = mapper;
        }

        public async Task<LocationDTO> Handle(CULocationCommand request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.ParentId))
            {
                if (request.Code.Equals("VN", StringComparison.OrdinalIgnoreCase))
                {
                    request.Path = "root";
                }
                else
                {
                    request.Path = request.Code?.ToLower();
                }
                request.Level = 0;
            }
            else
            {
                var parent = await _locationQueries.GetByLocationId(request.ParentId);
                if (parent != null)
                {
                    request.Path = $"{parent.Path}/{request.Code}";
                    request.Level = parent.Level + 1;
                    request.ParentName = parent.Name;
                }
            }

            var location = _mapper.Map<LocationModel>(request);
            location.Active = true;
            location.IsDeleted = false;

            if (string.IsNullOrEmpty(request.Id))
            {
                location.LocationId = Guid.NewGuid().ToString();
                await _locationQueries.CreateLocation(location);
            }
            else
            {
                await _locationQueries.UpdateLocation(location);
            }

            var locationAdded = await _locationQueries.GetById(location.Id);

            return _mapper.Map<LocationDTO>(locationAdded);
        }
    }
}
