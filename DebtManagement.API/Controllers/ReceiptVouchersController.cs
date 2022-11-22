using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.API.Grpc.Clients.StaticResource;
using DebtManagement.API.PolicyBasedAuthProvider;
using DebtManagement.API.Services;
using DebtManagement.Domain.Commands.DebtCommand;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Models.FilterModels;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DebtManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReceiptVouchersController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly IReceiptVoucherQueries _receiptVoucherQueries;
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly IUserGrpcService _userGrpcService;
        private readonly IReceiptVoucherService _receiptVoucherService;
        private readonly IFeedbackGrpcService _feedbackGrpcService;
        private readonly IContractGrpcService _contractGrpcService;

        public ReceiptVouchersController(
            IMediator mediator,
            IReceiptVoucherQueries receiptVoucherQueries,
            IReceiptVoucherRepository receiptVoucherRepository,
            IUserGrpcService userGrpcService,
            IReceiptVoucherService receiptVoucherService,
            IFeedbackGrpcService feedbackGrpcService,
            IContractGrpcService contractGrpcService)
        {
            _mediator = mediator;
            _receiptVoucherQueries = receiptVoucherQueries;
            _receiptVoucherRepository = receiptVoucherRepository;
            _userGrpcService = userGrpcService;
            _receiptVoucherService = receiptVoucherService;
            _feedbackGrpcService = feedbackGrpcService;
            this._contractGrpcService = contractGrpcService;
        }

        [HttpGet("printing-data-source")]
        public async Task<IActionResult> GetSourceForPrinting([FromQuery] string voucherIds)
        {
            if (string.IsNullOrEmpty(voucherIds))
            {
                return BadRequest();
            }

            return Ok(await _receiptVoucherQueries.GetForPrinting(voucherIds));
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ReceiptVoucherBothProjectNullFilterModel filterModel)
        {
            if (!UserIdentity.Permissions.Contains("VIEW_ALL_RECEIPT_VOUCHER"))
            {
                filterModel.UserId = UserIdentity.UniversalId;
            }

            var result = _receiptVoucherQueries.GetList(filterModel);
            var cIds = result.Subset.SelectMany(c => c.ReceiptLines).Where(r => !string.IsNullOrEmpty(r.CId)).Select(d => d.CId);

            if (cIds != null && cIds.Any())
            {
                var countingFeedbacks = await _feedbackGrpcService.CountFeedbackByCIds(cIds.ToArray());
                if (countingFeedbacks != null && countingFeedbacks.Any())
                {
                    foreach (var item in result.Subset)
                    {
                        var countingItem = countingFeedbacks
                            .FirstOrDefault(r =>
                                item.ReceiptLines.Any(t =>
                                    !string.IsNullOrWhiteSpace(t.CId)
                                    && t.CId.Equals(r.Item1, StringComparison.OrdinalIgnoreCase)));

                        if (!countingItem.Equals(default))
                        {
                            item.FeedbackCount = countingItem.Item2;
                        }
                    }
                }
            }

            return Ok(result);
        }

        [HttpGet("GetListByTargetId")]
        public async Task<IActionResult> GetListByTargetId(int targetId, int clearingId)
        {
            var result = _receiptVoucherQueries.GetListByTargetId(targetId, clearingId);
            var cIds = result.SelectMany(c => c.ReceiptLines).Select(d => d.CId);

            if (cIds != null && cIds.Any())
            {
                var countingFeedbacks = await _feedbackGrpcService.CountFeedbackByCIds(cIds.ToArray());
                foreach (var item in result)
                {
                    item.FeedbackCount = countingFeedbacks
                        .First(r => item.ReceiptLines.Any(t => t.CId.Equals(r.Item1, StringComparison.OrdinalIgnoreCase))).Item2;
                }
            }

            return Ok(result);
        }

        [HttpGet("GetListByTargetUid")]
        public async Task<IActionResult> GetListByTargetUid(string targetId, int clearingId)
        {
            var result = _receiptVoucherQueries.GetListByTargetUid(targetId, clearingId);
            var cIds = result.SelectMany(c => c.ReceiptLines).Select(d => d.CId);

            if (cIds != null && cIds.Any())
            {
                var countingFeedbacks = await _feedbackGrpcService.CountFeedbackByCIds(cIds.ToArray());
                foreach (var item in result)
                {
                    item.FeedbackCount = countingFeedbacks
                        .First(r => item.ReceiptLines.Any(t => t.CId.Equals(r.Item1, StringComparison.OrdinalIgnoreCase))).Item2;
                }
            }

            return Ok(result);
        }

        [HttpGet, Route("GenerateVoucherCode")]
        public async Task<IActionResult> GenerateVoucherCode(DateTime issuedDate, int? projectId = null, int? marketAreaId = null, bool isEnterprise = false)
        {
            if (issuedDate == null || issuedDate == DateTime.MinValue)
            {
                issuedDate = DateTime.Now;
            }
            return Ok(await _receiptVoucherService.GenerateReceiptVoucherCode(issuedDate, projectId, marketAreaId, isEnterprise));
        }

        [HttpGet, Route("GetLatestIndex")]
        public IActionResult GetLatestIndex()
        {
            return Ok(_receiptVoucherQueries.GetOrderNumberByNow());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReceiptVoucher(int id)
        {
            var receiptVoucher = _receiptVoucherQueries.Find(id);
            if (receiptVoucher == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(receiptVoucher.CreatedUserId))
                receiptVoucher.CreatedUserFullName = await _userGrpcService.GetByUid(receiptVoucher.CreatedUserId);

            var allOfCids = receiptVoucher.ReceiptLines.Where(r => !string.IsNullOrWhiteSpace(r.CId)).Select(r => r.CId);
            if (allOfCids.Count() > 0)
            {
                var channelAddresses = await this._contractGrpcService.GetChannelAddresses(allOfCids.ToArray());
                foreach (var receiptVoucherLine in receiptVoucher.ReceiptLines)
                {
                    var address = channelAddresses.FirstOrDefault(c => c.Cid.Equals(receiptVoucherLine.CId, StringComparison.OrdinalIgnoreCase));
                    receiptVoucherLine.StartPointAddress = address?.StartPointAddress;
                    receiptVoucherLine.EndPointAddress = address?.EndPointAddress;
                }
            }

            return Ok(receiptVoucher);
        }

        [HttpGet("PrintVoucherById/{id}")]
        public async Task<IActionResult> PrintVoucherById(int id)
        {
            var receiptVoucher = _receiptVoucherQueries.PrintVoucherById(id);

            if (!string.IsNullOrWhiteSpace(receiptVoucher.CreatedUserId))
                receiptVoucher.CreatedUserFullName = await _userGrpcService.GetByUid(receiptVoucher.CreatedUserId);

            if (receiptVoucher == null)
            {
                return NotFound();
            }

            return Ok(receiptVoucher);
        }

        [HttpGet("FinancialReport")]
        public IActionResult FinancialReport([FromQuery] ReceiptVoucherFilterModel filterModel)
        {
            return Ok(_receiptVoucherQueries.FinancialReport(filterModel));
        }

        //[HttpGet("GetTotalSharingRevenusByReceiptVoucher")]
        //public IActionResult GetTotalSharingRevenusByReceiptVoucher(string Ids, string startBillingDate, string endBillingDate, int currencyUnitId, int paymentVoucherId)
        //{
        //    if (string.IsNullOrEmpty(Ids))
        //        return BadRequest();

        //    var IdArr = Ids.Split(",");
        //    return Ok(
        //        _receiptVoucherQueries
        //            .GetTotalSharingRevenusByReceiptVoucher(IdArr, startBillingDate, endBillingDate, currencyUnitId, paymentVoucherId));
        //}

        [HttpGet("GetTotalSharingRevenusByReceiptVoucher")]
        public IActionResult GetTotalSharingRevenusByReceiptVoucher(int inContractId, string startBillingDate, string endBillingDate, int currencyUnitId, int paymentVoucherId, int skip)
        {
            return Ok(
                _receiptVoucherQueries
                    .GetTotalSharingRevenusByReceiptVoucher(inContractId, startBillingDate, endBillingDate, currencyUnitId, paymentVoucherId, skip));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReceiptVoucherCommand createReceiptVoucherCommand)
        {
            createReceiptVoucherCommand.CreatedDate = DateTime.Now;
            createReceiptVoucherCommand.CreatedUserId = UserIdentity.UniversalId;
            createReceiptVoucherCommand.CreatedUserFullName = UserIdentity.FullName;
            createReceiptVoucherCommand.CreatedBy = UserIdentity.UserName;

            createReceiptVoucherCommand.UpdatedBy = UserIdentity.UserName;
            var actResponse = await _mediator.Send(createReceiptVoucherCommand);

            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }

            return BadRequest(actResponse);
        }

        [HttpPut("BadReceiptVoucher")]
        public async Task<IActionResult> Create([FromBody] BadReceiptVoucherCommand badReceiptVoucherCommand)
        {
            badReceiptVoucherCommand.UpdatedBy = $"{UserIdentity.FullName}({UserIdentity.UserName})";

            var actResponse = await _mediator.Send(badReceiptVoucherCommand);

            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }

            return BadRequest(actResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,
            [FromBody] UpdateReceiptVoucherCommand updateReceiptVoucherCommand)
        {
            if (id != updateReceiptVoucherCommand.Id)
            {
                return BadRequest();
            }

            updateReceiptVoucherCommand.UpdatedBy = $"{UserIdentity.FullName}({UserIdentity.UserName})";
            var actionResponse = await _mediator.Send(updateReceiptVoucherCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPut("CancelReceiptVoucher/{id}")]
        public async Task<IActionResult> CancelReceiptVoucher(int id,
            [FromBody] CancelReceiptVoucherCommand cancelReceiptVoucherCommand)
        {
            if (id != cancelReceiptVoucherCommand.Id)
            {
                return BadRequest();
            }

            cancelReceiptVoucherCommand.UpdatedBy = $"{UserIdentity.FullName}({UserIdentity.UserName})";
            var actionResponse = await _mediator.Send(cancelReceiptVoucherCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPut("CancelListReceiptVoucher")]
        public async Task<IActionResult> CancelListReceiptVoucher(
            [FromBody] CancelListReceiptVoucherCommand cancelListReceiptVoucherCommand)
        {
            cancelListReceiptVoucherCommand.UpdatedBy = $"{UserIdentity.FullName}({UserIdentity.UserName})";
            var actionResponse = await _mediator.Send(cancelListReceiptVoucherCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        /// <summary>
        /// Chức năng xét duyệt phiếu thu đã thu được tiền từ nhân viên kế toán
        /// </summary>
        /// <param name="rcptVoucherIds"></param>
        /// <returns></returns>
        [HttpPut("accountant-confirmation")]
        [PermissionAuthorize("CONFIRM_COLLECTION_ON_BEHALF_DEBT")]
        public async Task<IActionResult> ConfirmCollectionOnBehalfDebt(int[] rcptVoucherIds)
        {
            if (rcptVoucherIds == null || rcptVoucherIds.Length == 0)
            {
                return BadRequest();
            }

            var confirmationCommand = new ConfirmCollectingDebtCommand()
            {
                ReceiptVoucherIds = rcptVoucherIds,
                ApprovedUserId = UserIdentity.UniversalId,
                ConfirmationDate = DateTime.UtcNow
            };

            var confirmationResp = await _mediator.Send(confirmationCommand);
            if (confirmationResp.IsSuccess)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var receiptVoucher = _receiptVoucherQueries.Find(id);
            if (receiptVoucher == null)
            {
                return NotFound();
            }

            var deleteResponse = _receiptVoucherRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }

        [HttpPost("Import")]
        public async Task<IActionResult> Import([FromForm] IFormFile formFiles)
        {
            if (formFiles == null || formFiles.Length == 0)
            {
                return NotFound();
            }

            if (!Path.GetExtension(formFiles.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Định dạng của tệp không được hỗ trợ");
            }

            var importReceiptVoucherCommand = new ImportReceiptVoucherCommand(formFiles)
            {
                InvoicedUser = UserIdentity.UserName
            };

            var actionResponse = await _mediator.Send(importReceiptVoucherCommand);

            return Ok(actionResponse);
        }

        [HttpGet("GetListTotalRevenue")]
        public IActionResult GetListTotalRevenue([FromQuery] ReportTotalRevenueFilter reportTotalRevenueFilter)
        {
            return Ok(_receiptVoucherQueries.GetListTotalRevenue(reportTotalRevenueFilter));
        }

        [HttpGet("GetReceiptVoucherIds")]
        public IActionResult GetReceiptVoucherIds(string outContractCode)
        {
            return Ok(_receiptVoucherQueries.GetReceiptVoucherIds(outContractCode));
        }
    }
}