using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.AccountViewModels
{
    public class UserClassViewModel
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public int ConditionStartPoint { get; set; }
        public int ConditionEndPoint { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
