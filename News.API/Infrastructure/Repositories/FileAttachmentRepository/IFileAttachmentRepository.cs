using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Repositories.FileAttachmentRepository
{
    public interface IFileAttachmentRepository : ICrudRepository<Models.FileAttachment, int>
    {
    }
}
