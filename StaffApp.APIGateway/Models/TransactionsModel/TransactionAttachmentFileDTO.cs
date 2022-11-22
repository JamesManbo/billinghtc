using GenericRepository.Extensions;
using Global.Configs.ResourceConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class TransactionAttachmentFileDTO
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int? InContractId { get; set; }
        public int? OutContractId { get; set; }
        public string ResourceStorage { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public int FileType { get; set; }
        public string Extension { get; set; }
        public string RedirectLink { get; set; }
        public string FullUrl =>
            $"{ResourceGlobalConfigs.MediaSourceURL}{ResourceGlobalConfigs.FileFolder}{FilePath}".ResolveUrl();
    }
}
