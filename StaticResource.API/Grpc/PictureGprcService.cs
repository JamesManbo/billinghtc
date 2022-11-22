using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Picture.StaticResource.API.Proto;
using StaticResource.API.Helper;

namespace StaticResource.API.Grpc
{
    public interface IPictureGprcService
    {
        Task<PictureGrpcDtoResponse>
            StoreAndSaveImages(TemporaryPictureGrpcRequest request, ServerCallContext context);
    }

    public class PictureGprcService : PictureResourceGrpc.PictureResourceGrpcBase, IPictureGprcService
    {
        private readonly IMapper _mapper;
        private readonly ImageProcessorHelper _imageProcessorHelper;

        public PictureGprcService(ImageProcessorHelper imageProcessorHelper,
            IMapper mapper)
        {
            _imageProcessorHelper = imageProcessorHelper;
            _mapper = mapper;
        }

        public override Task<PictureGrpcDtoResponse> StoreAndSaveImages(TemporaryPictureGrpcRequest request, ServerCallContext context)
        {
            var result = new PictureGrpcDtoResponse();
            foreach (var path in request.TemporaryFilePath)
            {
                var storedImage = _imageProcessorHelper.Save(path);
                if (storedImage == null) continue;

                result.PictureDtos.Add(_mapper.Map<PictureGrpcDto>(storedImage));
            }

            return Task.FromResult(result);
        }

    }
}