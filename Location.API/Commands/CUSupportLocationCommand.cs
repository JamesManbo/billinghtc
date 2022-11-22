using Location.API.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Location.API.Commands
{
    public class CUSupportLocationCommand : IRequest<SupportLocationDTO>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Phone { get; set; }
        public string Path { get; set; }
        public string Address { get; set; }
        public int Level { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
