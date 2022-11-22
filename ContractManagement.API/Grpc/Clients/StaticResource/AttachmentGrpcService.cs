using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttchmentFile.StaticResource.API.Proto;
using ContractManagement.Domain.Commands.Commons;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Grpc.Clients.StaticResource
{
    public interface IAttachmentFileResourceGrpcService
    {
        Task<List<CreateUpdateFileCommand>> PersistentFiles(string[] temporaryFilePath);
    }

    public class AttachmentFileResourceGrpcService : GrpcCaller, IAttachmentFileResourceGrpcService
    {
        private readonly IWrappedConfigAndMapper _wrappedConfig;
        public AttachmentFileResourceGrpcService(IWrappedConfigAndMapper wrappedConfig, ILogger<GrpcCaller> logger)
            : base(wrappedConfig, logger, MicroserviceRouterConfig.GrpcStaticResource)
        {
            this._wrappedConfig = wrappedConfig;
        }

        public async Task<List<CreateUpdateFileCommand>> PersistentFiles(string[] temporaryFilePath)
        {
            return await CallAsync(async channel =>
            {
                if (temporaryFilePath == null || 
                    temporaryFilePath.Length == 0 ||
                    temporaryFilePath.All(string.IsNullOrWhiteSpace))
                    return Enumerable.Empty<CreateUpdateFileCommand>().ToList();

                var client = new AttachmentFileResourceGrpc.AttachmentFileResourceGrpcClient(channel);
                var clientRequest = new TemporaryFilePathGrpcRequest();
                clientRequest.TemporaryFilePath.Add(temporaryFilePath.Where(t => !string.IsNullOrWhiteSpace(t)));
                var result = await client.StoreAndSaveFilesAsync(clientRequest);
                if (result == null || result.FileDtos == null || result.FileDtos.Count == 0) return default;
                return result.FileDtos.MapTo<List<CreateUpdateFileCommand>>(this._wrappedConfig.MapperConfig);
            });
        }
    }
}
