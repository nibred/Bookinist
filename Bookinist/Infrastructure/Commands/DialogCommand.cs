using Bookinist.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bookinist.Infrastructure.Commands;

internal class DialogCommand : Command
{
    private readonly Action<object> _execute;
    private readonly Func<object, bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;
    public DialogCommand(Action<object> execute, Func<object, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }
    public override bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;
    public override void Execute(object parameter)
    {
        Application.Current.Windows.OfType<Window>().FirstOrDefault(i => i.IsActive).Close();
    }
}
