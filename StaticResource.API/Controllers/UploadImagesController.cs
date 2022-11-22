using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global.Configs.ResourceConfig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using StaticResource.API.Helper;
using StaticResource.API.Models;

namespace StaticResource.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UploadImagesController : ControllerBase
    {
        private readonly ImageProcessorHelper _imageProcessorHelper;

        private AppSettings _appSettings;
        // Get the default form options so that we can use them to set the default limits for
        // request body data
        private readonly FormOptions _defaultFormOptions;

        public UploadImagesController(
            IOptions<AppSettings> appSettingOptions)
        {
            _appSettings = appSettingOptions.Value;
            _imageProcessorHelper = new ImageProcessorHelper();
            _defaultFormOptions = new FormOptions();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);

            var requestReader = new MultipartReader(boundary, HttpContext.Request.Body);
            int maxContentLength = 1024 * 10; //Size = 10 MB

            var requestSection = await requestReader.ReadNextSectionAsync();
            var allowedImageFileFormat = _appSettings.AllowedImageFileFormat.Split(',');

            var result = new List<TemporaryFileModel>();
            while (requestSection != null)
            {
                var hasContentDispositionHeader
                     = ContentDispositionHeaderValue.TryParse(requestSection.ContentDisposition, out var contentDisposition);
                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        var random = new Random();
                        var randomTempFileName =
                            $"{DateTime.Now:yyMMddHHmmssfff}{random.Next(1000)}_{contentDisposition.FileName.Value}";
                        var relativeFilePath = Path.Combine(ResourceGlobalConfigs.ResourceTempFolder, randomTempFileName);
                        var absoluteFilePath = Path.Combine(Program.RootDirectory, relativeFilePath);
                        using (var targetStream = System.IO.File.Create(absoluteFilePath))
                        {
                            var fileExtension = contentDisposition
                                .FileName
                                .Substring(contentDisposition.FileName.Value.LastIndexOf('.'))
                                .ToLower();
                            if (!allowedImageFileFormat.Contains(fileExtension))
                            {
                                return BadRequest("Tệp đẩy lên có định dạng không được cho phép"); 
                            }
                            if (contentDisposition.Size > maxContentLength)
                            {
                                return BadRequest("Tệp đẩy lên vượt quá dung lượng cho phép");
                            }

                            await requestSection.Body.CopyToAsync(targetStream);
                            result.Add(new TemporaryFileModel()
                            {
                                FileName = contentDisposition.FileName.Value,
                                TemporaryUrl = relativeFilePath,
                                FullTemporaryUrl = $"{ResourceGlobalConfigs.MediaSourceURL}{relativeFilePath}"
                            });
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }

                    requestSection = await requestReader.ReadNextSectionAsync();
                }
            }

            return Ok(result);
        }

        [HttpPut]
        public IActionResult SaveImages([FromBody] List<string> temporaryImagePaths)
        {
            var result = new List<PictureModel>();
            foreach (var path in temporaryImagePaths)
            {
                var storedImage = _imageProcessorHelper.Save(path);
                if (storedImage == null)
                {
                    return BadRequest();
                }

                result.Add(storedImage);
            }

            return Ok(result);
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
    }
}