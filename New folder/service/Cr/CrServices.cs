using BaseCore.Entities;
using Business;
using conin.Cached;
using conin.caching;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppServices
{
    public class CrServices : BaseFE, ICrServices
    {
        public IUserRepository _userRepository;
        public ICrRepository _customerRepository;
        public CrServices(ICachingReposity _cachingReposity,
          IUserRepository userRepository, ICrRepository customerRepository)
          : base(_cachingReposity)
        {
            this._userRepository = userRepository;
            this._customerRepository = customerRepository;
        }

        public async Task<CR_HTC> GetById(string code)
        {
            return await this._customerRepository.GetById(code);
        }

        public IList<CRHTCGridModel> GetListMenuByPaging(string tu_khoa, int currentPage, int pageSize, out int totalRecord)
        {
            return this._customerRepository.GetListMenuByPaging(tu_khoa, currentPage, pageSize,out totalRecord);
        }
    }
}
