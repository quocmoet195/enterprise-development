namespace Hospital.Domain.Interfaces;

/// <summary>
/// Generic repository interface for basic CRUD operations.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public interface IRepository<T>
{
    public IEnumerable<T> GetAll();
    public T? Get(int id);
    public T Add(T entity);
    public bool Update(T entity);
    public bool Delete(int id);
}
