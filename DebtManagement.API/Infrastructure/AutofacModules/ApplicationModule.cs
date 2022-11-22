using Autofac;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.Infrastructure.Repositories;
using GenericRepository.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtManagement.API.Grpc;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.API.Application.IntegrationEvents.EventHandling;
using DebtManagement.API.Application.Commands.ReceiptVoucherCommandHandler;
using EventBus.Abstractions;
using System.Reflection;
using DebtManagement.API.Services;
using DebtManagement.API.Grpc.Clients.StaticResource;
using DebtManagement.API.Grpc.Servers;
using Microsoft.AspNetCore.Http;
using Global.Models.Auth;
using GenericRepository;
using Microsoft.Extensions.Configuration;
using CachingLayer.Repository.Redis;
using CachingLayer.Implementation;
using CachingLayer.Interceptor;

namespace DebtManagement.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public bool EnableCache { get; }

        public ApplicationModule(IConfiguration configuration)
        {
            EnableCache = configuration.GetValue<bool?>("EnableCache") ?? false;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => c.Resolve<HttpContext>())
                .As<HttpContext>()
                .SingleInstance();

            builder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .SingleInstance();

            builder.RegisterType<UserIdentity>()
                .AsSelf()
                .UsingConstructor(typeof(IHttpContextAccessor));

            //Cache
            builder.RegisterType<RedisCacheRepository>().As<IRedisCacheRepository>();
            builder.RegisterType<RedisCacheHandler>().As<ICacheHandler>()
                .WithParameter("isEnable", EnableCache);
            builder.RegisterType<CacheInterceptor>();

            builder.GenericRepositorySetupByAutofac(typeof(ApplicationModule).Assembly);

            //Repo
            builder.RegisterAssemblyTypes(typeof(ReceiptVoucherRepository).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ICrudRepository<,>))
                .PropertiesAutowired();
            
            //Queries
            builder.RegisterAssemblyTypes(typeof(ReceiptVoucherQueries).GetTypeInfo().Assembly)
                .AsImplementedInterfaces()
                .PropertiesAutowired();

            //Grpc
            builder.RegisterType<CollectedVoucherGrpcService>().As<ICollectedVoucherGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractorGrpcService>().As<IContractorGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractGrpcService>().As<IContractGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<UserGrpcService>().As<IUserGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<TaxCategoryGrpcService>().As<ITaxCategoryGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<ProjectGrpcService>().As<IProjectGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<MarketAreaGrpcService>().As<IMarketAreaGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<AttachmentFileResourceGrpcService>().As<IAttachmentFileResourceGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<FeedbackGrpcService>().As<IFeedbackGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<ConfigurationSystemParameterGrpcService>().As<IConfigurationSystemParameterGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<ExchangeRateGrpcService>().As<IExchangeRateGrpcService>().InstancePerLifetimeScope();

            //Service
            builder.RegisterType<ReceiptVoucherService>().As<IReceiptVoucherService>().InstancePerLifetimeScope();
            builder.RegisterType<PaymentVoucherService>().As<IPaymentVoucherService>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(CreateReceiptVoucherCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

        }
    }
}
