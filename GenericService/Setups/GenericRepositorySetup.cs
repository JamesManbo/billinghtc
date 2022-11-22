// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using System;
using Autofac;
using GenericRepository.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace GenericRepository.Setups
{
    /// <summary>
    /// Used to chain ConfigureGenericServicesEntities to RegisterGenericServices
    /// </summary>
    public class GenericRepositorySetup : IGenericRepositorySetup
    {

        internal GenericRepositorySetup(IServiceCollection services, IWrappedConfigAndMapper configAndMapper)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            ConfigAndMapper = configAndMapper ?? throw new ArgumentNullException(nameof(configAndMapper));
        }

        internal GenericRepositorySetup(ContainerBuilder container, IWrappedConfigAndMapper configAndMapper)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
            ConfigAndMapper = configAndMapper ?? throw new ArgumentNullException(nameof(configAndMapper));
        }

        /// <inheritdoc />
        public IServiceCollection Services { get; }

        public ContainerBuilder Container { get; }

        /// <inheritdoc />
        public IWrappedConfigAndMapper ConfigAndMapper { get; }
    }
}