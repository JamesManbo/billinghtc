using BaseCore.Entities;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public interface ICrRepository
    {
        IList<CRHTCGridModel> GetListMenuByPaging(string tu_khoa, int currentPage, int pageSize, out int totalRecord);

        Task<CR_HTC> GetById(string code);
    }
}
