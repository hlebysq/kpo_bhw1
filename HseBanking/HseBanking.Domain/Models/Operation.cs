using HseBanking.HseBanking.Domain.Enums;

namespace HseBanking.HseBanking.Domain.Models;

public class Operation
{
    public Guid Id { get; } = Guid.NewGuid();
    public OperationType Type { get; set; }
    public Guid BankAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
}