using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BoardGame.Client.Connect4.ViewModels.Common
{
    public class Foo2
    {

    }
    //public class ActionCommand : ICommand
    //{
    //    private readonly Action<object> action;
    //    private readonly Predicate<object> predicate;
    //    public event EventHandler CanExecuteChanged;

    //    public ActionCommand(Action<object> action, Predicate<object> predicate = null)
    //    {
    //        if (action == null)
    //        {
    //            throw new ArgumentNullException(nameof(action), "You must specify an Action<T>.");
    //        }

    //        this.action = action;
    //        this.predicate = predicate;
    //    }

    //    public bool CanExecute(object parameter)
    //    {
    //        if (predicate == null)
    //        {
    //            return true;
    //        }
    //        return predicate(parameter);
    //    }

    //    public void Execute(object parameter = null)
    //    {
    //        action(parameter);
    //    }
    //}
}
