using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Client.Connect4.ViewModels.EventArguments.ViewModels
{
    public class BoardResetEventArgs : EventArgs
    {
        public bool IsAnimate { get; set; }
    }
}
