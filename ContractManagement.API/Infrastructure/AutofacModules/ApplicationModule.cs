using Autofac;
using ContractManagement.Infrastructure.Queries;
using GenericRepository.Setups;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using ContractManagement.Infrastructure.Repositories.ChangeHistoryRepository;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.API.Grpc.Clients.ApplicationUser;
using ContractManagement.API.Application.IntegrationEvents.EventHandling;
using EventBus.Abstractions;
using System.Reflection;
using ContractManagement.RadiusDomain.Repositories;
using ContractManagement.API.Application.Services.MikroTik;
using ContractManagement.API.Application.Services.Radius;
using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.API.Grpc.StaticResource;
using ContractManagement.API.Grpc.Clients.Location;
using Microsoft.AspNetCore.Http;
using Global.Models.Auth;
using GenericRepository;
using ContractManagement.Infrastructure.Services;
using CachingLayer.Interceptor;
using CachingLayer.Implementation;
using CachingLayer.Repository.Redis;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.Configuration;

namespace ContractManagement.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }
        public bool EnableCache { get; }

        public ApplicationModule(IConfiguration configuration)
        {
            QueriesConnectionString = configuration.GetConnectionString("ConnectionString");
            EnableCache = configuration.GetValue<bool?>("EnableCache") ?? false;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<StaticResourceService>().As<IStaticResourceService>().InstancePerLifetimeScope();
            builder.GenericRepositorySetupByAutofac(typeof(ApplicationModule).Assembly);

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

            builder.RegisterType<StaticResourceService>().As<IStaticResourceService>().InstancePerLifetimeScope();

            // Register all the Repository classes (they implement CrudRepository) in assembly holding the Repositories
            builder.RegisterAssemblyTypes(typeof(OutContractRepository).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ICrudRepository<,>))
                .PropertiesAutowired();

            // Register all the Queries classes (they implement QueryRepository) in assembly holding the QueryRepositories
            builder.RegisterAssemblyTypes(typeof(OutContractQueries).GetTypeInfo().Assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(CacheInterceptor))
                .PropertiesAutowired();

            //GRPC
            builder.RegisterType<AttachmentFileResourceGrpcService>().As<IAttachmentFileResourceGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<UserGrpcService>().As<IUserGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUserGrpcService>().As<IApplicationUserGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<NotificationGrpcService>().As<INotificationGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<LocationGrpcService>().As<ILocationGrpcService>().InstancePerLifetimeScope();
            builder.RegisterType<OrganizationUnitGrpcService>().As<IOrganizationUnitGrpcService>().InstancePerLifetimeScope();

            //MongoDb
            builder.RegisterType<ContractHistoryRepository>().As<IContractHistoryRepository>().InstancePerLifetimeScope();

            //Radius management repos
            builder.RegisterType<RadiusManagementRepository>().As<IRadiusManagementRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MikroTikService>().As<IMikroTikService>().InstancePerLifetimeScope();
            builder.RegisterType<RadiusAndBrasManagementService>().As<IRadiusAndBrasManagementService>().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(UpdateContractorIntegrationEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

            builder.RegisterType<ExchangeRateService>().As<IExchangeRateService>().InstancePerLifetimeScope();

        }
    }
}