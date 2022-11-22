using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using Picture.StaticResource.API.Proto;

namespace ContractManagement.API.Grpc.Clients.StaticResource
{
    public interface IPictureResourceService
    {
        Task<PictureGrpcDto> PersistentImage(string temporaryFilePath);
        Task<IEnumerable<PictureGrpcDto>> PersistentImage(string[] temporaryFilePath);
    }

    public class PictureGrpcService : GrpcCaller, IPictureResourceService
    {
        public PictureGrpcService(IWrappedConfigAndMapper wrappedConfig, ILogger<GrpcCaller> logger)
            : base(wrappedConfig, logger, MicroserviceRouterConfig.GrpcStaticResource)
        {
        }

        public async Task<PictureGrpcDto> PersistentImage(string temporaryFilePath)
        {
            return await CallAsync<PictureGrpcDto>(async channel =>
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
