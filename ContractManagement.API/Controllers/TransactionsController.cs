using System;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.API.PolicyBasedAuthProvider;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.MultipleTransactionCommand;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.Models.Notification;
using ContractManagement.Infrastructure.Queries;
using Global.Configs.SystemArgument;
using Global.Models.Filter;
using Global.Models.Notification;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionsController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly ITransactionQueries _transactionQueries;
        private readonly INotificationGrpcService _notificationGrpcService;
        private readonly IOrganizationUnitGrpcService _organizationGrpcService;
        public readonly DepartmentCode DepartmentCode;

        public const string EDIT_TRANSACTION_PERMISSION = "EDIT_TRANSACTION";

        public TransactionsController(IMediator mediator,
            ITransactionQueries transactionQueries,
            INotificationGrpcService notificationGrpcService,
            IOrganizationUnitGrpcService organizationGrpcService,
            IConfiguration config)
        {
            _mediator = mediator;
            _transactionQueries = transactionQueries;
            _notificationGrpcService = notificationGrpcService;
            this._organizationGrpcService = organizationGrpcService;
            DepartmentCode = config.GetSection("DepartmentCode").Get<DepartmentCode>();
        }

        [HttpGet("GenerateTransactionCode")]
        public IActionResult GetTransactionCode(string contractCode, bool isAppendix)
        {
            var response = new ActionResponse<string>();
            response.SetResult(_transactionQueries.GenerateTransactionCode(contractCode, isAppendix));
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] TransactionRequestFilterModel filterModel)
        {
            var alowedDepartments = await _organizationGrpcService.GetChildrenByCode(DepartmentCode.SupporterDepartmentCode);
            return Ok(_transactionQueries.GetList(filterModel, alowedDepartments));
        }

        [HttpGet("GetByCode")]
        public async Task<IActionResult> GetTransactionByCode(string code)
        {
            var decodedCode = Uri.UnescapeDataString(code);
            var alowedDepartments = await _organizationGrpcService.GetChildrenByCode(DepartmentCode.SupporterDepartmentCode);
            var transaction = _transactionQueries.FindByCode(decodedCode, alowedDepartments);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var alowedDepartments = await _organizationGrpcService.GetChildrenByCode(DepartmentCode.SupporterDepartmentCode);
            var transaction = _transactionQueries.Find(id, alowedDepartments);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpGet("TransactionStatus")]
        public IActionResult GetTransactionStatus()
        {
            return Ok(TransactionStatus.SelectionList());
        }

        [HttpGet("TransactionType")]
        public IActionResult GetTransactionType()
        {
            return Ok(TransactionType.SelectionList());
        }

        [HttpPost("ApprovedTransactions")]
        [PermissionAuthorize("APPROVED_TRANSACTION")]
        public async Task<IActionResult> ApprovedTransactions([FromBody] CUApprovedTransactionSimplesCommand cUApprovedTransactionSimplesCommand)
        {
            if (cUApprovedTransactionSimplesCommand.TransactionSimpleCommands == null
                || cUApprovedTransactionSimplesCommand.TransactionSimpleCommands.Count == 0)
            {
                return BadRequest();
            }

            foreach (var item in cUApprovedTransactionSimplesCommand.TransactionSimpleCommands)
            {
                if (!UserIdentity.Permissions.Any(p => p.Equals(TransactionType.GetTypePermission(item.Type), StringComparison.OrdinalIgnoreCase)))
                {
                    return BadRequest("Bạn không có quyền nghiệm thu phụ lục này");
                }
            }

            cUApprovedTransactionSimplesCommand.AcceptanceStaff = UserIdentity.UserName;
            var actResponse = await _mediator.Send(cUApprovedTransactionSimplesCommand);

            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost("CancelledTransactions")]
        [PermissionAuthorize("CANCELLED_TRANSACTION")]
        public async Task<IActionResult> CancelledTransactions([FromBody] CUCancelledTransactionSimplesCommand cUCancelledTransactionSimplesCommand)
        {
            if (cUCancelledTransactionSimplesCommand.TransactionIds == null
                || cUCancelledTransactionSimplesCommand.TransactionIds.Length == 0)
            {
                return BadRequest();
            }
            cUCancelledTransactionSimplesCommand.AcceptanceStaff = UserIdentity.UserName;

            var actResponse = await _mediator.Send(cUCancelledTransactionSimplesCommand);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("DeployNewChannelTransaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateDeployNewOutContractCommand command)
        {
            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0 && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }

            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("AddNewServicePackageTransaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] CUAddNewServicePackageTransaction command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("ADD_NEW_SERVICE_PACKAGE_OUT_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0 && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }

            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("ChangeServicePackageTransaction")]
        public async Task<IActionResult> ChangeServicePackageTransaction([FromBody] CUChangeServicePackageTransaction command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("CHANGE_SERVICE_PACKAGE_OUT_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0 && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }

            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("TransactionSuspendServicePackages")]
        public async Task<IActionResult> TransactionSuspendServicePackages([FromBody] CUTransactionSuspendServicePackagesCommand command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("SUSPEND_SERVICE_PACKAGE_OUT_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0 &&
                (command.OutContractIds == null || command.OutContractIds.Count == 0)
                && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }
            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("TransactionRestoreServicePackages")]
        public async Task<IActionResult> TransactionRestoreServicePackages([FromBody] CUTransactionRestoreServicePackagesCommand command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("RESTORE_SERVICE_PACKAGE_OUT_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0
                && (command.OutContractIds == null || command.OutContractIds.Count == 0)
                && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }
            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("TransactionTerminateServicePackages")]
        public async Task<IActionResult> TransactionTerminateServicePackages([FromBody] CUTransactionTerminateServicePackagesCommand command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("TERMINATE_SERVICE_PACKAGE_OUT_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0 && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }
            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("ChangeLocationServicePackages")]
        public async Task<IActionResult> ChangeLocationServicePackages([FromBody] CUChangeLocationServicePackagesCommand command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("CHANGE_LOCATION_OUT_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0 && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }
            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("TerminateContract")]
        public async Task<IActionResult> TerminateContract([FromBody] CUTerminateContractCommand command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("TERMINATE_OUT_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0 && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }
            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("ChangeEquipments")]
        public async Task<IActionResult> ChangeEquipments([FromBody] CUChangeEquipmentsCommand command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("CHANGE_EQUIPMENT_OUT_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0 && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }
            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("UpgradeEquipments")]
        public async Task<IActionResult> UpgradeEquipments([FromBody] CUUpgradeEquipmentsCommand command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("UPGRADE_EQUIPMENTS_OUT_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0
                && (command.OutContractIds == null || command.OutContractIds.Count == 0)
                && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }
            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("ReclaimEquipments")]
        public async Task<IActionResult> ReclaimEquipments([FromBody] CUReclaimEquipmentsCommand command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("RECLAIM_EQUIPMENT_OUT_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0 && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }
            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("UpgradeBandwidths")]
        public async Task<IActionResult> UpgradeBandwidths([FromBody] CUUpgradeBandwidthsCommand command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("UPGRADE_BANDWIDTH_OUT_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0
                && (command.OutContractIds == null || command.OutContractIds.Count == 0)
                && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }
            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        [Route("RenewContract")]
        public async Task<IActionResult> RenewContract([FromBody] CURenewContractCommand command)
        {
            if (command.Id == 0 && !UserIdentity.Permissions.Contains("RENEW_CONTRACT"))
                return Forbid();

            if (command.Id > 0 && !UserIdentity.Permissions.Contains(EDIT_TRANSACTION_PERMISSION))
                return Forbid();

            if (command.OutContractId <= 0
                && command.InContractId <= 0)
            {
                return BadRequest("Hợp đồng muốn thực hiện không hợp lệ");
            }

            command.CreatedBy = UserIdentity.UserName;
            command.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(command);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpGet]
        [Route("GetCurrentPendingTaskByProject")]
        public IActionResult GetCurrentPendingTaskByProject(int marketId, DateTime startDate, DateTime endDate)
        {
            return Ok(_transactionQueries.GetCurrentPendingTaskByProject(marketId, startDate, endDate));
        }

        [HttpPost]
        [Route("TestPushNotification")]
        public async Task<IActionResult> TestPushNotification([FromBody] string uid)
        {
            if (string.IsNullOrEmpty(uid)) return BadRequest();
            var notiReq = new PushNotificationRequest()
            {
                Zone = NotificationZone.Contract,
                Type = NotificationType.App,
                Category = NotificationCategory.None,
                Title = "Hello",
                Content = "qq",
                Payload = JsonConvert.SerializeObject(new { Category = NotificationCategory.None }, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })
            };

            var rs = await _notificationGrpcService.PushNotificationByUids(notiReq, uid);
            if (rs) return Ok("");
            return BadRequest();
        }

        [HttpPost]
        [Route("ConfirmInOutEquipment")]
        [PermissionAuthorize("CONFIRM_IN_OUT_EQUIPMENT")]
        public async Task<IActionResult> ConfirmInOutEquipment([FromBody] CUTransactionCommand cUTransactionCommand)
        {
            var actionResponse = await _mediator.Send(cUTransactionCommand);
            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPost("TechnicalAcceptance")]
        [PermissionAuthorize("TECHNICAL_ACCEPTANCE")]
        public async Task<IActionResult> TechnicalAcceptance([FromBody] AcceptanceTransactionCommandApp acceptanceTransactionCommandApp)
        {
            acceptanceTransactionCommandApp.AcceptanceStaff = UserIdentity.UniversalId;

            var actResponse = await _mediator.Send(acceptanceTransactionCommandApp);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost("MultipleUpgradePackage")]
        public async Task<IActionResult> MultiUpgradePackage([FromBody] MultipleUpgradePackageCommand upgradePackageCommand)
        {
            upgradePackageCommand.CreatedBy = UserIdentity.UserName;
            upgradePackageCommand.CreatorUserId = UserIdentity.UniversalId;
            var transactionResponse = await _mediator.Send(upgradePackageCommand);
            if (!transactionResponse.IsSuccess) return BadRequest(transactionResponse);
            return Ok(transactionResponse);
        }

        [HttpPost("MultipleSuspendChannel")]
        public async Task<IActionResult> MultipleSuspendChannel([FromBody] MultipleSuspendChannelCommand multiSuspendCommand)
        {
            multiSuspendCommand.CreatedBy = UserIdentity.UserName;
            multiSuspendCommand.CreatorUserId = UserIdentity.UniversalId;
            var transactionResponse = await _mediator.Send(multiSuspendCommand);
            if (!transactionResponse.IsSuccess) return BadRequest(transactionResponse);
            return Ok(transactionResponse);
        }

        [HttpPost("MultipleRestoreChannel")]
        public async Task<IActionResult> MultipleRestoreChannel([FromBody] MultipleRestoreChannelCommand multiRestoreCommand)
        {
            multiRestoreCommand.CreatedBy = UserIdentity.UserName;
            multiRestoreCommand.CreatorUserId = UserIdentity.UniversalId;

            var transactionResponse = await _mediator.Send(multiRestoreCommand);
            if (!transactionResponse.IsSuccess) return BadRequest(transactionResponse);

            return Ok(transactionResponse);
        }

        [HttpPost("MultipleTerminateService")]
        public async Task<IActionResult> MultipleTerminateService([FromBody] MultipleTerminateServiceCommand multiTerminateSrvCommand)
        {
            multiTerminateSrvCommand.CreatedBy = UserIdentity.UserName;
            multiTerminateSrvCommand.CreatorUserId = UserIdentity.UniversalId;

            var transactionResponse = await _mediator.Send(multiTerminateSrvCommand);
            if (!transactionResponse.IsSuccess) return BadRequest(transactionResponse);

            return Ok(transactionResponse);
        }

        [HttpPost("MultipleReclaimEquipment")]
        public async Task<IActionResult> MultipleReclaimEquipment([FromBody] MultipleReclaimEquipmentCommand multiReclaimEquipmentCmd)
        {
            multiReclaimEquipmentCmd.CreatedBy = UserIdentity.UserName;
            multiReclaimEquipmentCmd.CreatorUserId = UserIdentity.UniversalId;

            var transactionResponse = await _mediator.Send(multiReclaimEquipmentCmd);
            if (!transactionResponse.IsSuccess) return BadRequest(transactionResponse);

            return Ok(transactionResponse);
        }

        [HttpPost("MultipleChangeEquipment")]
        public async Task<IActionResult> MultipleChangeEquipment([FromBody] MultipleChangeEquipmentCommand multiChangeEquipmentCmd)
        {
            multiChangeEquipmentCmd.CreatedBy = UserIdentity.UserName;
            multiChangeEquipmentCmd.CreatorUserId = UserIdentity.UniversalId;

            var transactionResponse = await _mediator.Send(multiChangeEquipmentCmd);
            if (!transactionResponse.IsSuccess) return BadRequest(transactionResponse);

            return Ok(transactionResponse);
        }
    }
}