using GenericRepository;
using GenericRepository.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.API.Infrastructure.Repositories.FileAttachmentRepository
{
    public class FileAttachmentRepository : CrudRepository<Models.FileAttachment, int>, IFileAttachmentRepository
    {
        public FileAttachmentRepository(NewsDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {

        }
    }
}
