using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BoardGame.Client.Connect4.WPF
{
    public interface INavigationController
    {
        Frame GameFrame { get; }
        Dictionary<string, Page> Pages { get; }

        void Navigate(string name);
    }
}
