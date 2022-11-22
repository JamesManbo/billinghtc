using System;
using System.IO;
using System.Threading;
using Global.Configs.ResourceConfig;
using Microsoft.AspNetCore.Hosting;
using StaticResource.API.Models;

namespace StaticResource.API.Helper
{
    public class FileProcessorHelper
    {
        private static string FileContentRootPath = $"{Program.RootDirectory}";
        private readonly string _fileFolderPath;

        public FileProcessorHelper()
        {
            this._fileFolderPath = Path.Combine(FileContentRootPath, ResourceGlobalConfigs.FileFolder);
        }

        public FileAttachmentModel Save(string temporaryFileName, int findingAttemptTimes = 0)
        {
            var temporaryFilePath = string.Empty;
            if (!temporaryFileName.StartsWith(FileContentRootPath))
            {
                temporaryFilePath = Path.Combine(FileContentRootPath, temporaryFileName);
            }

            FileAttachmentModel storedFileObj = null;
            var fileName = Path.GetFileName(temporaryFilePath);

            if (File.Exists(temporaryFilePath))
            {
                var fileHolderPath = CreatefileHolderFolderPath();

                Directory.CreateDirectory(_fileFolderPath + fileHolderPath);

                var storedFileName = Path.GetFileNameWithoutExtension(fileName).ResolveFileName();
                storedFileName += $"-{DateTime.UtcNow.Ticks}";
                var storedExtension = Path.GetExtension(fileName);
                var storedPath = Path.Combine(fileHolderPath, $"{storedFileName}{storedExtension}");
                byte[] photoBytes = File.ReadAllBytes(temporaryFilePath);

                SaveToPath(temporaryFilePath, $"{_fileFolderPath}{storedPath}");

                storedFileObj = new FileAttachmentModel
                {
                    TemporaryUrl = temporaryFileName,
                    FileName = $"{storedFileName}{storedExtension}",
                    FilePath = storedPath,
                    Extension = storedExtension,
                    Size = photoBytes.Length
                };
            }
            else if (findingAttemptTimes <= 5)
            {
                // Perhaps due to delayed of synchronous data between two servers(load balancing), the temporary file could not be found.
                // So try to find the file again after minutes
                Thread.Sleep(2 * 1000);
                Save(temporaryFilePath, findingAttemptTimes + 1);
            }
            else
            {
                return null;
            }

            return storedFileObj;
        }

        private void SaveToPath(string sourceFilePath, string filePath)
        {
            System.IO.File.Copy(sourceFilePath, filePath);
        }

        private string CreatefileHolderFolderPath()
        {
            string holderPath = $"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}";
            return holderPath;
        }
    }
}