using System.Diagnostics;

namespace HseBanking.HseBanking.Application.Commands;

public class TimedCommandDecorator : ICommand
{
    private readonly ICommand _decorated;
    private readonly Action<string> _logger;

    public TimedCommandDecorator(ICommand decorated, Action<string> logger)
    {
        _decorated = decorated;
        _logger = logger;
    }

    public void Execute()
    {
        var watch = Stopwatch.StartNew();
        _decorated.Execute();
        watch.Stop();
        _logger?.Invoke($"Operation completed in {watch.ElapsedMilliseconds}ms");
    }
}