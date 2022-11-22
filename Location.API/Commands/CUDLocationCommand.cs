using Location.API.Model;
using MediatR;
using System;

namespace Location.API.Commands
{
    public class CULocationCommand : IRequest<LocationDTO>
    {
        public string Id { get; set; }
        public string LocationId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Path { get; set; }
        public int Level { get; set; }
        public string ParentId { get; set; }
        public string ParentName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Message { get; set; }
        public float DisplayOrder { get; set; }
        public bool IsShow { get; set; }
    }
    public class DeleteLocationCommand : IRequest<bool>
    {
        public DeleteLocationCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
