namespace HseBanking.HseBanking.Domain.Models;

public class BankAccount
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "New Account";
    public decimal Balance { get; set; } = 0;
}