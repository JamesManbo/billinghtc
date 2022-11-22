using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.AccountViewModels
{
    public class UserUserGroupViewModel
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }

    }
}
