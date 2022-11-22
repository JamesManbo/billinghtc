namespace GenericRepository.Models
{
    /// <summary>
    /// This allows you to link a class to an entity class. GenericServices then uses that to map your dto class to the entity class
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ILinkToEntity<TEntity> where TEntity : class { }
}
