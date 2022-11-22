using AppServices.Customers;
using AppServices.User;
using Aspose.Cells;
using BaseCore.Entities;
using Common;
using Domain.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserServices _userServices;
        private ICustomerServices _customerServices;
        private readonly IConfiguration _config;
        public CustomerController(ILogger<HomeController> logger, IUserServices userServices, IConfiguration configuration, ICustomerServices customerServices)
        {
            _logger = logger;
            this._userServices = userServices;
            this._customerServices = customerServices;
            this._config = configuration;
        }

        public IActionResult Index(string tu_khoa = "", int currentPage = 1)
        {
            int totalRecored = 0;
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            List<Customer> lstMenu = new List<Customer>();
            lstMenu = this._customerServices.GetListCustomerByPaging(tu_khoa, currentPage, pageSize, out totalRecored);
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = tu_khoa;
            return View(lstMenu);
        }

        public IActionResult Create()
        {
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);
            var currentPage = 1;
            var totalRecored = 0;
            List<DM_Nation> lstNation = new List<DM_Nation>();
            ViewBag.ListNations = this._userServices.GetListNationByPaging("", currentPage, pageSize, out totalRecored);
            List<DM_Region> lstRegion = new List<DM_Region>();
            ViewBag.ListRegion = this._customerServices.GetListRegion();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer item)
        {

            item.CreatedUid = Convert.ToInt32(User.GetUserUId());
            item.CreatedDateUtc = DateTime.Now;
            item.Deleted = 0;
            //item.Password = EncryptionExtensions.Md5(item.Password);
            var _pb = _customerServices.AddCustomer(item);
            if (_pb.Status == false)
            {
                ViewBag.error = 1;
                ViewBag.msg = "Tạo menu lỗi";
            }
            if (_pb.Status)
            {
                return View("~/Views/Customer/Create.cshtml");
            }
            return View(item);
        }

        public IActionResult Edit(string customerID, string type)
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "Nam",
                Value = "Nam"
            });
            list.Add(new SelectListItem()
            {
                Text = "Nữ",
                Value = "Nữ"
            });

            var customersss = _customerServices.GetById(customerID);
            ViewData["DateOfBirth"] = customersss.DateOfBirth.HasValue? customersss.DateOfBirth.Value.ToString("dd/MM/yyyy") : "";
            ViewData["DateContract"] = customersss.DateContract.HasValue ? customersss.DateContract.Value.ToString("dd/MM/yyyy"): "";
            ViewData["DateNetworkCharges"] = customersss.DateNetworkCharges.HasValue? customersss.DateNetworkCharges.Value.ToString("dd/MM/yyyy"):"";
            ViewData["NationId"] = new SelectList(_userServices.GetList(), "Id", "Name",customersss.NationId);
            ViewData["RegionId"] = new SelectList(_customerServices.GetListRegion(), "Id", "Name", customersss.RegionId);
            ViewData["GenderLookup"] = new SelectList(list,"Text","Value",customersss.GenderLookup);
            return View("~/Views/Customer/Edit.cshtml", this._customerServices.GetById(customerID));
        }

        [HttpPost]
        public IActionResult Edit(Customer item)
        {
            
            item.UpdatedUid = Convert.ToInt32(User.GetUserUId());
            item.UpdatedDateUtc = DateTime.Now;            
            item.CreatedUid = Convert.ToInt32(User.GetUserUId());
            item.Deleted = 0;                    
            var response = this._customerServices.EditCustomer(item);
            if (!response.Status)
            {
                // Xử lý các thông báo lỗi tương ứng
                ViewBag.error = 1;
                ViewBag.msg = "cập nhật khách hàng lỗi";
                return PartialView("~/Views/Customer/Edit.cshtml", item);
            }
            else
            {
                return RedirectToAction("Index");
            }
            //return View(item);

        }
        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            //var uid = Guid.Parse(userManager.GetUserId(User));
            var _pb = this._customerServices.DeleteCustomer(customer);
            if (!_pb.Status)
            {
                return Json(new { error = 1, msg = "Xóa lỗi" });
            }
            return Json(new { error = 0, msg = "Xóa thành công!", href = "/Customer/Index" });
        }

        #region export

        public IActionResult ExportOrder()
        {
            var fileName = "danh-sach-khach-hang.xlsx";
            var orders = _customerServices.GetList();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using var pck = new ExcelPackage();
            //Create the worksheet
            var ws = pck.Workbook.Worksheets.Add("Danh sách khách hàng");
            ws.DefaultColWidth = 20;
            ws.Cells.Style.WrapText = true;
            ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            ws.Column(1).Width = 10;
            ws.Column(2).Width = 15;
            ws.Column(4).Width = 30;
            ws.Column(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[2, 1].Value = "Mã khách hàng";
            ws.Cells[2, 2].Value = "Khu vực";
            ws.Cells[2, 3].Value = "Danh mục khách hàng";
            ws.Cells[2, 4].Value = "Nhóm khách hàng";
            ws.Cells[2, 5].Value = "Loại hình cơ cấu KH";
            ws.Cells[2, 6].Value = "Kiểu khách hàng";
            ws.Cells[2, 7].Value = "Hạng khách hàng";
            ws.Cells[2, 8].Value = "Tên khách hàng";
            ws.Cells[2, 9].Value = "Số hợp đồng";
            ws.Cells[2, 10].Value = "Ngày ký HĐ";
            ws.Cells[2, 11].Value = "Ngày nghiệm thu tính cước";
            ws.Cells[2, 12].Value = "Năm bắt đầu công nợ";
            ws.Cells[2, 13].Value = "Tháng bắt đầu công nợ";
            ws.Cells[2, 14].Value = "Chu kì thanh toán(tháng)";
            ws.Cells[2, 15].Value = "Thời hạn thanh toán(ngày)";
            ws.Cells[2, 16].Value = "Loại doanh thu";
            ws.Cells[2, 17].Value = "Loại dịch vụ";
            ws.Cells[2, 18].Value = "Băng thông";
            ws.Cells[2, 19].Value = "CID/User";
            ws.Cells[2, 20].Value = "Điểm đầu";
            ws.Cells[2, 21].Value = "Điểm cuối";
            ws.Cells[2, 22].Value = "Thông tin thiết bị";
            ws.Cells[2, 23].Value = "Địa chỉ liên hệ";
            ws.Cells[2, 24].Value = "Đầu mối liên hệ kĩ thuật/NOC";
            ws.Cells[2, 25].Value = "Số điện thoại";
            ws.Cells[2, 26].Value = "Email";
            ws.Cells[2, 27].Value = "Đầu mối liên hệ đối tác";            

            var i = 3;
            if (orders != null)
                foreach (var order in orders)
                {
                    //var nc = "";
                    //if(order.Nation == "Việt Nam")
                    //{
                    //    nc = "Trong nước";
                    //}
                    //else
                    //{
                    //    nc = "Nước ngoài";
                    //}
                    ws.Cells[i, 1].Value = i - 2;
                    ws.Cells[i, 2].Value = order.RegionId;
                    ws.Cells[i, 3].Value = order.Nation;
                    ws.Cells[i, 4].Value = order.GroupCustomer;
                    ws.Cells[i, 5].Value = order.SettingCustomer;
                    ws.Cells[i, 6].Value = order.TypeCustomer;
                    ws.Cells[i, 7].Value = order.CateCustomer;
                    ws.Cells[i, 8].Value = order.Name;
                    ws.Cells[i, 9].Value = order.NumberContract;
                    ws.Cells[i, 10].Value = order.DateContract.HasValue? order.DateContract.Value.ToString("dd/MM/yyyy"):"";
                    ws.Cells[i, 11].Value = order.DateNetworkCharges.HasValue? order.DateNetworkCharges.Value.ToString("dd/MM/yyyy"):"";
                    ws.Cells[i, 12].Value = order.YearOfDept;
                    ws.Cells[i, 13].Value = order.MounthOfDept;
                    ws.Cells[i, 14].Value = order.CurclePayment;
                    ws.Cells[i, 15].Value = order.DueDatePayment;
                    ws.Cells[i, 16].Value = order.CateRevenue;
                    ws.Cells[i, 17].Value = order.CateService;
                    ws.Cells[i, 18].Value = order.CateBandwidth;
                    ws.Cells[i, 19].Value = order.CID;
                    ws.Cells[i, 20].Value = order.StartPoint;
                    ws.Cells[i, 21].Value = order.EndPoint;
                    ws.Cells[i, 22].Value = order.Address2;
                    ws.Cells[i, 23].Value = order.Address1;
                    ws.Cells[i, 24].Value = order.Address3;
                    ws.Cells[i, 25].Value = order.PhoneNumber;
                    ws.Cells[i, 26].Value = order.Email;
                    ws.Cells[i, 27].Value = order.CluePayment;                   
                    i++;
                }            

            // set style name column
            using (var rng = ws.Cells["A2:AA2"])
            {
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                rng.Style.Font.Bold = true;
                rng.Style.WrapText = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(79, 129,
                    189)); //Set color to dark blue
                rng.Style.Font.Color.SetColor(System.Drawing.Color.White);
            }

            return File(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        public ActionResult DownloadExcel(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                return null;
            }
        }

        #endregion export
        
    }
}
