using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.AccountViewModels
{
    public class UserIndustryViewModel
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int IndustryId { get; set; }
        public string Name { get; set; }
    }
}
