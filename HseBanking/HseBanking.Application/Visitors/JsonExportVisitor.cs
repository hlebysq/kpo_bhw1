using System.Text;
using HseBanking.HseBanking.Domain.Models;

namespace HseBanking.HseBanking.Application.Visitors;

public class JsonExportVisitor : IFinancialVisitor
{
    private readonly StringBuilder _json = new StringBuilder("{");

    public string GetJson() => _json.Append('}').ToString();
        
    public void Visit(BankAccount account)
    {
        _json.Append($@"
                ""accounts"": [{{
                    ""id"": ""{account.Id}"",
                    ""name"": ""{account.Name}"",
                    ""balance"": {account.Balance}
                }}],");
    }

    public void Visit(Operation operation)
    {
        _json.Append($@"
                ""operations"": [{{
                    ""id"": ""{operation.Id}"",
                    ""type"": ""{operation.Type}"",
                    ""accountId"": ""{operation.BankAccountId}"",
                    ""amount"": {operation.Amount},
                    ""date"": ""{operation.Date:O}""
                }}],");
    }
}