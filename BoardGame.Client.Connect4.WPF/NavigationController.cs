using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BoardGame.Client.Connect4.WPF
{
    public class NavigationController : INavigationController
    {
        public Frame GameFrame { get; }
        public Dictionary<string, Page> Pages { get; }

        public NavigationController(Frame frame)
        {
            Pages = new Dictionary<string, Page>();
            GameFrame = frame;
        }

        public void Navigate(string name)
        {
            if (Pages.ContainsKey(name))
            {
                Page page;
                if (Pages.TryGetValue(name, out page))
                {
                    GameFrame.Navigate(page);
                }
            }
        }

    }
}
