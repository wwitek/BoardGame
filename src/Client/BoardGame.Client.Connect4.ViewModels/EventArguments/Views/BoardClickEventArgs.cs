using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Client.Connect4.ViewModels.EventArguments.Views
{
    public class BoardClickEventArgs : EventArgs
    {
        public int ColumnClicked { get; set; }
    }
}
