using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation.Results;
using Global.Configs.Authentication;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Google.Protobuf.WellKnownTypes;
using HandlebarsDotNet;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StaffApp.APIGateway.Infrastructure.ValidationConfigs;
using StaffApp.APIGateway.Models.ApplicationUserModels;
using StaffApp.APIGateway.Models.ContractModels;
using StaffApp.APIGateway.Models.ContractModels.AppModel;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Global.Configs.Authentication.AuthenticationSchemes.CmsApiIdentityKey)]
    public class ContractsController : CustomBaseController
    {
        private readonly IOutContractService _outContractService;
        private readonly IOrganizationUnitsService _organizationUnitsService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IConverter _converter;
        private readonly IApplicationUsersService _applicationUsersService;
        public ContractsController(IOutContractService outContractService,
            IOrganizationUnitsService usersService,
            IApplicationUsersService applicationUsersService,
            IConfiguration configuration,
            IConverter converter,
            IMapper mapper)
        {
            _outContractService = outContractService;
            _organizationUnitsService = usersService;
            _configuration = configuration;
            _mapper = mapper;
            _converter = converter;
            _applicationUsersService = applicationUsersService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<ContractSimpleDTO>>>> GetAsync([FromQuery] ContractFilterModel filterModel)
        {
            filterModel.ServiceIds = _configuration.GetValue<int>("ServiceIdFTTH").ToString();
            var actResponse = await _outContractService.GetListContact(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IActionResponse<ContractDTO>>> GetAsync(int id)
        {
            var actResponse = await _outContractService.GetDetail(id);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpGet("GetDetailByCode")]
        public async Task<ActionResult<IActionResponse<ContractDTO>>> GetDetailByCode(string code)
        {
            var actResponse = await _outContractService.GetDetailByCode(code);
            if (actResponse == null)
            {
                return NotFound();
            }
            actResponse.AgentName = actResponse.AgentCode;
            //actResponse.Payment.MethodName = actResponse.Payment.Method==0? "Tiền mặt": actResponse.Payment.Method == 1 ? "Chuyển khoản":actResponse.Payment.Method == 0 ? "Bù trừ thanh toán" : "";
            return Ok(actResponse);
        }

        [HttpPost]
        public async Task<IActionResult> PostOutContract([FromBody] CreateOutContractApp request)
        {
            request.CreatedBy = UserIdentity.UserName;
            request.SignedUserId = UserIdentity.UniversalId;
            request.SignedUserName = UserIdentity.UserName;
            if (UserIdentity.Organizations.FirstOrDefault()!=null) {
                var ou = await _organizationUnitsService.GetByCode(UserIdentity.Organizations.FirstOrDefault());
                if (ou != null)
                {
                    request.OrganizationUnitId = ou.IdentityGuid;
                    request.OrganizationUnitCode = ou.Code;
                    request.OrganizationUnitName = ou.Name;
                }
            }

            var createOutContract = _mapper.Map<CreateOutContract>(request);

            CreateOutContractValidator validator = new CreateOutContractValidator();

            ValidationResult validateResult = validator.Validate(createOutContract);

            if (!validateResult.IsValid)
            {
                if (validateResult.Errors != null && validateResult.Errors.Count > 0)
                {
                    return BadRequest(validateResult.Errors.ElementAt(0).ErrorMessage);
                }
                else
                {
                    return BadRequest("error");
                }

            }

            createOutContract.SalesmanIdentityGuid = UserIdentity.UniversalId;
            createOutContract.CreatedBy = UserIdentity.UserName;
            createOutContract.CreatorUserId = UserIdentity.UniversalId;
            createOutContract.SalesmanId = UserIdentity.Id;
            foreach (var channel in createOutContract.ServicePackages)
            {
                channel.IsDefaultSLAByServiceId = 1;
            }

            var actResponse = await _outContractService.CreatedOutContract(new StringValue()
            {
                Value = JsonConvert.SerializeObject(createOutContract)
            }); ;

            if (actResponse != null && actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }

            return BadRequest(actResponse?.Message ?? "Có lỗi xảy ra");
        }

        [HttpGet("GetOutContractStatuses")]

        public async Task<IActionResult> GetOutContractStatuses()
        {
            var result = await _outContractService.GetOutContractStatuses();
            if (result == null)
            {
                return NotFound();
            }
            var allItem = new JObject();
            allItem.Add("Name", "Tất cả");
            allItem.Add("Id", 0);
            var rs = JArray.Parse(result);
            rs.AddFirst(allItem);
            return Ok(rs);
        }

        [Route("GenerateContractCode")]
        [HttpGet]
        public async Task<IActionResult> GenerateContractCodeAsync(bool isEnterprise, string customerFullName, int? marketAreaId = null, int? projectId = null, string srvPackageIds = "")
        {
            return Ok(await _outContractService.GenerateContractCode(isEnterprise, customerFullName, marketAreaId, projectId, srvPackageIds));
        }

        [HttpGet("GetServicePackagesOfContractor")]
        public async Task<ActionResult<IActionResponse<IEnumerable<OutContractServicePackageSimpleDTO>>>>
            GetServicePackagesOfContractor(int contractorId)
        {
            var filter = new ContractFilterModel
            {
                Paging = false,
                ContractorId = contractorId
            };
            var actResponse = await _outContractService.GetListContact(filter);
            if (actResponse == null || actResponse.Subset == null)
            {
                return NotFound();
            }

            var rs = new List<OutContractServicePackageSimpleDTO>();
            actResponse.Subset.ForEach(c =>
            {
                if (c.ServicePackages != null)
                {
                    c.ServicePackages.ForEach(s =>
                    {
                        s.ContractCode = c.ContractCode;
                        rs.Add(s);
                    });
                }
            });

            return Ok(rs);
        }

        [HttpGet("GetOutContractPrint")]

        public async Task<IActionResult> GetOutContractPrint(string code)
        {
            //await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            //await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            //{
            //    Headless = true,
            //    Args = new string[] { "--no-sandbox", "--disable-setuid-sandbox" },
            //});
            //await using var page = await browser.NewPageAsync();
            //await page.EmulateMediaTypeAsync(MediaType.Screen);
            //await page.SetContentAsync("<div><h1>Hello PDF world!</h1><h2 style='color: red; text-align: center;'>Greetings from <i>HTML</i> world</h2></div>");
            //var pdfContent = await page.PdfStreamAsync(new PdfOptions
            //{
            //    Format = PaperFormat.A4,
            //    PrintBackground = true
            //});
            //return Ok(File(pdfContent, "application/pdf"));
            //============
            var contractModel = await _outContractService.GetDetailByCode(code);
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
            //==================

            //// Create the itext pdf
            //MemoryStream stream = new MemoryStream();
            //PdfWriter writer = new PdfWriter(stream);
            //var pdf = new PdfDocument(writer);
            //var document = new Document(pdf);
            //document.Add(new Paragraph("Hello World!"));
            //document.Close();  // don't forget to close or the doc will be corrupt! ;)

            //// Load the mem stream into a StreamContent
            //HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new StreamContent(stream)
            //};

            //// Prep the response with headers, filenames, etc.
            //httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            //{
            //    FileName = "WebApi2GeneratedFile.pdf"
            //};

            //httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            //ResponseMessageResult responseMessageResult = ResponseMessage(httpResponseMessage);

            //// Cross your fingers...
            //return responseMessageResult;
        }
    }
}