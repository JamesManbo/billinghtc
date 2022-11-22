using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.APIGateway.Models.ApplicationUserModels;
using CustomerApp.APIGateway.Models.OutContract;
using CustomerApp.APIGateway.Models.RequestModels;
using CustomerApp.APIGateway.Services;
using Global.Configs.Authentication;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Google.Protobuf.WellKnownTypes;
using HandlebarsDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CustomerApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContractsController : CustomerBaseController
    {
        private readonly IContractService _contractService;
        private readonly IContractorService _contractorService;
        private readonly IApplicationUsersService _applicationUsersService;
        public ContractsController(IContractService service, IContractorService contractorService,
            IApplicationUsersService applicationUsersService)
        {
            _contractService = service;
            _contractorService = contractorService;
            _applicationUsersService = applicationUsersService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IActionResponse<ContractDTO>>> GetById(int id)
        {
            var actResponse = await _contractService.GetDetail(id);
            if (actResponse==null)
            {
                return NotFound();
            }

           return Ok(actResponse);


        }
        
        [HttpGet("GetListContract")]
        public async Task<ActionResult<IActionResponse<IPagedList<ContractSimpleDTO>>>> GetListContract()
        {
            var contractor = await _contractorService.GetContactorByIdentity(UserIdentity.UniversalId);
            if (contractor == null) return NotFound();

            var filter = new ContractFilterModel
            {
                Paging = false,
                ContractorId = contractor.Id
            };
            var actResponse = await _contractService.GetListContact(filter);
            if (actResponse==null)
            {
                return NotFound();
            }

           return Ok(actResponse);


        }
        
        [HttpGet("GetServicePackagesOfContracts")]
        public async Task<ActionResult<IActionResponse<IEnumerable<OutContractServicePackageSimpleDTO>>>> GetServicePackagesOfContracts()
        {
            var contractor = await _contractorService.GetContactorByIdentity(UserIdentity.UniversalId);
            if (contractor == null) return NotFound();

            var filter = new ContractFilterModel
            {
                Paging = false,
                ContractorId = contractor.Id
            };
            var actResponse = await _contractService.GetListContact(filter);
            if (actResponse==null||actResponse.Subset==null)
            {
                return NotFound();
            }

            var rs = new List<OutContractServicePackageSimpleDTO>();
            actResponse.Subset.ForEach(c=> {
                if (c.ServicePackages != null)
                {
                    c.ServicePackages.ForEach(s=> {
                        s.ContractCode = c.ContractCode;
                        rs.Add(s);
                    });
                }
            });

           return Ok(rs);
        }

        [HttpGet("ContractEquipments")]
        [AllowAnonymous]
        public async Task<IActionResult> AutocompleteInstanceAsync([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(await _contractService.AutocompleteInstanceAsync(filterModel));
        }


        [HttpGet("GetOutContractPrint")]

        public async Task<IActionResult> GetOutContractPrint(int id)
        {
            var contractModel = await _contractService.GetDetail(id);
            if (contractModel == null)
            {
                return BadRequest();
            }
            var applicationUser = JsonConvert.DeserializeObject<ApplicationUserDTO>(await _applicationUsersService.GetApplicationUserByUid(new StringValue { Value = contractModel.Contractor.ApplicationUserIdentityGuid }));

            string invoiceHtml;

                string templateHtml = System.IO.File.ReadAllText("Assets/Contract.html");
                Handlebars.RegisterHelper("ifneq", (output, options, context, arguments) =>
                {
                    if (arguments.Length == 2)
                    {
                        if (!arguments.ElementAt(0).Equals(arguments.ElementAt(1))) options.Template(output, context);
                        else options.Inverse(output, context);
                    }
                    else
                    {
                        options.Inverse(output, context);
                    }
                });
                Handlebars.RegisterHelper("ifeq", (output, options, context, arguments) =>
                {
                    if (arguments.Length == 2)
                    {
                        if (arguments.ElementAt(0).Equals(arguments.ElementAt(1))) options.Template(output, context);
                        else options.Inverse(output, context);
                    }
                    else
                    {
                        options.Inverse(output, context);
                    }
                });
                Handlebars.RegisterHelper("inc", (output, options, context, arguments) =>
                {
                    if (arguments.Length == 1)
                    {
                        output.Write(((int)arguments.ElementAt(0) + 1));
                        options.Template(output, context);
                    }
                    else
                    {
                        options.Inverse(output, context);
                    }
                });
                var template = Handlebars.Compile(templateHtml);
                var data = new
                {
                    contractModel = contractModel,
                    applicationUser = applicationUser
                };

                //string invoiceHtml = template(data);
                invoiceHtml = template(data);


            return Ok(invoiceHtml);
        }
    }
}