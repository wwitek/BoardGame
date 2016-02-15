using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame.Client.Connect4.ViewModels.EventArgs
{
    public class BoardEventArgs
    {
        public int Column { get; private set; }

        public BoardEventArgs(int column)
        {
            Column = column;
        }
    }
}
