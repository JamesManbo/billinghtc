using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Grpc.StaticResource;
using ContractManagement.Domain.Commands.ServiceCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Validations;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Queries.ServiceQueries;
using ContractManagement.Infrastructure.Repositories.PictureRepository;
using ContractManagement.Infrastructure.Repositories.ServiceLevelAgreementRepository;
using ContractManagement.Infrastructure.Repositories.ServicePackagePriceRepository;
using ContractManagement.Infrastructure.Repositories.ServicePackageRepository;
using ContractManagement.Infrastructure.Repositories.ServiceRepository;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicesController : CustomBaseController
    {
        private readonly IServicesQueries _servicesQueries;
        private readonly IServicePackagesRepository _packageRepository;
        private readonly IServicesRepository _servicesRepository;
        private readonly IStaticResourceService _staticResourceService;
        private readonly IPictureRepository _pictureRepository;
        private readonly IServiceLevelAgreementRepository _serviceLevelAgreementRepository;
        private readonly IServiceLevelAgreementQueries _serviceLevelAgreementQueries;
        private readonly ITransactionServiceLevelAgreementQueries _transactionSlaQueries;
        private readonly IMapper _mapper;
        private readonly IServicePackagePriceRepository _packagePriceRepository;

        public ServicesController(IServicesQueries servicesQueries,
            IServicePackagesRepository packageRepository,
            IServicesRepository servicesRepository,
            IStaticResourceService staticResourceService,
            IPictureRepository pictureRepository,
            IServiceLevelAgreementRepository serviceLevelAgreementRepository,
            IServiceLevelAgreementQueries serviceLevelAgreementQueries,
            IServicePackagePriceRepository packagePriceRepository,
            IMapper mapper, 
            ITransactionServiceLevelAgreementQueries transactionSlaQueries)
        {
            _servicesQueries = servicesQueries;
            _servicesRepository = servicesRepository;
            _staticResourceService = staticResourceService;
            _pictureRepository = pictureRepository;
            _serviceLevelAgreementRepository = serviceLevelAgreementRepository;
            _serviceLevelAgreementQueries = serviceLevelAgreementQueries;
            _packageRepository = packageRepository;
            _packagePriceRepository = packagePriceRepository;
            _mapper = mapper;
            this._transactionSlaQueries = transactionSlaQueries;
        }

        [HttpGet]
        public IActionResult GetServices([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.SimpleAll)
            {
                return Ok(_servicesQueries.GetAll(filterModel));
            }

            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_servicesQueries.GetSelectionList(filterModel));
            }

            return Ok(_servicesQueries.GetList(filterModel));
        }

        [HttpGet("GetAllNotAvailableStartAndEndPoint")]
        public IActionResult GetAllNotAvailableStartAndEndPoint()
        {
            return Ok(_servicesQueries.GetAllNotAvailableStartAndEndPoint());
        }

        [HttpGet("{id}")]
        public IActionResult GetService(int id)
        {
            var service = _servicesQueries.Find(id);

            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, ServiceCommand serviceModel)
        {
            if (id != serviceModel.Id)
            {
                return BadRequest();
            }

            var validator = new ServicesValidator();
            var validateResult = await validator.ValidateAsync(serviceModel);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            var actionResponse = new ActionResponse<ServiceDTO>();

            if (_servicesQueries.CheckExistName(id, serviceModel.ServiceName))
            {
                actionResponse.AddError("Tên dịch vụ đã tồn tại", nameof(serviceModel.ServiceName));
                return BadRequest(actionResponse);
            }

            if (_servicesQueries.CheckExistCode(id, serviceModel.ServiceCode))
            {
                actionResponse.AddError("Mã dịch vụ đã tồn tại", nameof(serviceModel.ServiceCode));
                return BadRequest(actionResponse);
            }

            if (serviceModel.Avatar != null &&
                serviceModel.Avatar.Id == 0 &&
                !string.IsNullOrEmpty(serviceModel.Avatar.TemporaryUrl))
            {
                var storedAvatarItem =
                    await _staticResourceService.PersistentImage(serviceModel.Avatar.TemporaryUrl);
                var addUserAvatarResponse = await _pictureRepository.CreateAndSave(storedAvatarItem);
                if (!addUserAvatarResponse.IsSuccess)
                    throw new ContractDomainException(addUserAvatarResponse.Message);

                serviceModel.AvatarId = addUserAvatarResponse.Result.Id;
            }

            if((serviceModel.Avatar?.IsUpdating ?? false) && 
                !string.IsNullOrEmpty(serviceModel.Avatar.TemporaryUrl))
            {
                var storedAvatarItem =
                    await _staticResourceService.PersistentImage(serviceModel.Avatar.TemporaryUrl);
                storedAvatarItem.Id = serviceModel.AvatarId;
                var addUserAvatarResponse = await _pictureRepository.UpdateAndSave(storedAvatarItem);
                if (!addUserAvatarResponse.IsSuccess)
                    throw new ContractManagementException(addUserAvatarResponse.Message);
            }

            var updateSaveResponse = await _servicesRepository.UpdateAndSave(serviceModel);

            actionResponse.SetResult(this._mapper.Map<ServiceDTO>(updateSaveResponse.Result));
            actionResponse.CombineResponse(updateSaveResponse);

            if (serviceModel.ServiceLevelAgreements != null)
            {
                foreach (var item in serviceModel.ServiceLevelAgreements.Where(s => s.Id == 0))
                {
                    item.ServiceId = actionResponse.Result.Id;
                    item.IsDefault = true;
                    await _serviceLevelAgreementRepository.CreateAndSave(item);
                }

                foreach (var item in serviceModel.ServiceLevelAgreements.Where(s => s.Id > 0))
                {
                    item.ServiceId = actionResponse.Result.Id;
                    item.IsDefault = true;
                    await _serviceLevelAgreementRepository.UpdateAndSave(item);
                }
            }

            if (serviceModel.DeletedSLAs != null)
            {
                foreach (var slaId in serviceModel.DeletedSLAs)
                {
                    _serviceLevelAgreementRepository.DeleteAndSave(slaId);
                }
            }

            if (actionResponse.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(actionResponse);
        }

        [HttpPost]
        public async Task<IActionResult> PostService(ServiceCommand serviceModel)
        {
            var validator = new ServicesValidator();
            var validateResult = await validator.ValidateAsync(serviceModel);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            var actionResponse = new ActionResponse<ServiceDTO>();

            if (_servicesQueries.CheckExistName(serviceModel.Id, serviceModel.ServiceName))
            {
                actionResponse.AddError("Tên dịch vụ đã tồn tại", nameof(serviceModel.ServiceName));
                return BadRequest(actionResponse);
            }

            if (_servicesQueries.CheckExistCode(serviceModel.Id, serviceModel.ServiceCode))
            {
                actionResponse.AddError("Mã dịch vụ đã tồn tại", nameof(serviceModel.ServiceCode));
                return BadRequest(actionResponse);
            }

            if (serviceModel.Avatar != null 
                && !string.IsNullOrEmpty(serviceModel.Avatar.TemporaryUrl))
            {
                var storedAvatarItem =
                    await _staticResourceService.PersistentImage(serviceModel.Avatar.TemporaryUrl);
                var addUserAvatarResponse = await _pictureRepository.CreateAndSave(storedAvatarItem);
                if (!addUserAvatarResponse.IsSuccess)
                    throw new ContractDomainException(addUserAvatarResponse.Message);

                serviceModel.AvatarId = addUserAvatarResponse.Result.Id;
            }

            var createResponse = await _servicesRepository.CreateAndSave(serviceModel);
            actionResponse.SetResult(this._mapper.Map<ServiceDTO>(createResponse.Result));
            actionResponse.CombineResponse(createResponse);
            if (!actionResponse.IsSuccess) return BadRequest(actionResponse);

            if (serviceModel.ServiceLevelAgreements != null)
            {
                foreach (var item in serviceModel.ServiceLevelAgreements)
                {
                    item.ServiceId = actionResponse.Result.Id;
                    item.IsDefault = true;
                    await _serviceLevelAgreementRepository.CreateAndSave(item);
                }
            }

            if (actionResponse.IsSuccess)
            {
                return CreatedAtAction("GetService", new { id = actionResponse.Result.Id }, actionResponse);
            }
            else
            {
                _servicesRepository.DeleteAndSave(actionResponse.Result.Id);
            }

            return BadRequest(actionResponse);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteService(int id)
        {
            var serviceModel = _servicesQueries.Find(id);
            if (serviceModel == null)
            {
                return NotFound();
            }

            var deleteResponse = _servicesRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }


        [HttpGet("GetSLAByServiceId")]
        public IActionResult GetSLAByServiceId(int serviceId)
        {
            var SLAs = _serviceLevelAgreementQueries.GetSLAByServiceId(serviceId);

            if (SLAs == null)
            {
                return NotFound();
            }

            return Ok(SLAs);
        }

        [HttpGet("GetSLAByOutContractServicePackageId")]
        public IActionResult GetSLAByOutContractServicePackageId(int outContractServicePackageId)
        {
            var SLAs = _serviceLevelAgreementQueries.GetSLAByOutContractServicePackageId(outContractServicePackageId);

            if (SLAs == null)
            {
                return NotFound();
            }

            return Ok(SLAs);
        }

        [HttpGet("GetSLAByTransactionServicePackageId")]
        public IActionResult GetSLAByTransactionServicePackageId(int id)
        {
            return Ok(_transactionSlaQueries.GetSLAByTransactionServicePackageId(id));
        }

        //[HttpGet("GetALLSLAByOutContractServicePackageId")]
        //public IActionResult GetALLSLAByOutContractServicePackageId(int outContractServicePackageId, int serviceId)
        //{
        //    var SLAs = _serviceLevelAgreementQueries.GetALLSLAByOutContractServicePackageId(outContractServicePackageId, serviceId);

        //    if (SLAs == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(SLAs);
        //}
    }
}