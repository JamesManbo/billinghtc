using System;
using System.IO;
using System.Threading;
using Global.Configs.ResourceConfig;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using StaticResource.API.Models;

namespace StaticResource.API.Helper
{
    public class ImageProcessorHelper
    {
        private static string FileContentRootPath = $"{Program.RootDirectory}";
        //        private readonly string _originalFolderPath;
        private readonly string _optimalFolderPath;
        private readonly string _w1280FolderPath;
        private readonly string _w640FolderPath;
        private readonly string _w320FolderPath;
        private readonly string _w160FolderPath;
        private readonly string _thumbFolderPath;
        private const int CommonQuantity = 75;

        public ImageProcessorHelper()
        {
//            this._originalFolderPath = FileContentRootPath + ResourceGlobalConfigs.ImageOriginalFolder;
            this._optimalFolderPath = Path.Combine(FileContentRootPath,
                ResourceGlobalConfigs.ImageOptimizeFolder);
            this._w1280FolderPath =
                Path.Combine(FileContentRootPath, ResourceGlobalConfigs.ImageW1280Folder);
            this._w640FolderPath =
                Path.Combine(FileContentRootPath, ResourceGlobalConfigs.ImageW640Folder);
            this._w320FolderPath =
                Path.Combine(FileContentRootPath, ResourceGlobalConfigs.ImageW320Folder);
            this._w160FolderPath =
                Path.Combine(FileContentRootPath, ResourceGlobalConfigs.ImageW160Folder);
            this._thumbFolderPath =
                Path.Combine(FileContentRootPath, ResourceGlobalConfigs.ImageThumbFolder);
        }

        public PictureModel Save(string filePath, int findingAttemptTimes = 0)
        {
            if (!filePath.StartsWith(FileContentRootPath))
            {
                filePath = Path.Combine(FileContentRootPath, filePath);
            }

            PictureModel storedImageObj = null;
            var fileName = Path.GetFileName(filePath);

            if (File.Exists(filePath))
            {
                var imageHolderPath = CreateImageHolderFolderPath();

//                Directory.CreateDirectory(_originalFolderPath + imageHolderPath);
                Directory.CreateDirectory(_optimalFolderPath + imageHolderPath);
                Directory.CreateDirectory(_w1280FolderPath + imageHolderPath);
                Directory.CreateDirectory(_w640FolderPath + imageHolderPath);
                Directory.CreateDirectory(_w320FolderPath + imageHolderPath);
                Directory.CreateDirectory(_w160FolderPath + imageHolderPath);
                Directory.CreateDirectory(_thumbFolderPath + imageHolderPath);

                var storedFileName = Path.GetFileNameWithoutExtension(fileName).ResolveFileName();
                var storedExtension = Path.GetExtension(fileName);
                var storedPath = Path.Combine(imageHolderPath, $"{storedFileName}{storedExtension}");
                byte[] photoBytes = File.ReadAllBytes(filePath);

//                SaveToPath(filePath, $"{_originalFolderPath}/{storedPath}", ImageScaleType.ORIGINAL);
                SaveToPath(filePath, $"{_optimalFolderPath}/{storedPath}", ImageScaleType.OPTIMAL);
                SaveToPath(filePath, $"{_w1280FolderPath}/{storedPath}", ImageScaleType.W1280);
                SaveToPath(filePath, $"{_w640FolderPath}/{storedPath}", ImageScaleType.W640);
                SaveToPath(filePath, $"{_w320FolderPath}/{storedPath}", ImageScaleType.W320);
                SaveToPath(filePath, $"{_w160FolderPath}/{storedPath}", ImageScaleType.W160);
                SaveToPath(filePath, $"{_thumbFolderPath}/{storedPath}", ImageScaleType.THUMBNAIL);

                storedImageObj = new PictureModel
                {
                    FileName = storedFileName,
                    FilePath = storedPath,
                    Extension = storedExtension,
                    Size = photoBytes.Length
                };
            }
            else if (findingAttemptTimes <= Convert.ToInt32(ResourceGlobalConfigs.NumberOfAttemptFindingTemporaryFile))
            {
                // Perhaps due to delayed of synchronous data between two servers(load balancing), the temporary file could not be found.
                // So try to find the file again
                Thread.Sleep(2 * 1000);
                Save(filePath, findingAttemptTimes + 1);
            }
            else
            {
                return null;
            }

            return storedImageObj;
        }

        private void SaveToPath(string sourceFilePath, string filePath, ImageScaleType scaleType)
        {
            using (var imageFactory = new MagickImage(sourceFilePath))
            {
                switch (scaleType)
                {
                    case ImageScaleType.W1280:
                        imageFactory.Resize(1280, 0);
                        break;
                    case ImageScaleType.W640:
                        imageFactory.Resize(640, 0);
                        break;
                    case ImageScaleType.W320:
                        imageFactory.Resize(320, 0);
                        break;
                    case ImageScaleType.W160:
                        imageFactory.Resize(160, 0);
                        break;
                    case ImageScaleType.THUMBNAIL:
                        imageFactory.Resize(80, 0);
                        break;
                }

                imageFactory.Strip();
                imageFactory.Quality = CommonQuantity;
                imageFactory
                    .Write(filePath);
            }
        }

        private string CreateImageHolderFolderPath()
        {
            string holderPath = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
            return holderPath;
        }
    }

    public enum ImageScaleType
    {
        ORIGINAL,
        OPTIMAL,
        W1280,
        W640,
        W320,
        W160,
        THUMBNAIL
    }
}