using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using Picture.StaticResource.API.Proto;

namespace ApplicationUserIdentity.API.Services.GRPC.StaticResource
{
    public interface IStaticResourceService
    {
        Task<PictureGrpcDto> PersistentImage(string temporaryFilePath);
        Task<IEnumerable<PictureGrpcDto>> PersistentImage(string[] temporaryFilePath);
    }

    public class StaticResourceService : Grpc.Clients.GrpcCaller, IStaticResourceService
    {
        public StaticResourceService(ILogger<StaticResourceService> logger)
            : base(logger, MicroserviceRouterConfig.GrpcStaticResource)
        {
        }

        public async Task<PictureGrpcDto> PersistentImage(string temporaryFilePath)
        {
            return await CallAsync(async channel =>
            {
                var client = new PictureResourceGrpc.PictureResourceGrpcClient(channel);
                var clientRequest = new TemporaryPictureGrpcRequest();
                clientRequest.TemporaryFilePath.Add(temporaryFilePath);
                var result = await client.StoreAndSaveImagesAsync(clientRequest);
                return result.PictureDtos.FirstOrDefault();
            });
        }

        public async Task<IEnumerable<PictureGrpcDto>> PersistentImage(string[] temporaryFilePath)
        {
            return await CallAsync<IEnumerable<PictureGrpcDto>>(async channel =>
            {
                var client = new PictureResourceGrpc.PictureResourceGrpcClient(channel);
                var clientRequest = new TemporaryPictureGrpcRequest();
                clientRequest.TemporaryFilePath.Add(temporaryFilePath);
                var result = await client.StoreAndSaveImagesAsync(clientRequest);
                return result.PictureDtos;
            });
        }
    }
}