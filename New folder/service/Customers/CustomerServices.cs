using BaseCore.Entities;
using Business;
using conin.Cached;
using conin.caching;
using Domain.Shared;
using Domain.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppServices.Customers
{
    public class CustomerServices : BaseFE, ICustomerServices
    {
        public IUserRepository _userRepository;
        public ICustomerRepository _customerRepository;
        public CustomerServices(ICachingReposity _cachingReposity,
          IUserRepository userRepository, ICustomerRepository customerRepository)
          : base(_cachingReposity)
        {
            this._userRepository = userRepository;
            this._customerRepository = customerRepository;
        }

        public Customer GetById(string customerID)
        {
            return this._customerRepository.GetById(customerID);
        }
        public List<DM_Region> GetListRegion()
        {
            return this._customerRepository.GetListRegion();
        }
        public IEnumerable<string> ExistCodesAsync(IEnumerable<string> codes)
        {
            return this._customerRepository.ExistCodesAsync(codes);
        }
        public ResponseBase AddCustomer(Customer item)
        {
            return this._customerRepository.AddCustomer(item);
        }
        

        public ResponseBase DeleteCustomer(Customer customer)
        {
            return this._customerRepository.DeleteCustomer(customer);
        }

        public ResponseBase EditCustomer(Customer item)
        {
            return this._customerRepository.EditCustomer(item);
        }

        public List<AspNetUsers> GetListCRHTC()
        {
            return this._userRepository.GetListCRHTC();
        }

        public List<CustomerDTO> GetList()
        {
            return this._customerRepository.GetList();
        }

        public List<Customer> GetListCustomerByPaging(string tu_khoa, int currentPage, int pageSize, out int totalRecord)
        {
            return this._customerRepository.GetListCustomerByPaging(tu_khoa, currentPage, pageSize, out totalRecord);
        }

        
    }
}
