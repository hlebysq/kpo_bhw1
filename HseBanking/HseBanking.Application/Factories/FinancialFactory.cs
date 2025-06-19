using HseBanking.HseBanking.Domain.Enums;
using HseBanking.HseBanking.Domain.Models;

namespace HseBanking.HseBanking.Application.Factories;

public class FinancialFactory
{
    public BankAccount CreateAccount(string name, decimal initialBalance = 0)
    {
        if (initialBalance < 0)
            throw new ArgumentException("Balance cannot be negative");
            
        return new BankAccount { Name = name, Balance = initialBalance };
    }

    public Operation CreateOperation(
        OperationType type,
        Guid accountId,
        Guid categoryId,
        decimal amount,
        string? description = null)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
            
        return new Operation {
            Type = type,
            BankAccountId = accountId,
            CategoryId = categoryId,
            Amount = amount,
            Description = description
        };
    }
}