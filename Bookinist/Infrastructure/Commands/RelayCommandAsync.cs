using Bookinist.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bookinist.Infrastructure.Commands;

internal class RelayCommandAsync : Command
{
    private readonly Func<object?, Task> _ExecuteAsync;
    private readonly Func<object?, bool>? _CanExecuteAsync;

    private volatile Task? _ExecutingTask;
    public bool Background { get; set; }

    public RelayCommandAsync(Func<Task> ExecuteAsync, Func<bool>? CanExecute = null)
        : this(
            ExecuteAsync is null ? throw new ArgumentNullException(nameof(ExecuteAsync)) : new Func<object?, Task>(_ => ExecuteAsync()),
            CanExecute is null ? null : _ => CanExecute!())
    { }

    public RelayCommandAsync(Func<object?, Task> ExecuteAsync, Func<bool>? CanExecute)
        : this(ExecuteAsync, CanExecute is null ? null : _ => CanExecute!())
    { }

    public RelayCommandAsync(Func<object?, Task> ExecuteAsync, Func<object?, bool>? CanExecuteAsync = null)
    {
        _ExecuteAsync = ExecuteAsync ?? throw new ArgumentNullException(nameof(ExecuteAsync));
        _CanExecuteAsync = CanExecuteAsync;
    }

    public override bool CanExecute(object? parameter) =>
        (_ExecutingTask is null || _ExecutingTask.IsCompleted)
        && (_CanExecuteAsync?.Invoke(parameter) ?? true);

    public override async void Execute(object? parameter)
    {
        var background = Background;

        var can_execute = background
            ? await Task.Run(() => CanExecute(parameter))
            : CanExecute(parameter);
        if (!can_execute) return;

        var execute_async = background ? Task.Run(() => _ExecuteAsync(parameter)) : _ExecuteAsync(parameter);
        _ = Interlocked.Exchange(ref _ExecutingTask, execute_async);
        _ExecutingTask = execute_async;

        try
        {
            await execute_async.ConfigureAwait(true);
        }
        catch (OperationCanceledException)
        {

        }
    }
}
