using ContractManagement.Domain.AggregateModels.PictureAggregate;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.EquipmentAggregate
{
    [Table("EquipmentPictures")]
    public class EquipmentPicture : Entity
    {
        public int PictureId { get; set; }
        public int EquipmentId { get; set; }

        //private Equipment _equipment;
        //private Picture _picture;
    }
}
