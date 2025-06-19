namespace HseBanking.HseBanking.Infrastructure.Repositories;

public interface IRepository<T>
{
    T GetById(Guid id);
    IEnumerable<T> GetAll();
    void Add(T entity);
}