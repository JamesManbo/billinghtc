using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using BaseCore.Entities;
using Aspose.Cells;
using Microsoft.AspNetCore.Http;
using Domain.Web.Models;
using AppServices.Customers;
using System.Linq;
using AppServices.User;

namespace HG.WebApp.Controllers
{
    public class UploadController : Controller
    {
        private IWebHostEnvironment Environment;
        //IConfiguration _iconfiguration;
        private readonly IConfiguration _config;
        private ICustomerServices _customerServices;
        private IUserServices _userServices;
        //
        // GET: /Upload/
        public UploadController(IWebHostEnvironment _environment, IConfiguration configuration, ICustomerServices customerServices, IUserServices userServices)
        {
            Environment = _environment;
            _config = configuration;
            _customerServices = customerServices;
            _userServices = userServices;
        }

        [HttpPost]
        public JsonResult UploadFiles(string folder)
        {
            var now = DateTime.Now;
            var files = HttpContext.Request.Form.Files;
            if (HttpContext.Request.Form.Files.Count <= 0) return Json(new { status = false, msg = "Bạn chưa chọn file." });
            var lstFiles = new List<string>();
            var lstFilesName = new List<string>();
            var virtualPath = string.Format(_config.GetSection(_config["AppSetting:PageSize"]).GetSection("TestUrl").Value + "{0}\\{1}\\{2}\\{3}\\", folder, now.Year,
                now.Month, now.Day);
            string webRootPath = Environment.WebRootPath;
            var path = Path.Combine(webRootPath, virtualPath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (var fileItem in files)
            {
                string filename = fileItem.FileName;
                using (var fileStream = new FileStream(Path.Combine( path, filename), FileMode.Create))
                {
                    fileItem.CopyToAsync(fileStream);
                    lstFiles.Add(string.Format("{0}\\{1}", path, filename));
                    lstFilesName.Add(filename);
                }
            }
            FileGetListDataChannel(files[0]);
            return Json(new { status = true, msg = "Upload thành công", files = lstFiles, names = lstFilesName });
        }
        public IActionResult FileGetListDataChannel(IFormFile file)
        {
            using var mst = new MemoryStream(file.OpenReadStream().ToByteArray());

            Workbook workbook = new Workbook(mst);

            Worksheet worksheet = workbook.Worksheets[0];
            if (worksheet == null)
            {
                return Ok(new  { success = false });
            }            
            List<Customer> list = new List<Customer>();
            var availableListNation = _userServices.GetList();
            var listRegion = _customerServices.GetListRegion();
            var lstCodes = list.Where(w => !string.IsNullOrWhiteSpace(w.CustomerId)).Select(s => s.CustomerId).Distinct();
            var customersss = _customerServices.ExistCodesAsync(lstCodes);
            
            for (int i = 2; i <= worksheet.Cells.MaxDataRow; i++)
            {
                list.Add(new Customer()
                {

                    CustomerId = GetValueResult(worksheet.Cells[i,0].StringValue),                    
                    RegionId = listRegion.FirstOrDefault(x => x.Name.Equals(worksheet.Cells[i, 1].StringValue.Trim()))?.Id,
                    NationId = availableListNation.FirstOrDefault(x => x.Name.Equals(worksheet.Cells[i, 2].StringValue.Trim()))?.Id,
                    GroupCustomer = GetValueResult(worksheet.Cells[i, 3].StringValue),
                    SettingCustomer = GetValueResult(worksheet.Cells[i, 4].StringValue),                  
                    TypeCustomer = GetValueResult(worksheet.Cells[i, 5].StringValue),
                    CateCustomer = GetValueResult(worksheet.Cells[i, 6].StringValue),
                    Name = GetValueResult(worksheet.Cells[i, 7].StringValue),
                    NumberContract = GetValueResult(worksheet.Cells[i, 8].StringValue),                   
                    DateContract = DateTime.Parse(worksheet.Cells[i, 9].StringValue),                    
                    DateNetworkCharges = DateTime.Parse(worksheet.Cells[i, 10].StringValue),
                    YearOfDept = int.Parse(GetValueResultNumber(worksheet.Cells[i, 11].StringValue)),
                    MounthOfDept = int.Parse(GetValueResultNumber(worksheet.Cells[i, 12].StringValue)),
                    CurclePayment = int.Parse(GetValueResultNumber(worksheet.Cells[i, 13].StringValue)),
                    DueDatePayment = int.Parse(GetValueResultNumber(worksheet.Cells[i, 14].StringValue)),
                    CateRevenue = GetValueResult(worksheet.Cells[i, 15].StringValue),
                    CateService = GetValueResult(worksheet.Cells[i, 16].StringValue),
                    CateBandwidth = GetValueResult(worksheet.Cells[i, 17].StringValue),
                    CID = GetValueResult(worksheet.Cells[i, 18].StringValue),
                    StartPoint = GetValueResult(worksheet.Cells[i, 19].StringValue),
                    EndPoint = GetValueResult(worksheet.Cells[i, 20].StringValue),
                    Address2 = GetValueResult(worksheet.Cells[i, 21].StringValue),
                    Address1 = GetValueResult(worksheet.Cells[i, 22].StringValue),
                    Address3 = GetValueResult(worksheet.Cells[i, 23].StringValue),
                    PhoneNumber = validate(worksheet.Cells[i, 24].StringValue),
                    Email = GetValueResult(worksheet.Cells[i, 25].StringValue),
                    CluePayment = GetValueResult(worksheet.Cells[i, 26].StringValue),
                    Deleted = 0,
                    CreatedDateUtc = DateTime.Now,
                    CreatedUid = Convert.ToInt32(User.GetUserUId())
            });               
            }
            int hhh = 0;
            foreach (var item in list)
            {
                try
                {
                    hhh++;
                    var res = _customerServices.AddCustomer(item);
                }
                catch (Exception e)
                {
                    continue;
                }

            }
            //            var res = await _channelService.InsertAsync(list);

            return Ok(hhh);
        }
        
        public static string validate(string t)
        {
            return t;
        }
        public static string GetValueResult(string t)
        {
            t = t.Trim();
            return t.Equals("null") || string.IsNullOrEmpty(t) || t.Equals("NA") ? "" : t;
        }       
        public static string GetValueResultNumber(string t)
        {
            t = t.Replace("Km", "").Trim();
            if (t.Contains("E"))
                return Math.Round(Decimal.Parse(t, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint), 2).ToString();
            if (t.Contains("G"))
                return "0";
            return t.Equals("null") || string.IsNullOrEmpty(t) || t.Equals("NA") ? "0" : t;
        }       

    }
}
