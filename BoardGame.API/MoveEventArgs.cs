using System;
using BoardGame.Domain.Interfaces;

namespace BoardGame.API
{
    public class MoveEventArgs : EventArgs
    {
        public IMove Move { get; set; }
    }
}
