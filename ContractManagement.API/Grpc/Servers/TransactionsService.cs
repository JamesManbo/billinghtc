using AutoMapper;
using ContractManagement.API.Protos;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Commands.TransactionCommand.AcceptancedTransactions;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Infrastructure.Queries;
using Global.Models.StateChangedResponse;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using static ContractManagement.Domain.AggregatesModel.TransactionAggregate.TransactionType;

namespace ContractManagement.API.Grpc.Servers
{
    public class TransactionsService : TransactionsGrpc.TransactionsGrpcBase
    {

        private readonly IMediator _mediator;
        private readonly ITransactionQueries _transactionQueries;
        private readonly IMapper _mapper;
        public TransactionsService(IMediator mediator, ITransactionQueries transactionQueries, IMapper mapper)
        {
            _mediator = mediator;
            _transactionQueries = transactionQueries;
            _mapper = mapper;
        }

        public override async Task<IsSuccessGRPC> AddNewTransaction(RequestTransactionGRPC request, ServerCallContext context)
        {
            var actResponse = new ActionResponse();

            switch (request.TypeId)
            {
                case (int)TransactionTypeEnums.AddNewServicePackage:
                    {
                        CUAddNewServicePackageTransaction command = JObject.Parse(request.TransactionJSON).ToObject<CUAddNewServicePackageTransaction>();
                        try
                        {
                            actResponse = await _mediator.Send(command);
                        }
                        catch (ContractDomainException e)
                        {
                            actResponse.AddError(e.Message);
                        }
                        break;
                    }
                case (int)TransactionTypeEnums.ChangeServicePackage:
                    {
                        CUChangeServicePackageTransaction command = JObject.Parse(request.TransactionJSON).ToObject<CUChangeServicePackageTransaction>();
                        actResponse = await _mediator.Send(command);
                        break;
                    }
                case (int)TransactionTypeEnums.ChangeEquipment:
                    {
                        CUChangeEquipmentsCommand command = JObject.Parse(request.TransactionJSON).ToObject<CUChangeEquipmentsCommand>();
                        actResponse = await _mediator.Send(command);
                        break;
                    }
                case (int)TransactionTypeEnums.ReclaimEquipment:
                    {
                        CUReclaimEquipmentsCommand command = JObject.Parse(request.TransactionJSON).ToObject<CUReclaimEquipmentsCommand>();
                        actResponse = await _mediator.Send(command);
                        break;
                    }
                default:
                    break;
            }


            var rs = new IsSuccessGRPC() { IsSuccess = actResponse.IsSuccess };
            foreach (var e in actResponse.Errors)
            {
                rs.Errors.Add(new ErrorModelGRPC { ErrorMessage = e.ErrorMessage, MemberName = e.MemberName });
            }
            return rs;
        }

        public override async Task<IsSuccessGRPC> AcceptanceTransactionApp(AcceptanceTransactionCommandAppGrpc request, ServerCallContext context)
        {
            var command = _mapper.Map<AcceptanceTransactionCommandApp>(request);
            var actResponse = await _mediator.Send(command);
            return new IsSuccessGRPC() { IsSuccess = actResponse.IsSuccess };

        }

        public override async Task<IsSuccessGRPC> GetDetail(Int32Value request, ServerCallContext context)
        {
            var detail = _transactionQueries.FindFromSupporterService(request.Value);
            if (detail != null)
                return new IsSuccessGRPC() { IsSuccess = true, DataJson = JsonConvert.SerializeObject(detail) };
            return new IsSuccessGRPC() { IsSuccess = false };
        }
    }
}
