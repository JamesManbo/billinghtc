using System;
using AutoMapper;

namespace GenericRepository.Configurations
{
    /// <summary>
    /// This is the interface used for dependency injection of the <see cref="WrappedAndMapper"/>
    /// </summary>
    public interface IWrappedConfigAndMapper
    {
        /// <summary>
        /// This is the AutoMapper configuration used for reading/projection from entity class to DTO
        /// </summary>
        MapperConfiguration MapperConfig { get; }
    }

    /// <summary>
    /// This contains the AutoMapper setting needed by Services
    /// </summary>
    public class WrappedAndMapper : IWrappedConfigAndMapper
    {
        internal WrappedAndMapper(MapperConfiguration mapperConfig)
        {
            MapperConfig = mapperConfig ?? throw new ArgumentNullException(nameof(mapperConfig));
        }

        /// <inheritdoc />
        public MapperConfiguration MapperConfig { get; }
    }
}
