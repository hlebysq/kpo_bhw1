using HseBanking.HseBanking.Domain.Models;

namespace HseBanking.HseBanking.Application.Commands;

public class AddOperationCommand : ICommand
{
    private readonly Operation _operation;
    private readonly Action<Operation> _addAction;

    public AddOperationCommand(Operation operation, Action<Operation> addAction)
    {
        _operation = operation;
        _addAction = addAction;
    }

    public void Execute() => _addAction(_operation);
}