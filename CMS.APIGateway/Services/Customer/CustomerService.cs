using ApplicationUserIdentity.API.Protos;
using AutoMapper;
using Global.Configs.MicroserviceRouterConfig;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.APIGateway.Services.Customer
{
    public interface ICustomerService
    {
        Task<CustomerPageListGrpc> GetList(RequestFilterGrpcModel filterModel);
    }

    public class CustomerService : GrpcCaller, ICustomerService
    {
        public CustomerService(IMapper mapper, ILogger<GrpcCaller> logger)
            : base(mapper, logger, MicroserviceRouterConfig.GrpcApplicationUser)
        {
        }

        public async Task<CustomerPageListGrpc> GetList(RequestFilterGrpcModel filterModel)
        {
            return await Call<CustomerPageListGrpc>(async channel =>
            {
                var grpcClient = new CustomerGrpc.CustomerGrpcClient(channel);
                return await grpcClient.GetListCustomerAsync(filterModel);
            });
        }
    }
}
