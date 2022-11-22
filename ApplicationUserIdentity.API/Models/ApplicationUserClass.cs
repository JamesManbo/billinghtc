using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Models.Seed;

namespace ApplicationUserIdentity.API.Models
{
    [Table("ApplicationUserClasses")]
    public class ApplicationUserClass: Entity
    {
        public static ApplicationUserClass Copper = new ApplicationUserClass(1,"Hạng đồng", "COPPER_LEVEL", 0, 499999 );
        public static ApplicationUserClass Silver = new ApplicationUserClass(2, "Hạng bạc", "SILVER_LEVEL", 500000, 999999);
        public static ApplicationUserClass Gold = new ApplicationUserClass(3, "Hạng vàng", "GOLD_LEVEL", 1000000, 2999999);
        public static ApplicationUserClass Diamond = new ApplicationUserClass(4, "Hạng kim cương", "DIAMOND_LEVEL", 3000000, 5000000);

        public ApplicationUserClass()
        {
        }

        public static IEnumerable<ApplicationUserClass> Seeds() => new[]
            {Copper, Silver, Gold, Diamond};

        public ApplicationUserClass(int id, string className, string classCode, decimal conditionStartPoint, decimal conditionEndPoint)
        {
            Id = id;
            ClassName = className;
            ClassCode = classCode;
            ConditionStartPoint = conditionStartPoint;
            ConditionEndPoint = conditionEndPoint;
        }

        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public decimal ConditionStartPoint { get; set; }
        public decimal ConditionEndPoint { get; set; }
    }
}
