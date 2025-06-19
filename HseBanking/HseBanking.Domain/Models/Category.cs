using HseBanking.HseBanking.Domain.Enums;

namespace HseBanking.HseBanking.Domain.Models;

public class Category
{
    public Guid Id { get; } = Guid.NewGuid();
    public OperationType Type { get; set; }
    public string Name { get; set; } = "Unnamed Category";
}