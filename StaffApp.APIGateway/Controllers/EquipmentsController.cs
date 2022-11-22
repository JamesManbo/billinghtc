using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.EquipmentModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EquipmentsController : CustomBaseController
    {
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IOutContractService _outContractService;
        public EquipmentsController(IEquipmentTypeService equipmentTypeService, IOutContractService outContractService)
        {
            _equipmentTypeService = equipmentTypeService;
            _outContractService = outContractService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<EquipmentDTO>>>> Get([FromQuery] RequestFilterModel filterModel)
        {
            var actResponse = await _equipmentTypeService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpGet("ContractEquipments")]
        public async Task<IActionResult> AutocompleteInstanceAsync([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(await _outContractService.AutocompleteInstanceAsync(filterModel));
        }
    }
}