using AppServices;
using BaseCore;
using BaseCore.Entities;
using Domain.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AppServices.Customers;
using AppServices.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Business;

namespace Domain.Web.Controllers
{
    public class CrController : Controller
    {
        private readonly ICrServices _crServices;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public CrController(ICrServices crServices, IConfiguration config, ILogger<HomeController> logger)
        {
            _crServices = crServices;
            _config = config;
            _logger = logger;
        }

        public IActionResult Index(string tu_khoa = "", int currentPage = 1)
        {
            int totalRecored = 0;
            var pageSize = Convert.ToInt32(_config["AppSetting:PageSize"]);

            var res = this._crServices.GetListMenuByPaging(tu_khoa, currentPage, pageSize, out totalRecored);

            List<CRHTCGridModel> lstMenu = new List<CRHTCGridModel>();
            lstMenu.AddRange(res);
            ViewBag.TotalRecored = totalRecored;
            ViewBag.TotalPage = (totalRecored / pageSize) + ((totalRecored % pageSize) > 0 ? 1 : 0);
            ViewBag.CurrentPage = currentPage;
            ViewBag.RecoredFrom = 1;
            ViewBag.RecoredTo = ViewBag.TotalPage == 1 ? totalRecored : pageSize;
            ViewBag.txtSearch = tu_khoa;


            return View(res);
        }
    }
}
