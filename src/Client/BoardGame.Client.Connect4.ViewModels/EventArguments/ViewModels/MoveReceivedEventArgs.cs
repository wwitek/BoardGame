using BoardGame.Domain.Interfaces;
using System;

namespace BoardGame.Client.Connect4.ViewModels.EventArguments.ViewModels
{
    public class MoveReceivedEventArgs : EventArgs
    {
        public IMove MoveMade { get; set; }
    }
}
