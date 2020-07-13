using System;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Shared.Services
{
    public class SharkChangedEventArgs : EventArgs
    {
        public int GameId { get; set; }
        public SharkDto SharkDto { get; set; }
    }
}