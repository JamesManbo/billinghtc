using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericRepository.Extensions;
using Global.Models.Response;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentMethodsController : CustomBaseController
    {

        [HttpGet()]
        public async Task<IActionResult> GetListMethod()
        {
            List<Dictionary<string, object>> listMethod = new List<Dictionary<string, object>>() {
            new Dictionary<string, object> {
                { "name", "Tiền mặt" },
                { "value", 0},
                { "id", 0 }
            },
            new Dictionary<string, object> {
                { "name", "Chuyển khoản" },
                { "value", 1},
                { "id", 1 }
            },
            new Dictionary<string, object> {
                { "name", "Bù trừ thanh toán" },
                { "value", 9},
                { "id", 9 }
            },
                //new SelectionItem() { Text= "Tiền mặt", Value= 0 },
                //new SelectionItem() { Text= "Chuyển khoản", Value= 1 },
                //new SelectionItem() { Text= "Bù trừ thanh toán", Value= 9 }
            };
            return Ok(listMethod);
        }

        [HttpGet("GetListForm")]
        public async Task<IActionResult> GetListForm()
        {
            List<SelectionItem> listForm = new List<SelectionItem>() {
                new SelectionItem() { Text= "Trả sau", Value= 0 },
                new SelectionItem() { Text= "Trả trước", Value= 1 }
            };
            return Ok(listForm);
        }
    }
}
