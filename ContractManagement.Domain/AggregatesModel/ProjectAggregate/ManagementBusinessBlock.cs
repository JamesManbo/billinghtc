using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ProjectAggregate
{
    [Table("ManagementBusinessBlocks")]
    public class ManagementBusinessBlock : Entity
    {
        public static ManagementBusinessBlock DVVT = new ManagementBusinessBlock(1,"DVVT","", "");
        public static ManagementBusinessBlock Data = new ManagementBusinessBlock(2, "Dữ liệu", "", "");
        public static ManagementBusinessBlock Technique = new ManagementBusinessBlock(3, "Ký thuật", "", "");

        public static IEnumerable<ManagementBusinessBlock> Seeds() => new[]
            {DVVT, Data, Technique};

        [StringLength(256)]
        public string BusinessBlockName { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }

        public ManagementBusinessBlock()
        {

        }
        public ManagementBusinessBlock(int id, string businessBlockName, string code, string note)
        {
            Id = id;
            BusinessBlockName = businessBlockName;
            Code = code;
            Note = note;
        }
    }


}
