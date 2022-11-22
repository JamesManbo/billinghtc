using System;
using System.Threading.Tasks;
using AutoMapper;
using DebtManagement.API.Protos;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Models.FilterModels;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.Infrastructure.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Newtonsoft.Json;

namespace DebtManagement.API.Grpc.Servers
{
    public class ReceiptVoucherGrpcService : ReceiptVoucherGrpc.ReceiptVoucherGrpcBase
    {
        private readonly IReceiptVoucherQueries _receiptVoucherQueries;
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ReceiptVoucherGrpcService(
            IReceiptVoucherQueries receiptVoucherQueries, IMapper mapper,
            IReceiptVoucherRepository receiptVoucherRepository, IMediator mediator)
        {
            _receiptVoucherQueries = receiptVoucherQueries;
            _mapper = mapper;
            _receiptVoucherRepository = receiptVoucherRepository;
            _mediator = mediator;
        }

        public override Task<ReceiptVoucherPageListGrpcDTO> GetReceiptVouchers(ReceiptVoucherFilterGrpc request, ServerCallContext context)
        {
            var lstService = _receiptVoucherQueries.GetList(_mapper.Map<ReceiptVoucherBothProjectNullFilterModel>(request));
            return Task.FromResult(_mapper.Map<ReceiptVoucherPageListGrpcDTO>(lstService));
        }

        public override Task<ReceiptVoucherDTOGrpc> GetReceiptVoucherDetail(StringValue request, ServerCallContext context)
        {
            int.TryParse(request.Value, out int id);
            var receiptVoucher = _receiptVoucherQueries.Find(id);
            return Task.FromResult(_mapper.Map<ReceiptVoucherDTOGrpc>(receiptVoucher));
        }

        public override async Task<ActionResponseGrpc> Update(StringValue request, ServerCallContext context)
        {
            var updateReceiptVoucherCmd = JsonConvert.DeserializeObject<UpdateReceiptVoucherCommand>(request.Value);
            var actResponse = await _mediator.Send(updateReceiptVoucherCmd);
            return _mapper.Map<ActionResponseGrpc>(actResponse);
        }

        public override Task<StringValue> GetReceiptStatus(Empty request, ServerCallContext context)
        {
            var result = ReceiptVoucherStatus.List();
            return Task.FromResult(new StringValue() { Value = JsonConvert.SerializeObject(result) });
        }
    }
}
