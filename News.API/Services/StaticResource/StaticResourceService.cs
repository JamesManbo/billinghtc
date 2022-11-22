using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using Picture.StaticResource.API.Proto;

namespace News.API.Services.StaticResource
{
    public interface IStaticResourceService
    {
        Task<PictureGrpcDto> PersistentImage(string temporaryFilePath);
        Task<IEnumerable<PictureGrpcDto>> PersistentImage(string[] temporaryFilePath);
    }

    public class StaticResourceService : GrpcCaller, IStaticResourceService
    {
        public StaticResourceService(IWrappedConfigAndMapper wrappedConfig, ILogger<GrpcCaller> logger)
            : base(wrappedConfig, logger, MicroserviceRouterConfig.GrpcStaticResource)
        {
        }

        public async Task<PictureGrpcDto> PersistentImage(string temporaryFilePath)
        {
            return await Call<PictureGrpcDto>(async channel =>
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
            return await Call<IEnumerable<PictureGrpcDto>>(async channel =>
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