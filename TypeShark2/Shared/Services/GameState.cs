using System;
using System.Collections.Generic;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Shared.Services
{
    /// <summary>
    /// The elements of a game that are internal to a game engine and never be sent down from a server to a client
    /// </summary>
    public class GameState
    {
        public List<SharkManager> Sharks { get; set; }
        public GameDto GameDto { get; set; }
        public DateTime? LastKeypress = null;
    }

}
