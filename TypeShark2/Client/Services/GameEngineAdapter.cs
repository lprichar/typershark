using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Client.Services
{
    /// <summary>
    /// The game engine adapter passes messages to and from the game engine.  If user is in single player mode
    /// then this happens locally to support offline mode.  If user is in multi player then requests are passed
    /// to and received from a SignalR connection and managed on the server.
    /// </summary>
    public interface IGameEngineAdapter : IDisposable
    {
        Task OnKeyPress(string key);
        Task ToggleGameState();
        event EventHandler<SharkDto> SharkAdded;
        event EventHandler<List<SharkDto>> SharksChanged;
        event EventHandler<GameDto> GameOver;
        event EventHandler<GameDto> GameChanged;
        event EventHandler<List<PlayerDto>> PlayerAdded;
        Task OnInitializeAsync(int? gameId, PlayerDto player);
    }
}
