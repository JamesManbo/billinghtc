using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Global.Configs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.TransactionsModel;
using StaffApp.APIGateway.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.CmsApiIdentityKey)]
    public class TransactionsController : CustomBaseController
    {

        private readonly ITransactionsService _transactionsService;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionsService transactionsService, IMapper mapper)
        {
            _transactionsService = transactionsService;
            _mapper = mapper;
        }

        [HttpPost("AddNewServicePackage")]
        public async Task<IActionResult> Post([FromBody] AddNewServicePackageTransactionApp req)
        {
            if (req.TransactionServicePackage==null)
            {
                return BadRequest();
            }

            var command = _mapper.Map<AddNewServicePackageTransactionCommand>(req);
            //command.Note = req.Note;
            //command.TransactionDate = req.TransactionDate;

            command.OutContractId = req.ContractId;
            command.StatusId = 1; //WaitApprove
            //command.HandleUserId = req.HandleUserId;

            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;
            //command.Code = Guid.NewGuid().ToString();

            var cuServicePackage = _mapper.Map<CUTransactionServicePackage>(req.TransactionServicePackage);
            //if (req.TransactionServicePackage.HasStartAndEndPoint)
            //{
            //    cuServicePackage.SpSubTotal =
            //        req.TransactionServicePackage.SpPackagePrice.Value *
            //        req.TransactionServicePackage.TimeLine.PaymentPeriod;
            //    cuServicePackage.SpGrandTotal =
            //        cuServicePackage.SpSubTotal +
            //        req.TransactionServicePackage.SpInstallationFee.Value;

            //    cuServicePackage.EpInstallationFee = req.TransactionServicePackage.EpInstallationFee.Value;
            //    cuServicePackage.EpPackagePrice = req.TransactionServicePackage.EpPackagePrice.Value;
            //    cuServicePackage.EpSubTotal =
            //        req.TransactionServicePackage.SpPackagePrice.Value *
            //        req.TransactionServicePackage.TimeLine.PaymentPeriod;
            //    cuServicePackage.EpGrandTotal =
            //        cuServicePackage.EpSubTotal +
            //        req.TransactionServicePackage.EpInstallationFee.Value;

            //    cuServicePackage.PackagePrice =
            //        req.TransactionServicePackage.SpPackagePrice.Value +
            //        req.TransactionServicePackage.EpPackagePrice.Value;
            //    cuServicePackage.InstallationFee =
            //        req.TransactionServicePackage.SpInstallationFee.Value +
            //        req.TransactionServicePackage.EpInstallationFee.Value;
            //    cuServicePackage.SubTotal =
            //        cuServicePackage.SpSubTotal + cuServicePackage.EpSubTotal;
            //}
            //else
            //{

            //    cuServicePackage.InstallationFee = req.TransactionServicePackage.InstallationFee.Value;
            //    cuServicePackage.PackagePrice = req.TransactionServicePackage.PackagePrice;
            //    cuServicePackage.SubTotal =
            //        req.TransactionServicePackage.PackagePrice *
            //        req.TransactionServicePackage.TimeLine.PaymentPeriod;
            //}

            var lstService = new List<CUTransactionServicePackage>();
            lstService.Add(cuServicePackage);
            command.TransactionServicePackages = lstService;

            var request = new RequestAddNewTransaction();
            request.TransactionJSON = JsonConvert.SerializeObject(command);
            request.TypeId = 1; //AddNewServicePackage


            var actResponse = await _transactionsService.AddNewTransaction(request);
            if (actResponse ==null) return BadRequest();
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest((actResponse.Errors!=null&&actResponse.Errors.Count>0)?actResponse.Errors.ElementAt(0).ErrorMessage:"");
            }
        }

        
        [HttpPost("AcceptancedTransaction")]
        public async Task<IActionResult> Post([FromBody] AcceptanceTransactionCommand command)
        {
            if (command.TransactionId <= 0)
            {
                return BadRequest();
            }
            command.AcceptanceStaff = UserIdentity.UniversalId;

            var actResponse = await _transactionsService.AcceptanceTransaction(command);
            if (actResponse!=null && actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }
    }
}
