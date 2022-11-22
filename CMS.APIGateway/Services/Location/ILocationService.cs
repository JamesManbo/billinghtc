using CMS.APIGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.APIGateway.Services.Location
{
    public interface ILocationService
    {
        Task<LocationDTO> CreateDraftLocation(CreateDraftLocationCommand command);
        Task<MarketAreaDTO> CreateDraftMarketArea(CreateDraftMarketAreaCommand command);
    }
}
