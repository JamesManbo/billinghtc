using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.IntegrationEvents;
using ApplicationUserIdentity.API.IntegrationEvents.EventHandling;
using ApplicationUserIdentity.API.Services.BLL;
using ApplicationUserIdentity.API.Services.GRPC.StaticResource;
using Autofac;
using EventBus.Abstractions;
using GenericRepository.Setups;
using ApplicationUserIdentity.API.Services.GRPC.Clients;
using ApplicationUserIdentity.API.Grpc.Clients.Location;
using Global.Models.Auth;
using Microsoft.AspNetCore.Http;
using GenericRepository;

namespace ApplicationUserIdentity.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string ConnectionString { get; set; }

        public ApplicationModule(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.GenericRepositorySetupByAutofac(typeof(ApplicationModule).Assembly);

            builder.RegisterRepository();

            builder.RegisterAssemblyTypes(typeof(NewContractorCreatedIntegrationEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }

    public static class CustomApplicationModuleExtension
    {
        public static void RegisterRepository(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .SingleInstance();

            containerBuilder.RegisterType<UserIdentity>()
                .AsSelf()
                .UsingConstructor(typeof(IHttpContextAccessor));


            // Register all the Repository classes (they implement CrudRepository) in assembly holding the Repositories
            containerBuilder.RegisterAssemblyTypes(typeof(UserRepository).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(ICrudRepository<,>))
                .PropertiesAutowired();

            // Register all the Queries classes (they implement QueryRepository) in assembly holding the QueryRepositories
            containerBuilder.RegisterAssemblyTypes(typeof(UserQueries).GetTypeInfo().Assembly)
                .AsImplementedInterfaces()
                .PropertiesAutowired();

            containerBuilder.RegisterType<CustomerService>()
               .As<ICustomerService>()
               .InstancePerLifetimeScope();
            ;
            
            containerBuilder.RegisterType<OtpQueries>()
               .As<IOtpQueries>()
               .InstancePerLifetimeScope();
            
            containerBuilder.RegisterType<ApplicationUserIntegrationEventService>()
               .As<IApplicationUserIntegrationEventService>()
               .InstancePerLifetimeScope();
            
            containerBuilder.RegisterType<ContractorGrpcService>()
               .As<IContractorGrpcService>()
               .InstancePerLifetimeScope();

            containerBuilder.RegisterType<LocationGrpcService>()
               .As<ILocationGrpcService>()
               .InstancePerLifetimeScope();
            
            containerBuilder.RegisterType<NotificationGrpcService>()
               .As<INotificationGrpcService>()
               .InstancePerLifetimeScope();
        }
    }
}