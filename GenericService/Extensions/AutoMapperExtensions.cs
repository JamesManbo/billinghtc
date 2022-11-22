using AutoMapper;

namespace GenericRepository.Extensions
{
    public static class AutoMapperExtensions
    {
        public static TItem MapTo<TItem>(this object sourceObject, MapperConfiguration mapperConfiguration)
        {
            var mapper = new Mapper(mapperConfiguration);
            return mapper.Map<TItem>(sourceObject);
        }
    }
}
