using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregateModels.PictureAggregate
{
    [Table("Pictures")]
    public class Picture : Entity
    {

        public Picture()
        {
        }

        [StringLength(256)] public string Name { get; set; }
        [Required] [StringLength(256)] public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public int? Order { get; set; }
        public int PictureType { get; set; }
        [Required] public string Extension { get; set; }
        public string RedirectLink { get; set; }
    }
}
