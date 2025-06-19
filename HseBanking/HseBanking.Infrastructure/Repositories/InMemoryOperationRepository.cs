using System.Collections.Concurrent;
using HseBanking.HseBanking.Domain.Models;

namespace HseBanking.HseBanking.Infrastructure.Repositories;

public class InMemoryOperationRepository : IRepository<Operation>
{
    private readonly ConcurrentDictionary<Guid, Operation> _operations = new();
        
    public Operation GetById(Guid id) => _operations[id];
    public IEnumerable<Operation> GetAll() => _operations.Values;
    public void Add(Operation entity) => _operations[entity.Id] = entity;
}