using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.FilterModels.ReportsModel;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Utility;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ContractManagement.API.Grpc.Servers
{
    public class OutContractService : ContractManagementGrpc.ContractManagementGrpcBase
    {
        private readonly IMediator _mediator;
        private readonly IOutContractQueries _outContractQueries;
        private readonly IOutContractServicePackageQueries _outContractServicePackageQueries;
        private readonly IEquipmentTypeQueries _equipmentTypeQueries;
        private readonly IMapper _mapper;
        private readonly IOutputChannelPointQueries _outputChannelPointQueries;
        private readonly IPromotionQueries _promotionQueries;

        public OutContractService(IMediator mediator,
            IOutContractQueries outContractQueries,
            IOutputChannelPointQueries outputChannelPointQueries,
            IOutContractServicePackageQueries outContractServicePackageQueries,
            IEquipmentTypeQueries equipmentTypeQueries,
            IMapper mapper,
            IPromotionQueries promotionQueries)
        {
            _mediator = mediator;
            _outContractQueries = outContractQueries;
            _mapper = mapper;
            _equipmentTypeQueries = equipmentTypeQueries;
            _outputChannelPointQueries = outputChannelPointQueries;
            _outContractServicePackageQueries = outContractServicePackageQueries;
            _promotionQueries = promotionQueries;
        }

        public override Task<ContractGrpcDTO> FindByContractorId(StringValue request, ServerCallContext context)
        {
            try
            {
                var outContracts = _outContractQueries.FindByContractorId(request.Value);
                return Task.FromResult(_mapper.Map<ContractGrpcDTO>(outContracts));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override Task<ContractGrpcDTO> FindByContractCode(StringValue request, ServerCallContext context)
        {
            try
            {
                var outContracts = _outContractQueries.FindByContractCode(request.Value);

                return Task.FromResult(_mapper.Map<ContractGrpcDTO>(outContracts));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override async Task<ContractPageListGrpcDTO> GetContracts(RequestGetContractsGrpc request, ServerCallContext context)
        {
            var outContracts = await _outContractQueries.GetPagedList(_mapper.Map<ContactsFilterModel>(request));

            return _mapper.Map<ContractPageListGrpcDTO>(outContracts);
        }

        public override Task<ContractGrpcDTO> GetDetail(Int32Value request, ServerCallContext context)
        {
            var outContract = _outContractQueries.FindById(request.Value);

            return Task.FromResult(_mapper.Map<ContractGrpcDTO>(outContract));
        }

        public override Task<GetOutContractByIdsResponse> GetByIds(StringValue request, ServerCallContext context)
        {
            var result = new GetOutContractByIdsResponse();
            var outContracts = _outContractQueries.GetOutContractSimpleAllByIds(request.Value);
            result.OutContracts.AddRange(_mapper.Map<IEnumerable<OutContractSimpleGrpcDTO>>(outContracts));
            return Task.FromResult(result);
        }

        public override async Task<OutContractIsSuccessGRPC> CreatedOutContract(StringValue request, ServerCallContext context)
        {
            var actResponse = new ActionResponse();
            CreateContractCommand command = JObject.Parse(request.Value).ToObject<CreateContractCommand>();
            actResponse = await _mediator.Send(command);
            var message = "";
            if (actResponse.Errors != null && actResponse.Errors.Count > 0) message = actResponse.Errors.ElementAt(0).ErrorMessage;
            return new OutContractIsSuccessGRPC() { IsSuccess = actResponse.IsSuccess, Message = message };
        }

        public override Task<ContractGrpcDTO> GetContractByChannelId(Int32Value channelId, ServerCallContext context)
        {
            var outContract = _outContractQueries.FindByChannelId(channelId.Value);
            return Task.FromResult(_mapper.Map<ContractGrpcDTO>(outContract));
        }
        public override Task<ContractGrpcDTO> GetContractByChannelCId(StringValue channelId, ServerCallContext context)
        {
            var outContract = _outContractQueries.FindByChannelCId(channelId.Value);
            return Task.FromResult(_mapper.Map<ContractGrpcDTO>(outContract));
        }

        public override Task<StringValue> GetOutContractStatuses(Empty request, ServerCallContext context)
        {
            var result = ContractStatus.List();
            return Task.FromResult(new StringValue() { Value = JsonConvert.SerializeObject(result) });
        }

        public override Task<OutContractServicePackageListGrpcResponse> GetOutContractServicePackageByIds(StringValue request, ServerCallContext context)
        {
            var result = new OutContractServicePackageListGrpcResponse();
            var outContractServicePackages = _outContractServicePackageQueries.GetAllAvailableByIds(request.Value);
            result.OutContractServicePackages.AddRange(_mapper.Map<IEnumerable<OutContractServicePackageGrpcDTO>>(outContractServicePackages));
            return Task.FromResult(result);
        }

        public override Task<StringValue> GenerateContractCode(GenerateContractCodeRequestGrpc request, ServerCallContext context)
        {
            var result = _outContractQueries.GenerateContractCode(request.IsEnterprise,
                request.CustomerFullName,
                request.SrvPackageIds.SplitToInt(','),
                request.MarketAreaId,
                request.ProjectId);

            return Task.FromResult(new StringValue() { Value = result });
        }
        public override Task<OutContractEquipmentListGrpc> GetOutContractEquipments(RequestFilterGrpc request, ServerCallContext context)
        {
            var lstEquipments = _equipmentTypeQueries.AutocompleteInstance(_mapper.Map<RequestFilterModel>(request));
            var rs = new OutContractEquipmentListGrpc();
            if (lstEquipments != null)
            {
                foreach (var model in lstEquipments)
                {
                    rs.Data.Add(_mapper.Map<OutContractEquipmentGrpcDTO>(model));
                }
            }

            return Task.FromResult(rs);
        }

        public override Task<OutputChannelPointListGrpc> AutoCompleteOutputChannelPoint(OutputChannelPointRequestGrpc request, ServerCallContext context)
        {
            var channelPoints = _outputChannelPointQueries.Autocomplete(_mapper.Map<OutputChannelFilterModel>(request));
            var rs = new OutputChannelPointListGrpc();
            foreach (var model in channelPoints)
            {
                rs.Data.Add(_mapper.Map<OutputChannelPointGrpcDTO>(model));
            }

            return Task.FromResult(rs);
        }

        public override Task<ContractStatusReportResponseGrpc> GetReportContractStatus(ContractStatusReportFilterGrpc request, ServerCallContext context)
        {
            var reports = _outContractQueries.GetReportContractStatus(_mapper.Map<ContractStatusReportFilter>(request));
            var rs = new ContractStatusReportResponseGrpc();
            foreach (var model in reports)
            {
                rs.Data.Add(_mapper.Map<ContractStatusReportModelGrpc>(model));
            }
            return Task.FromResult(rs);
        }

        public override Task<ChannelAddressesResponseGrpc> GetChannelAddressesByCid(StringValue request, ServerCallContext context)
        {
            var channelAddresses = _outContractServicePackageQueries.GetChannelAddresses(request.Value?.Split(','));
            var result = new ChannelAddressesResponseGrpc();
            result.Result.AddRange(channelAddresses.Select(_mapper.Map<ChannelAddressModelGrpc>));
            return Task.FromResult(result);
        }

        public override Task<AvailabelPromotionResponse> GetAvailablePromotions(Empty request, ServerCallContext context)
        {
            var availablePromotions = _promotionQueries.GetAvailablePromotions();
            var result = new AvailabelPromotionResponse();
            if (availablePromotions.Any())
            {
                result.PromotionGrpcModels.AddRange(availablePromotions.Select(_mapper.Map<PromotionGrpcModel>));
            }
            return Task.FromResult(result);
        }
    }
}

