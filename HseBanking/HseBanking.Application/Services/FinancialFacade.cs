using HseBanking.HseBanking.Application.Commands;
using HseBanking.HseBanking.Application.Factories;
using HseBanking.HseBanking.Domain.Enums;
using HseBanking.HseBanking.Domain.Models;
using HseBanking.HseBanking.Infrastructure.Repositories;

namespace HseBanking.HseBanking.Application.Services;

public class FinancialFacade : IFinancialFacade
{
    private readonly IRepository<BankAccount> _accountRepository;
    private readonly IRepository<Operation> _operationRepository;
    private readonly FinancialFactory _factory;

    public FinancialFacade(
        IRepository<BankAccount> accountRepository,
        IRepository<Operation> operationRepository,
        FinancialFactory factory)
    {
        _accountRepository = accountRepository;
        _operationRepository = operationRepository;
        _factory = factory;
    }

    public Guid AddAccount(string name, decimal balance)
    {
        var account = _factory.CreateAccount(name, balance);
        _accountRepository.Add(account);
        return account.Id;
    }

    public ICommand CreateAddOperationCommand(
        OperationType type,
        Guid accountId,
        Guid categoryId,
        decimal amount,
        string? description = null)
    {
        var operation = _factory.CreateOperation(
            type, accountId, categoryId, amount, description);
        
            
        return new AddOperationCommand(
            operation, 
            op => _operationRepository.Add(op));
    }

    public decimal CalculateBalance(Guid accountId)
    {
        var account = _accountRepository.GetById(accountId);
        if (account == null)
            throw new ArgumentException("Account not found", nameof(accountId));

        var operations = _operationRepository.GetAll()
            .Where(op => op.BankAccountId == accountId)
            .ToList();

        decimal balance = account.Balance;

        foreach (var op in operations)
        {
            if (op.Type == OperationType.Income)
                balance += op.Amount;
            else
                balance -= op.Amount;
        }

        return balance;
    }

    public IEnumerable<Operation> GetAllOperations()
    {
        return _operationRepository.GetAll();
    }
}