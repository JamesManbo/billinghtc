using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using GenericRepository;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebtManagement.Infrastructure.Repositories
{
    public interface IAttachmentFileRepository : ICrudRepository<AttachmentFile, int>
    {
        void DeleteMany(List<int> deleteIds);
    }

    public class AttachmentFileRepository : CrudRepository<AttachmentFile, int>, IAttachmentFileRepository
    {
        private readonly DebtDbContext _context;
        public AttachmentFileRepository(DebtDbContext context, IWrappedConfigAndMapper configAndMapper) : base(context, configAndMapper)
        {
            _context = context;
        }

        public void DeleteMany(List<int> deleteIds)
        {
            if (deleteIds.Count == 0) return;

            Context.Database.ExecuteSqlRaw($"DELETE FROM {TableName} WHERE Id IN ({string.Join(",", deleteIds)})");
        }
    }
}
