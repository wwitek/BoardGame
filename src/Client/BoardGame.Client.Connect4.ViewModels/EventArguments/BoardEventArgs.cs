using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Client.Connect4.ViewModels.EventArguments
{
    public class BoardEventArgs : EventArgs
    {
        public int ColumnClicked { get; set; }
    }
}
