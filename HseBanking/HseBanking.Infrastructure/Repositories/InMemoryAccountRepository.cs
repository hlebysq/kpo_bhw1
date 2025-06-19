using System.Collections.Concurrent;
using HseBanking.HseBanking.Domain.Models;

namespace HseBanking.HseBanking.Infrastructure.Repositories;

public class InMemoryAccountRepository : IRepository<BankAccount>
{
    private readonly ConcurrentDictionary<Guid, BankAccount> _accounts = new();
        
    public BankAccount GetById(Guid id) => _accounts[id];
    public IEnumerable<BankAccount> GetAll() => _accounts.Values;
    public void Add(BankAccount entity) => _accounts[entity.Id] = entity;
}