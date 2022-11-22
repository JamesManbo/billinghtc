using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories.OtpRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemUserIdentity.API.Proto;

namespace SystemUserIdentity.API.Grpc
{
    public class OtpService : OtpGrpc.OtpGrpcBase
    {
        private readonly IOtpQueries _otpQueries;
        private readonly IOtpRepository _otpRepository;
        private readonly IMapper _mapper;

        public OtpService(
            IOtpRepository otpRepository,
            IOtpQueries otpQueries,
            IMapper mapper)
        {
            _otpQueries = otpQueries;
            _otpRepository = otpRepository;
            _mapper = mapper;
        }
        public override async Task<OtpDtoGrpc> FindByUserName(StringValue request, ServerCallContext context)
        {
            var rs = _otpQueries.FindByUserName(request.Value);
            return await Task.FromResult(_mapper.Map<OtpDtoGrpc>(rs));
        }

        public override async Task<UpdateOtpUsedResponseGrpc> UpdateOtpUsed(Int32Value request, ServerCallContext context)
        {
            var model = await _otpRepository.GetByIdAsync(request.Value);
            if (model!=null)
            {
                model.IsUse = true;
                await _otpRepository.UpdateAndSave(model);
                return new UpdateOtpUsedResponseGrpc() { IsSuccess = true};
            }
            return new UpdateOtpUsedResponseGrpc() { IsSuccess = false };
        }
    }
}
