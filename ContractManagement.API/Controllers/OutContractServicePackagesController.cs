using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Infrastructure.Queries;
using Global.Models.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OutContractServicePackagesController : CustomBaseController
    {
        private readonly IOutContractServicePackageQueries _outContractServicePackageQueries;
        private readonly IServicePackageSuspensionTimeQueries _servicePackageSuspensionTimeQueries;

        public OutContractServicePackagesController(
            IOutContractServicePackageQueries outContractServicePackageQueries,
            IServicePackageSuspensionTimeQueries servicePackageSuspensionTimeQueries)
        {
            _outContractServicePackageQueries = outContractServicePackageQueries;
            _servicePackageSuspensionTimeQueries = servicePackageSuspensionTimeQueries;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ChannelRequestFilterModel requestFilterModel)
        {
            if(requestFilterModel.Type == RequestType.Selection)
            {
                return Ok(_outContractServicePackageQueries.GetSelectionList());
            }
            return Ok(await _outContractServicePackageQueries.GetPagedList(requestFilterModel));
        }

        [HttpGet("GetAllRunningByOutContractIds/{ids}")]
        public IActionResult GetAllRunningByOutContractIds(string ids)
        {
            return Ok(_outContractServicePackageQueries.GetAllByOutContractIds(ids, OutContractServicePackageStatus.Developed.Id));
        }

        [HttpGet("GetAllSuspendByOutContractIds/{ids}")]
        public IActionResult GetAllSuspendByOutContractIds(string ids)
        {
            return Ok(_outContractServicePackageQueries.GetAllByOutContractIds(ids, OutContractServicePackageStatus.Suspend.Id));
        }

        [HttpGet("GetAllNotYetTerminateByOutContractIds/{ids}")]
        public IActionResult GetAllNotYetTerminateByOutContractIds(string ids)
        {
            return Ok(_outContractServicePackageQueries.GetAllNotYetTerminateByOutContractIds(ids));
        }

        [HttpGet("GetAllNotAvailableStartAndEndPointByOutContractIds/{ids}")]
        public IActionResult GetAllNotAvailableStartAndEndPointByOutContractIds(string ids)
        {
            return Ok(_outContractServicePackageQueries.GetAllNotAvailableStartAndEndPointByOutContractIds(ids));
        }

        [HttpGet("GetChannelIndexOfCustomer/{contractorGuid}")]
        public IActionResult GetChannelIndexOfCustomer(string contractorGuid)
        {
            return Ok(_outContractServicePackageQueries.GetChannelIndexOfCustomer(contractorGuid));
        }

        [HttpGet("GenerateCid")]
        public IActionResult GenerateCid(int contractorId, string serviceCode, string contractorShortName, DateTime effective, int numberOfNewChannels = 0)
        {
            var cIdPaths = new List<string>();
            if (!string.IsNullOrEmpty(serviceCode))
            {
                cIdPaths.Add(serviceCode);
            }
            if (!string.IsNullOrEmpty(contractorShortName))
            {
                cIdPaths.Add(contractorShortName.Substring(0, 3));
            }
            cIdPaths.Add(effective.ToString("MMyy"));

            var channelIndex = 0;
            try
            {
                var currentNumberChannels = _outContractServicePackageQueries.GetChannelIndexOfCustomer(contractorShortName.Substring(0, 3));
                channelIndex += currentNumberChannels;
            }
            catch (Exception)
            {

            }
            channelIndex += numberOfNewChannels;

            cIdPaths.Add(channelIndex.ToString().PadLeft(3, '0'));

            return Ok(string.Join("_", cIdPaths));
        }

        [HttpGet("GetSuspensions")]
        public IActionResult GetSuspensions(string cId)
        {
            if (string.IsNullOrEmpty(cId))
            {
                return BadRequest();
            }

            return Ok(_servicePackageSuspensionTimeQueries.GetChannelSuspensionTimes(cId));
        }

    }
}