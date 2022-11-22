using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Models
{
    [Table("FileAttachments")]
    public class FileAttachment : Entity
    {
        public FileAttachment()
        {
        }

        [Key]
        public int Id { get; set; }
       
        [StringLength(256)]
        public string Name { get; set; }
        [Required]
        [StringLength(256)]
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public int? Order { get; set; }
        public int FileType { get; set; }
        public string Extension { get; set; }
        public string RedirectLink { get; set; }
    }
}
