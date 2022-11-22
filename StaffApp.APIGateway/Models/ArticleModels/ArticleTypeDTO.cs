using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ArticleModels
{
    public class ArticleTypeDTO
    {
        public int Id { get; set; }
        public string ASCII { get; set; }
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public string Description { get; set; }
    }
}
