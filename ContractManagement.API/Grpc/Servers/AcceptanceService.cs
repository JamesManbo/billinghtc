using AutoMapper;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.API.Protos;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Infrastructure.Queries;
using Global.Configs.SystemArgument;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Grpc.Servers
{
    public class AcceptanceService : AcceptanceGrpc.AcceptanceGrpcBase
    {
        private readonly ITransactionQueries _transactionQueries;
        private readonly IAcceptanceQueries _acceptanceQueries;
        private readonly IOrganizationUnitGrpcService _organizationGrpcService;
        private readonly DepartmentCode _departmentCode;
        private readonly IMapper _mapper;
        public AcceptanceService(ITransactionQueries transactionQueries,
            IAcceptanceQueries acceptanceQueries,
            IMapper mapper,
            IConfiguration config,
            IOrganizationUnitGrpcService organizationGrpcService)
        {
            _transactionQueries = transactionQueries;
            _acceptanceQueries = acceptanceQueries;
            _mapper = mapper;
            _departmentCode = config.GetSection("DepartmentCode").Get<DepartmentCode>();
            this._organizationGrpcService = organizationGrpcService;
        }

        public override async Task<AcceptancesPageListGrpcDTO> GetAcceptances(RequestGetAcceptancesGrpc request, ServerCallContext context)
        {
            var alowedDepartments = await _organizationGrpcService.GetChildrenByCode(_departmentCode.SupporterDepartmentCode);
            var result = _transactionQueries.GetList(_mapper.Map<TransactionRequestFilterModel>(request), alowedDepartments, true);
            return _mapper.Map<AcceptancesPageListGrpcDTO>(result);
        }

        public override Task<AcceptanceDTOGrpc> GetDetail(Int32Value request, ServerCallContext context)
        {
            var result = _transactionQueries.FindFromSupporterService(request.Value);
            return Task.FromResult(_mapper.Map<AcceptanceDTOGrpc>(result));
        }
    }
}
