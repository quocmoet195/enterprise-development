namespace Hospital.Domain.Interfaces;

/// <summary>
/// Generic repository interface for basic CRUD operations.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T? Get(int id);
    T Add(T entity);
    bool Update(T entity);
    bool Delete(int id);
}
