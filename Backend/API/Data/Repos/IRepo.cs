namespace API.Data.Repos;

public interface IRepo<T> where T : class
{
    Task<T> Save(T entity);
    Task<List<T>> GetAll();
    Task<T?> GetById(uint id);
    Task Delete(T entity);
    Task SaveChangesAsync();
}

