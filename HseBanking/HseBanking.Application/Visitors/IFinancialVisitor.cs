using HseBanking.HseBanking.Domain.Models;

namespace HseBanking.HseBanking.Application.Visitors;

public interface IFinancialVisitor
{
    void Visit(BankAccount account);
    void Visit(Operation operation);
}