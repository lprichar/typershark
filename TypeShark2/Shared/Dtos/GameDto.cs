using System.Collections.Generic;

namespace TypeShark2.Shared.Dtos
{
    /// <summary>
    /// The elements of a game that should be shared between multiple players in a multi-player game
    /// </summary>
    public class GameDto
    {
        public List<PlayerDto> Players { get; set; }
        public List<SharkDto> Sharks { get; set; }

        public int? Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public bool IsStarted { get; set; } = false;
        public bool IsEasy { get; set; }
        public string Message { get; set; }
    }
}
