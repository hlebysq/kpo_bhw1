using HseBanking.HseBanking.Application.Commands;
using HseBanking.HseBanking.Domain.Enums;
using HseBanking.HseBanking.Domain.Models;

namespace HseBanking.HseBanking.Application.Services;

public interface IFinancialFacade
{
    Guid AddAccount(string name, decimal balance);
    ICommand CreateAddOperationCommand(
        OperationType type, 
        Guid accountId, 
        Guid categoryId, 
        decimal amount, 
        string? description = null);
    decimal CalculateBalance(Guid accountId);
    IEnumerable<Operation> GetAllOperations();
}