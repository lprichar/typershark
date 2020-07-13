using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TypeShark2.Shared.Dtos;
using TypeShark2.Shared.Services;

namespace TypeShark2.Server.Hubs
{
    /// <summary>
    /// For sending messages to clients from background threads or outside of the GameHub
    /// see also: https://docs.microsoft.com/en-us/aspnet/core/signalr/hubcontext?view=aspnetcore-3.1
    /// </summary>
    public class MultiPlayerGameEngineEventHandler : IGameEngineEventHandler
    {
        private readonly IHubContext<GameHub> _hubContext;

        public MultiPlayerGameEngineEventHandler(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async void GameChanged(GameDto gameDto)
        {
            await SendToGroup("GameChanged", gameDto.Id, gameDto);
        }

        public async void GameOver(GameDto gameDto)
        {
            await SendToGroup("GameOver", gameDto.Id, gameDto);
        }

        public async void SharkAdded(SharkChangedEventArgs args)
        {
            await SendToGroup("SharkAdded", args.GameId, args.SharkDto);
        }

        private async Task SendToGroup<T>(string method, int? gameId, T arg)
        {
            if (gameId == null) throw new ArgumentNullException(nameof(gameId));
            var groupName = GameHub.GetGroupName(gameId.Value);
            await _hubContext.Clients.Group(groupName).SendAsync(method, arg);
        }
    }
}