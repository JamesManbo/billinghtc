using System;
using System.Linq;
using System.Threading.Tasks;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.API.PolicyBasedAuthProvider;
using DebtManagement.API.Services;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using DebtManagement.Domain.Models.ContractModels;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.Infrastructure.Repositories;
using FluentValidation;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DebtManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentVouchersController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly IPaymentVoucherRepository _paymentVoucherRepository;
        private readonly IPaymentVoucherQueries _paymentVoucherQueries;
        private readonly IContractGrpcService _outContractService;
        private readonly IWrappedConfigAndMapper _wrappedConfigAndMapper;
        private readonly IPaymentVoucherService _paymentVoucherService;
        private readonly IContractGrpcService _contractGrpcService;

        public PaymentVouchersController(IMediator mediator,
            IPaymentVoucherRepository paymentVoucherRepository,
            IPaymentVoucherQueries paymentVoucherQueries,
            IContractGrpcService outContractGrpcService,
            IWrappedConfigAndMapper wrappedConfigAndMapper,
            IPaymentVoucherService paymentVoucherService,
            IContractGrpcService contractGrpcService)
        {
            _mediator = mediator;
            _paymentVoucherRepository = paymentVoucherRepository;
            _paymentVoucherQueries = paymentVoucherQueries;
            _outContractService = outContractGrpcService;
            _wrappedConfigAndMapper = wrappedConfigAndMapper;
            _paymentVoucherService = paymentVoucherService;
            this._contractGrpcService = contractGrpcService;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] PaymentFilterModel paymentFilterModel)
        {
            return Ok(_paymentVoucherQueries.GetList(paymentFilterModel));
        }

        [HttpGet("GetListByTargetId")]
        public IActionResult GetListByTargetId(int targetId, int clearingId)
        {
            return Ok(_paymentVoucherQueries.GetListByTargetId(targetId, clearingId));
        }

        [HttpGet("GetListByTargetUid")]
        public IActionResult GetListByTargetUid(string targetId, int clearingId)
        {
            return Ok(_paymentVoucherQueries.GetListByTargetUid(targetId, clearingId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentVoucher(int id)
        {
            var paymentVoucher = _paymentVoucherQueries.Find(id);
            if (paymentVoucher == null)
            {
                return NotFound();
            }

            var allOfCids = paymentVoucher.PaymentVoucherDetails
                .Where(r => !string.IsNullOrWhiteSpace(r.CId))
                .Select(r => r.CId);
            if (allOfCids.Count() > 0)
            {
                var channelAddresses = await this._contractGrpcService.GetChannelAddresses(allOfCids.ToArray());
                foreach (var receiptVoucherLine in paymentVoucher.PaymentVoucherDetails)
                {
                    var address = channelAddresses.FirstOrDefault(c => c.Cid.Equals(receiptVoucherLine.CId, StringComparison.OrdinalIgnoreCase));
                    receiptVoucherLine.StartPointAddress = address?.StartPointAddress;
                    receiptVoucherLine.EndPointAddress = address?.EndPointAddress;
                }
            }

            return Ok(paymentVoucher);
        }

        [HttpGet("FinancialReport")]
        public IActionResult FinancialReport([FromQuery] PaymentFilterModel filterModel)
        {
            return Ok(_paymentVoucherQueries.FinancialReport(filterModel));
        }

        [HttpGet, Route("GetLatestIndex")]
        public IActionResult GetLatestIndex()
        {
            return Ok(_paymentVoucherQueries.GetOrderNumberByNow());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentVoucherCommand createPaymentVoucherCommand)
        {
            createPaymentVoucherCommand.CreatedBy = $"{UserIdentity.FullName}({UserIdentity.UserName})";
            var actResponse = await _mediator.Send(createPaymentVoucherCommand);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }

            return BadRequest(actResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,
            [FromBody] UpdatePaymentVoucherCommand updatePaymentVoucherCommand)
        {
            if (id != updatePaymentVoucherCommand.Id)
            {
                return BadRequest();
            }

            updatePaymentVoucherCommand.UpdatedBy = $"{UserIdentity.FullName}({UserIdentity.UserName})";
            var actionResponse = await _mediator.Send(updatePaymentVoucherCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPut("CancelPaymentVoucher/{id}")]
        public async Task<IActionResult> CancelPaymentVoucher(int id,
            [FromBody] CancelPaymentVoucherCommand cancelPaymentVoucherCommand)
        {
            if (id != cancelPaymentVoucherCommand.Id)
            {
                return BadRequest();
            }

            cancelPaymentVoucherCommand.UpdatedBy = $"{UserIdentity.FullName}({UserIdentity.UserName})";
            cancelPaymentVoucherCommand.UpdatedByUserId = UserIdentity.UniversalId;
            var actionResponse = await _mediator.Send(cancelPaymentVoucherCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var paymentVoucher = _paymentVoucherQueries.Find(id);
            if (paymentVoucher == null)
            {
                return NotFound();
            }

            var deleteResponse = _paymentVoucherRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }

        [HttpPut("CancelListPaymentVoucher")]
        public async Task<IActionResult> CancelListPaymentVoucher(
            [FromBody] CancelListPaymentVoucherCommand cancelListPaymentVoucherCommand)
        {
            cancelListPaymentVoucherCommand.UpdatedBy = $"{UserIdentity.FullName}({UserIdentity.UserName})";
            cancelListPaymentVoucherCommand.UpdatedByUserId = UserIdentity.UniversalId;
            var actionResponse = await _mediator.Send(cancelListPaymentVoucherCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPut("accountant-payment-confirmation")]
        [PermissionAuthorize("PAYMENT_CONFIRMATION")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentConfirmCommand command)
        {
            var approvalUser = UserIdentity.UniversalId;
            var actResp = await _paymentVoucherRepository.ConfirmPayment(command.PaymentVoucherIds, command.Approved ?? true, approvalUser, command.ReasonContent);
            if (actResp.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(actResp);
        }

        [HttpGet, Route("GenerateVoucherCode")]
        public async Task<IActionResult> GenerateVoucherCode(int? projectId = null, int? marketAreaId = null)
        {
            return Ok(await _paymentVoucherService.GeneratePaymentVoucherCode(projectId, marketAreaId));
        }

        [HttpGet("GetPaymentVoucherIds")]
        public IActionResult GetPaymentVoucherIds(string inContractCode)
        {
            return Ok(_paymentVoucherQueries.GetPaymentVoucherIds(inContractCode));
        }
    }

    public class PaymentConfirmCommand
    {
        public int[] PaymentVoucherIds { get; set; }
        public bool? Approved { get; set; } = true;
        public string? ReasonContent { get; set; }
    }
}