namespace TypeShark2.Client.Data
{
    public interface IGameContext
    {
        string PlayerName { get; set; }
        public Game CurrentGame { get; set; }
    }

    /// <summary>
    /// A singleton that owns the context data for a single player's gaming session
    /// </summary>
    public class GameContext : IGameContext
    {
        public string PlayerName { get; set; }
        public Game CurrentGame { get; set; }
    }
}
