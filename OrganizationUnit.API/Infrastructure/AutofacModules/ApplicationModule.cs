using System.Reflection;
using Autofac;
using EventBus.Abstractions;
using GenericRepository.Setups;
using OrganizationUnit.API.Application.Commands.User;
using OrganizationUnit.API.Application.IntegrationEvents.EventHandling;
using OrganizationUnit.API.Grpc.StaticResource;
using OrganizationUnit.API.Infrastructure.Services;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories;
using OrganizationUnit.Infrastructure.Repositories.ConfigurationSettingUserRepository;
using OrganizationUnit.Infrastructure.Repositories.ConfigurationSystemUserRepository;
using OrganizationUnit.Infrastructure.Repositories.FCMRepository;
using OrganizationUnit.Infrastructure.Repositories.OrganizationUnitRepository;
using OrganizationUnit.Infrastructure.Repositories.OtpRepository;
using OrganizationUnit.Infrastructure.Repositories.PictureRepository;
using CachingLayer.Interceptor;
using CachingLayer.Implementation;
using CachingLayer.Repository.Redis;
using Autofac.Extras.DynamicProxy;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Global.Models.Auth;

namespace OrganizationUnit.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string queriesConnectionString)
        {
            QueriesConnectionString = queriesConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.GenericRepositorySetupByAutofac(typeof(ApplicationModule).Assembly);

            builder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .SingleInstance();

            builder.RegisterType<UserIdentity>()
                .AsSelf()
                .UsingConstructor(typeof(IHttpContextAccessor));

            //Cache
            builder.RegisterType<RedisCacheRepository>().As<IRedisCacheRepository>();
            builder.RegisterType<RedisCacheHandler>().As<ICacheHandler>();
            builder.RegisterType<CacheInterceptor>();
            // Grpc services
            builder.RegisterType<StaticResourceService>().As<IStaticResourceService>().InstancePerLifetimeScope();

            // Sql repositories
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();

            // Register all the Repository classes (they implement CrudRepository) in assembly holding the Repositories
            builder.RegisterAssemblyTypes(typeof(UserRepository).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ICrudRepository<,>))
                .PropertiesAutowired();

            // Register all the Queries classes (they implement QueryRepository) in assembly holding the QueryRepositories
            builder.RegisterAssemblyTypes(typeof(UserQueries).GetTypeInfo().Assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(CacheInterceptor))
                .PropertiesAutowired();

            builder.RegisterAssemblyTypes(typeof(UpdateContractorIntegrationEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

            //builder.RegisterAssemblyTypes(typeof(CreateUserCommandHandler).GetTypeInfo().Assembly)
            //    .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}