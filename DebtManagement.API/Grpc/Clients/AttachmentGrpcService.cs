using System.Collections.Generic;
using System.Threading.Tasks;
using AttchmentFile.StaticResource.API.Proto;
using AutoMapper;
using DebtManagement.Domain.Commands.Commons;
using GenericRepository.Configurations;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;

namespace DebtManagement.API.Grpc.Clients.StaticResource
{
    public interface IAttachmentFileResourceGrpcService
    {
        Task<List<AttachmentFileCommand>> PersistentFiles(string[] temporaryFilePath);
    }

    public class AttachmentFileResourceGrpcService : GrpcCaller, IAttachmentFileResourceGrpcService
    {
        private readonly IMapper _mapper;
        public AttachmentFileResourceGrpcService(ILogger<GrpcCaller> logger,
            IMapper mapper)
            : base(logger, MicroserviceRouterConfig.GrpcStaticResource)
        {
            this._mapper = mapper;
        }

        public async Task<List<AttachmentFileCommand>> PersistentFiles(string[] temporaryFilePath)
        {
            return await CallAsync(async channel =>
            {
                var client = new AttachmentFileResourceGrpc.AttachmentFileResourceGrpcClient(channel);
                var clientRequest = new TemporaryFilePathGrpcRequest();
                clientRequest.TemporaryFilePath.Add(temporaryFilePath);
                var result = await client.StoreAndSaveFilesAsync(clientRequest);
                return this._mapper.Map<List<AttachmentFileCommand>>(result.FileDtos);
            });
        }
    }
}
