using BaseCore.Entities;
using BaseCoreDataObject;
using Business;
using Domain.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using BaseCore;
using Microsoft.EntityFrameworkCore;

namespace Business
{

    public class CrRepository : BaseRepository<TicketDBContext, CR_HTC>, ICrRepository
    {

        public CrRepository()
            : base(new TicketDBContext())
        {
            //_httpClientFactory = new ApiMemoric();
            //_bip39Repository = new Bip39Repository();
        }

        public async Task<CR_HTC> GetById(string code)
        {
            return await this._dbContext.CR_HTC.FirstOrDefaultAsync(x => x.Id.Equals(code));
        }

        public IList<CRHTCGridModel> GetListMenuByPaging(string tu_khoa, int currentPage, int pageSize, out int totalRecord)
        {

            var query = from p in _dbContext.CR_HTC.ToList()
                        join t in _dbContext.CR.ToList() on p.CrId equals t.Id into pt
                        from tp in pt.DefaultIfEmpty()
                        join k in _dbContext.Status.ToList() on tp.Status equals k.Id into kn
                        from kt in kn.DefaultIfEmpty()
                        where tp.ProjectId.Equals("961D7F87-BBB3-4755-8100-D7BB9FE44581")
                        select new CRHTCGridModel
                        {
                            Id = tp.Id,
                            CrId = tp.Id,
                            Code = tp.Code,
                            Name = tp.Name,
                            Status = kt.Name,
                            CreatedBy = tp.CreatedBy,
                            CreatedDate = tp.CreatedDate,
                            ModifiedBy = tp.ModifiedBy,
                            ModifiedDate = tp.ModifiedDate,
                            Detail = tp.Detail,
                            Note = tp.Note,
                            Inactive = tp.Inactive,
                            ProjectId = tp.ProjectId,
                            StartDate = tp.StartDate,
                            FinishDate = tp.FinishDate,
                            ImplementationSteps = tp.ImplementationSteps,
                            CrReason = tp.CrReason,
                            FieldHandler = tp.FieldHandler,
                            InfluenceChannel = tp.InfluenceChannel,
                            StartTimeAction = p.StartTimeAction,
                            RestoreTimeService = p.RestoreTimeService,
                            HourTimeMinus = p.HourTimeMinus ?? 0,
                            MinuteTimeMinus = p.MinuteTimeMinus ?? 0,
                            SecondTimeMinus = p.SecondTimeMinus ?? 0,
                            OverTimeRegister = p.OverTimeRegister,
                            Supervisor = p.Supervisor,
                            StatusId = kt.Id,
                            Total = tp.TotalTime.ToString(),
                        };
            query = query.Skip(pageSize * (currentPage - 1)).Take(pageSize);
            totalRecord = query.Count();
            return query.ToList();
        }
    }
}
