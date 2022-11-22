using BaseCore.Entities;
using Domain.Shared;
using Domain.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppServices.Customers
{
    public interface ICustomerServices
    {
        List<AspNetUsers> GetListCRHTC();
        List<Customer> GetListCustomerByPaging(string tu_khoa, int currentPage, int pageSize, out int totalRecord);
        Customer GetById(string customerID);
        ResponseBase AddCustomer(Customer item);
        ResponseBase EditCustomer(Customer item);
        ResponseBase DeleteCustomer(Customer customer);
        List<CustomerDTO> GetList();
        IEnumerable<string> ExistCodesAsync(IEnumerable<string> codes);
        List<DM_Region> GetListRegion();
        //  ResponseBase AddCustomer(Customer item);
    }
}
