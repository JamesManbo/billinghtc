using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class AttachmentFileDTO
    {
        public int Id { get; set; }
        public int OutContractId { get; set; }
        public string ResourceStorage { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Size { get; set; }
        public int FileType { get; set; }
        public string Extension { get; set; }
        public string RedirectLink { get; set; }
        public string TemporaryUrl { get; set; }
    }
}
