using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Models.AccountViewModels
{
    public class AddMapUsersAndUserGroupDTO
    {
        public List<int> UserIds { get; set; }
        public int? GroupId { get; set; }
    }
}
