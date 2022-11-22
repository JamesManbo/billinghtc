using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.AggregatesModel.Commons;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ContractManagement.Infrastructure.Repositories.FileRepository
{

    public interface IFileRepository : ICrudRepository<AttachmentFile, int>
    {
        void CheckAndDeleteFileInContract(List<string> fileName, long size, string extension, int inContractId);
    }

    public class FileRepository : CrudRepository<AttachmentFile, int>, IFileRepository
    {
        private readonly ContractDbContext _context;
        public FileRepository(ContractDbContext context, IWrappedConfigAndMapper configAndMapper) 
            : base(context, configAndMapper)
        {
            _context = context;
        }

        //Check những file đã trùng

        public void CheckAndDeleteFileInContract(List<string> fileName, long size, string extension, int inContractId)
        {
            //lấy ra các bản ghi có trong hợp đồng 
            var lstAttachFile = _context.AttachmentFiles.Where(x => x.InContractId == inContractId && x.IsDeleted == false);
            var x = lstAttachFile.Where(a => !fileName.Contains(a.FileName)).ToList();

            if (x.Count() > 0)
            {
                foreach (var i in lstAttachFile)
                {
                    _context.AttachmentFiles.Remove(i);
                }
                _context.SaveChanges();
            }
        }

    }
}