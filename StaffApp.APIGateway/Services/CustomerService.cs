
using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using StaffApp.APIGateway.Models.CustomerModels;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Protos;
using Google.Protobuf.WellKnownTypes;
using Global.Models.PagedList;
using System.Collections.Generic;
using StaffApp.APIGateway.Models.RequestModels;
using Global.Models.Filter;

namespace StaffApp.APIGateway.Services
{
    public interface ICustomerService
    {
        Task<CreateCustomerDTO> CreateCustomer(CCustomerCommandGrpc applicationUser);
        Task<IPagedList<CustomerDTO>> GetListCustomer(RequestFilterModel filterModel);
        Task<IEnumerable<CustomerClassDtoGrpc>> GetCustomerClass();
        Task<IEnumerable<CustomerCategoryDtoGrpc>> GetCustomerCategory();
        Task<IEnumerable<CustomerGroupDtoGrpc>> GetCustomerGroup();
        Task<IEnumerable<CustomerTypeDtoGrpc>> GetCustomerType();
        Task<IEnumerable<CustomerStructDtoGrpc>> GetCustomerStruct();
        Task<IEnumerable<IndustryDto>> GetIndustries();
    }
    public class CustomerService : GrpcCaller, ICustomerService
    {
        private readonly IMapper _mapper;
        public CustomerService(IMapper mapper, ILogger<GrpcCaller> logger) : base(mapper, logger, MicroserviceRouterConfig.GrpcApplicationUser)
        {
            _mapper = mapper;
        }
        public async Task<CreateCustomerDTO> CreateCustomer(CCustomerCommandGrpc applicationUser)
        {
            return await Call<CreateCustomerDTO>(async channel =>
            {
                var client = new CustomerGrpc.CustomerGrpcClient(channel);
                var request = _mapper.Map<CreateCustomerCommandGrpc>(applicationUser);
                var customerModelGrpc = await client.CreateCustomerAsync(request);

                return _mapper.Map<CreateCustomerDTO>(customerModelGrpc);
            });
        }

        public async Task<IEnumerable<CustomerClassDtoGrpc>> GetCustomerClass()
        {
            return await Call<IEnumerable<CustomerClassDtoGrpc>>(async channel =>
            {
                var client = new CustomerGrpc.CustomerGrpcClient(channel);
                var customerModelGrpc = await client.GetCustomerClassAsync(new Empty());

                return _mapper.Map<IPagedList<CustomerClassDtoGrpc>>(customerModelGrpc).Subset;
            });
        }

        public async Task<IEnumerable<CustomerGroupDtoGrpc>> GetCustomerGroup()
        {
            return await Call<IEnumerable<CustomerGroupDtoGrpc>>(async channel =>
            {
                var client = new CustomerGrpc.CustomerGrpcClient(channel);
                var customerModelGrpc = await client.GetCustomerGroupAsync(new Empty());

                return _mapper.Map< IPagedList<CustomerGroupDtoGrpc>>(customerModelGrpc).Subset;
            });
        }

        public async Task<IEnumerable<CustomerTypeDtoGrpc>> GetCustomerType()
        {
            return await Call<IEnumerable<CustomerTypeDtoGrpc>>(async channel =>
            {
                var client = new CustomerGrpc.CustomerGrpcClient(channel);
                var customerModelGrpc = await client.GetCustomerTypeAsync(new Empty());

                return _mapper.Map<IPagedList<CustomerTypeDtoGrpc>>(customerModelGrpc).Subset;
            });
        }

        public async Task<IEnumerable<CustomerCategoryDtoGrpc>> GetCustomerCategory()
        {
            return await Call<IEnumerable<CustomerCategoryDtoGrpc>>(async channel =>
            {
                var client = new CustomerGrpc.CustomerGrpcClient(channel);
                var customerModelGrpc = await client.GetCustomerCategoryAsync(new Empty());

                return _mapper.Map<IPagedList<CustomerCategoryDtoGrpc>>(customerModelGrpc).Subset;
            });
        }

        public async Task<IEnumerable<CustomerStructDtoGrpc>> GetCustomerStruct()
        {
            return await Call<IEnumerable<CustomerStructDtoGrpc>>(async channel =>
            {
                var client = new CustomerGrpc.CustomerGrpcClient(channel);
                var customerModelGrpc = await client.GetCustomerStructAsync(new Empty());

                return _mapper.Map<IPagedList<CustomerStructDtoGrpc>>(customerModelGrpc).Subset;
            });
        }

        public async Task<IEnumerable<IndustryDto>> GetIndustries()
        {
            return await Call<IEnumerable<IndustryDto>>(async channel =>
            {
                var client = new CustomerGrpc.CustomerGrpcClient(channel);
                var customerModelGrpc = await client.GetIndustriesAsync(new Empty());

                return _mapper.Map<IPagedList<IndustryDto>>(customerModelGrpc).Subset;
            });
        }

        public async Task<IPagedList<CustomerDTO>> GetListCustomer(RequestFilterModel filterModel)
        {
            return await Call<IPagedList<CustomerDTO>>(async channel =>
            {
                var client = new CustomerGrpc.CustomerGrpcClient(channel);
                var request = _mapper.Map<RequestFilterGrpcModel>(filterModel);

                var lstContractorGrpc = await client.GetListCustomerAsync(request);

                return _mapper.Map<IPagedList<CustomerDTO>>(lstContractorGrpc);
            });
        }


    }
}
