using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Services.GRPC.Servers
{
    public class OtpGrpcService : OtpGrpc.OtpGrpcBase
    {
        private readonly IOtpQueries _otpQueries;
        private readonly IOtpRepository _otpRepository;
        private readonly IMapper _mapper;

        public OtpGrpcService(
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
            try {
                var rs = _otpQueries.FindByUserName(request.Value);
                return await Task.FromResult(_mapper.Map<OtpDtoGrpc>(rs));
            }
            catch(Exception e)
            {
                throw;
            }

        }

        public override async Task<UpdateOtpUsedResponseGrpc> UpdateOtpUsed(Int32Value request, ServerCallContext context)
        {
            var model = await _otpRepository.GetByIdAsync(request.Value);
            if (model != null)
            {
                model.IsUse = true;
                await _otpRepository.UpdateAndSave(model);
                return new UpdateOtpUsedResponseGrpc() { IsSuccess = true };
            }
            return new UpdateOtpUsedResponseGrpc() { IsSuccess = false };
        }
    }
}
