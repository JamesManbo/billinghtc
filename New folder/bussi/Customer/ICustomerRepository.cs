using BaseCore.Entities;
using Domain.Shared;
using Domain.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public interface ICustomerRepository
    {
        List<AspNetUsers> GetListCRHTC();
        List<Customer> GetListCustomerByPaging(string tu_khoa, int currentPage, int pageSize, out int totalRecord);
        ResponseBase AddCustomer(Customer item);
        ResponseBase EditCustomer(Customer item);
        ResponseBase DeleteCustomer(Customer customer);
        Customer GetById(string customerID);
        List<CustomerDTO> GetList();
        IEnumerable<string> ExistCodesAsync(IEnumerable<string> codes);
        List<DM_Region> GetListRegion();
    }
}
