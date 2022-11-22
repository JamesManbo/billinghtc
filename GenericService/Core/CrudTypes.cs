namespace GenericRepository.Core
{
    internal enum CrudTypes
    {
        None = 0,
        Create = 1,
        ReadOne = 2,
        ReadMany = 4,
        Update = 8,
        Delete = 16
    }
}
