using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using GenericRepository.Configurations;
using GenericRepository.Core;
using GenericRepository.Models;
using Global.Models.StateChangedResponse;

namespace GenericRepository.Setups
{
    internal class SetupDtosAndMappings : ActionResponse
    {

        public SetupDtosAndMappings()
        {
        }

        public IWrappedConfigAndMapper ScanAllAssemblies(Assembly[] assembliesToScan)
        {
            if (assembliesToScan == null || assembliesToScan.Length == 0)
                throw new ArgumentException("There were no assembles to scan!", nameof(assembliesToScan));
            
            return CreateConfigAndMapper(assembliesToScan);
        }

        public static IWrappedConfigAndMapper CreateConfigAndMapper(Assembly[] assembliesToScan)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // Add all profiles in current assembly
                foreach (var assembly in assembliesToScan)
                {
                    cfg.AddMaps(assembly);
                }
            });

            return new WrappedAndMapper(mapperConfig);
        }
    }
}