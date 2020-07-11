using TypeShark2.Shared.Dtos;

namespace TypeShark2.Client.Data
{
    public interface IGameContext
    {
        public PlayerDto Player { get; set; }
        public GameDto CurrentGame { get; set; }
    }

    /// <summary>
    /// A singleton that owns the context data for a single player's gaming session
    /// </summary>
    public class GameContext : IGameContext
    {
        public PlayerDto Player { get; set; }
        public GameDto CurrentGame { get; set; }
    }
}
