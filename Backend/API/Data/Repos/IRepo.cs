namespace API.Data.Repos;

public interface IRepo<T> where T : class
{
    Task<T> Save(T entity);
    Task<List<T>> GetAll(string? userId);
    Task<T?> GetById(uint id, string? userId);
    Task Delete(T entity);
    Task SaveChangesAsync();
}

