using BaseCore.Entities;
using BaseCoreDataObject;
using Business;
using Domain.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Domain.Shared.DTO;

namespace BaseCore
{

    public class CustomerRepository : BaseRepository<HTCDataContext, AspNetUsers>, ICustomerRepository 
    {

        public CustomerRepository()
            : base(new HTCDataContext())
        {
            //_httpClientFactory = new ApiMemoric();
            //_bip39Repository = new Bip39Repository();
        }
        public Customer GetById(string customerID)
        {
            return _dbContext.Customers.FirstOrDefault(n => n.CustomerId == customerID);
        }

        public IEnumerable<string> ExistCodesAsync(IEnumerable<string> codes)
        {
            if (codes == null)
                throw new ArgumentNullException(nameof(codes));

            return _dbContext.Customers
                .Where(w => !string.IsNullOrEmpty(w.CustomerId) && codes.Contains(w.CustomerId))
                .Select(s => s.CustomerId);
        }
        public List<DM_Region> GetListRegion()
        {

            var lstMenu = new List<DM_Region>();
            var ds = _dbContext.DM_Regions.ToList();
            return ds;

        }
        public List<AspNetUsers> GetListCRHTC()
        {
            return _dbContext.AspNetUsers.AsQueryable().ToList();
        }
        public List<CustomerDTO> GetList()
        {
            var query = (from p in _dbContext.Customers.AsNoTracking()
                        join u in _dbContext.DM_Nations on p.NationId equals u.Id
                        select new CustomerDTO
                        {
                            RegionId = p.RegionId,
                            NationId = u.Id,
                            Nation = u.Name,
                            GroupCustomer = p.GroupCustomer,
                            SettingCustomer = p.SettingCustomer,
                            TypeCustomer = p.TypeCustomer,
                            CateCustomer = p.CateCustomer,
                            Name = p.Name,
                            NumberContract = p.NumberContract,
                            DateContract = p.DateContract,
                            DateNetworkCharges = p.DateNetworkCharges,
                            YearOfDept = p.YearOfDept,
                            MounthOfDept = p.MounthOfDept,
                            CurclePayment = p.CurclePayment,
                            DueDatePayment = p.DueDatePayment,
                            CateRevenue = p.CateRevenue,
                            CateService = p.CateService,
                            CateBandwidth = p.CateBandwidth,
                            CID = p.CID,
                            StartPoint = p.StartPoint,
                            EndPoint = p.EndPoint,
                            Address2 = p.Address2,
                            Address1 = p.Address1,
                            Address3 = p.Address3,
                            PhoneNumber = p.Address3,
                            Email = p.Email,
                            CluePayment = p.CluePayment
                        }).ToList();

            return query;
        }
        public List<Customer> GetListCustomerByPaging(string tu_khoa, int currentPage, int pageSize,out int totalRecord)
        {
                var lstMenu = new List<Customer>();
                var ds = _dbContext.Customers.Where(n => n.Deleted == 0).OrderBy(n => n.Stt.HasValue ? n.Stt : 999999).ToList();
                lstMenu = ds.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();
                totalRecord = ds.Count();
                return lstMenu;
        }


        public ResponseBase AddCustomer(Customer customer)
        {
            try
            {
                _dbContext.Customers.Add(customer);
                _dbContext.SaveChanges();                
                ResponseBase responseBase = new ResponseBase() { Status = true, Message = "Thêm mới thành công", Code = customer.CustomerId };
                return responseBase;
            }
            catch (Exception e)
            {
                return new ResponseBase { Id = 0, Status = false, Message = "Thêm mới chức năng không thành công!" };
            }
        }
        public ResponseBase EditCustomer(Customer customer)
        {
            try
            {                
                _dbContext.Update(customer);
                var a = _dbContext.SaveChanges() > 0;
                ResponseBase responseBase = new ResponseBase() { Status = a, Message =a? "Cập nhật thành công":"Thất bại", Code = customer.CustomerId };
                return responseBase;
            }
            catch (Exception e)
            {
                return new ResponseBase { Id = 0, Status = false, Message = "Cập nhật không thành công!" };
            }
        }
        public ResponseBase DeleteCustomer(Customer customer)
        {
            try
            {
                _dbContext.Remove(customer);
                _dbContext.SaveChanges();
                ResponseBase responseBase = new ResponseBase() { Status = true, Message = "Xóa chức năng thành công", Code = customer.CustomerId };
                return responseBase;
            }
            catch (Exception e)
            {
                return new ResponseBase { Id = 0, Status = false, Message = "Xóa chức năng không thành công!" };
            }
        }

    }
}
