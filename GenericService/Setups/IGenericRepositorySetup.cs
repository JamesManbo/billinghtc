using Autofac;
using GenericRepository.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace GenericRepository.Setups
{
    /// <summary>
    /// Used to chain ConfigureGenericServicesEntities to RegisterGenericServices
    /// </summary>
    public interface IGenericRepositorySetup
    {
        /// <summary>
        /// The DI ServiceCollection to use for registering
        /// </summary>
        IServiceCollection Services { get; }
        /// <summary>
        /// The DI ServiceCollection to use for registering
        /// </summary>
        ContainerBuilder Container { get; }

        /// <summary>
        /// The AutoMapper setting needed by GenericServices
        /// </summary>
        IWrappedConfigAndMapper ConfigAndMapper { get; }
    }
}