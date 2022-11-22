using System;
using System.Reflection;
using Autofac;
using GenericRepository.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenericRepository.Setups
{
    /// <summary>
    /// This contains extension methods for setting up GenericServices at startup.
    /// It assumes the use of dependency injection (DI) and <see cref="IServiceCollection"/> for DI registering
    /// </summary>
    public static class ConfigureGenericRepository
    {
        /// <summary>
        /// This will configure GenericServices if you are using one DbContext and you are happy to use the default GenericServices configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembliesToScan">You can define the assemblies to scan for DTOs/ViewModels. Otherwise it will scan all assemblies (slower, but simple)</param>
        /// <returns></returns>
        public static IServiceCollection GenericRepositorySetup(this IServiceCollection services,
            params Assembly[] assembliesToScan)
        {
            return services.ScanAssemblesForAutoMapper(assembliesToScan)
                .RegisterRepositoryServices();
        }

        public static ContainerBuilder GenericRepositorySetupByAutofac(this ContainerBuilder containerBuilder,
            params Assembly[] assembliesToScan)
        {
            return containerBuilder.ScanAssemblesForAutoMapper(assembliesToScan)
                .RegisterRepositoryServicesByAutofac();
        }

        /// <summary>
        /// If you use ConfigureGenericServicesEntities, then you should follow it with this method to find/set up the DTOs
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="assembliesToScan"></param>
        /// <returns></returns>
        public static IGenericRepositorySetup ScanAssemblesForAutoMapper(this IServiceCollection serviceCollection,
            params Assembly[] assembliesToScan)
        {
            var dtosRegister = new SetupDtosAndMappings();
            var wrappedMapping = dtosRegister.ScanAllAssemblies(assembliesToScan);
            if (!dtosRegister.IsSuccess)
                throw new InvalidOperationException(
                    $"SETUP FAILED with {dtosRegister.Errors.Count} errors. Errors are:\n"
                    + dtosRegister.GetAllErrors());

            return new GenericRepositorySetup(serviceCollection, wrappedMapping);
        }

        public static IGenericRepositorySetup ScanAssemblesForAutoMapper(this ContainerBuilder containerBuilder,
            params Assembly[] assembliesToScan)
        {
            var dtosRegister = new SetupDtosAndMappings();
            var wrappedMapping = dtosRegister.ScanAllAssemblies(assembliesToScan);
            if (!dtosRegister.IsSuccess)
                throw new InvalidOperationException(
                    $"SETUP FAILED with {dtosRegister.Errors.Count} errors. Errors are:\n"
                    + dtosRegister.GetAllErrors());

            return new GenericRepositorySetup(containerBuilder, wrappedMapping);
        }

        /// <summary>
        /// If you used ScanAssemblesForDtos you should add this method on the end
        /// This registers all the services needed to run GenericServices. You will be able to access GenericServices
        /// via its interfaces: ICrudServices, <see cref="ICrudRepository{TContext}" /> and async versions
        /// </summary>
        /// <param name="setup"></param>
        /// <param name="singleContextToRegister">If you have one DbContext and you want to use the non-generic ICrudServices
        /// then GenericServices has to register your DbContext against your application's DbContext</param>
        /// <returns></returns>
        public static IServiceCollection RegisterRepositoryServices(this IGenericRepositorySetup setup)
        {
            //setup.Services.AddTransient(typeof(ICrudRepository<>), typeof(CrudRepository<>));
            //Register AutoMapper configuration goes here
            setup.Services.AddSingleton(setup.ConfigAndMapper);
            return setup.Services;
        }

        public static ContainerBuilder RegisterRepositoryServicesByAutofac(this IGenericRepositorySetup setup)
        {
            //setup.Container.RegisterType(typeof(CrudRepository<>)).As(typeof(ICrudRepository<>))
            //    .InstancePerLifetimeScope();
            //Register AutoMapper configuration goes here
            setup.Container.RegisterInstance(setup.ConfigAndMapper)
                .SingleInstance();

            return setup.Container;
        }
    }
}