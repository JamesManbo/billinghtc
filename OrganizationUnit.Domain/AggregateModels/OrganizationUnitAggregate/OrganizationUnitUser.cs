using OrganizationUnit.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate
{
    [Table("OrganizationUnitUsers")]
    public class OrganizationUnitUser
    {
        public int OrganizationUnitId { get; set; }
        public int UserId { get; set; }
        /// <summary>
        /// Vị trí/chức vụ của nhân viên trong phòng ban
        /// 0: Nhân viên
        /// 1: Trưởng phòng
        /// </summary>
        public int PositionLevel { get; set; } = 0;
    }
}
