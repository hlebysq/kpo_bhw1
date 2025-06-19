namespace HseBanking.HseBanking.Infrastructure.Repositories;

public class CachingRepositoryProxy<T> : IRepository<T>
{
    private readonly IRepository<T> _realRepository;
    private Dictionary<Guid, T>? _cache;

    public CachingRepositoryProxy(IRepository<T> realRepository)
    {
        _realRepository = realRepository;
    }

    public void InitializeCache()
    {
        _cache = _realRepository.GetAll().ToDictionary(GetId, v => v);
    }

    private Guid GetId(T entity) => 
        (Guid)entity.GetType().GetProperty("Id")!.GetValue(entity)!;

    public T GetById(Guid id)
    {
        if (_cache == null) InitializeCache();
        return _cache!.TryGetValue(id, out var value) 
            ? value 
            : _realRepository.GetById(id);
    }

    public IEnumerable<T> GetAll() => 
        _cache?.Values ?? _realRepository.GetAll();

    public void Add(T entity)
    {
        _realRepository.Add(entity);
        _cache?.Add(GetId(entity), entity);
    }
}