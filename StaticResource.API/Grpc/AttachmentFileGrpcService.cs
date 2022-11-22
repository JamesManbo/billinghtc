using System.Threading.Tasks;
using AttchmentFile.StaticResource.API.Proto;
using AutoMapper;
using Grpc.Core;
using StaticResource.API.Helper;

namespace StaticResource.API.Grpc
{
    public interface IAttachmentFileGrpcService
    {
        Task<FileGrpcDtoResponse> StoreAndSaveFiles(TemporaryFilePathGrpcRequest request, ServerCallContext context);
    }

    public class AttachmentFileGprcService : AttachmentFileResourceGrpc.AttachmentFileResourceGrpcBase, IAttachmentFileGrpcService
    {
        private readonly IMapper _mapper;
        private readonly FileProcessorHelper _fileProcessor;

        public AttachmentFileGprcService(IMapper mapper, FileProcessorHelper fileProcessor)
        {
            _mapper = mapper;
            _fileProcessor = fileProcessor;
        }
        
        public override Task<FileGrpcDtoResponse> StoreAndSaveFiles(TemporaryFilePathGrpcRequest request, ServerCallContext context)
        {
            var result = new FileGrpcDtoResponse();
            foreach (var path in request.TemporaryFilePath)
            {
                var storedImage = _fileProcessor.Save(path);
                if (storedImage == null) continue;

                result.FileDtos.Add(_mapper.Map<AttachmentFileGrpcDto>(storedImage));
            }
            return Task.FromResult(result);
        }
    }
}