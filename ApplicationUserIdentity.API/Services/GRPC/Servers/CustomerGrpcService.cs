using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using ApplicationUserIdentity.API.Models.CustomerViewModels;
using ApplicationUserIdentity.API.Protos;
using ApplicationUserIdentity.API.Services.BLL;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ApplicationUserIdentity.API.Services.GRPC.Servers
{
    public class CustomerGrpcService : CustomerGrpc.CustomerGrpcBase
    {
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        private readonly IUserQueries _userQueries;

        public CustomerGrpcService(IMapper mapper, 
            ICustomerService customerService,
            IUserQueries userQueries)
        {
            _mapper = mapper;
            _customerService = customerService;
            _userQueries = userQueries;
        }

        public override async Task<CreateCustomerResponseGrpc> CreateCustomer(CreateCustomerCommandGrpc request, ServerCallContext context)
        {
            var rq = _mapper.Map<UserViewModel>(request);
            if (!string.IsNullOrEmpty(request.GroupIdsStr))
            {
                var groupIds = request.GroupIdsStr.Split(",").Select(int.Parse).ToList();
                rq.GroupIds = groupIds;
            }

            var result = await _customerService.CreateCustomer(rq);
            return result;
        }

        public override async Task<CustomerClassPageListGrpc> GetCustomerClass(Empty request, ServerCallContext context)
        {
            var result = await _customerService.GetCustomerClass();
            return _mapper.Map<CustomerClassPageListGrpc>(result);
        }

        public override async Task<CustomerGroupPageListGrpc> GetCustomerGroup(Empty request, ServerCallContext context)
        {
            var result = await _customerService.GetCustomerGroup();
            return _mapper.Map<CustomerGroupPageListGrpc>(result);
        }
       
        public override async Task<CustomerTypePageListGrpc> GetCustomerType(Empty request, ServerCallContext context)
        {
            var result = await _customerService.GetCustomerType();
            return _mapper.Map<CustomerTypePageListGrpc>(result);
        }

        public override async Task<CustomerCategoryPageListGrpc> GetCustomerCategory(Empty request, ServerCallContext context)
        {
            var result = await _customerService.GetCustomerCategory();
            return _mapper.Map<CustomerCategoryPageListGrpc>(result);
        }

        public override async Task<CustomerStructPageListGrpc> GetCustomerStruct(Empty request, ServerCallContext context)
        {
            var result = await _customerService.GetCustomerStructure();
            return _mapper.Map<CustomerStructPageListGrpc>(result);
        }

        public override async Task<IndustryPageListGrpc> GetIndustries(Empty request, ServerCallContext context)
        {
            var result = await _customerService.GetIndustries();
            return _mapper.Map<IndustryPageListGrpc>(result);
        }

        public override Task<CustomerPageListGrpc> GetListCustomer(RequestFilterGrpcModel request, ServerCallContext context)
        {
            var requestFilterModel = request != null ? _mapper.Map<UserRequestFilterModel>(request) : new UserRequestFilterModel();
            var result = _userQueries.GetList(requestFilterModel);
            return Task.FromResult(_mapper.Map<CustomerPageListGrpc>(result));
        }
    }
}
