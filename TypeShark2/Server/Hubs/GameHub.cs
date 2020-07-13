using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using TypeShark2.Server.Services;
using TypeShark2.Shared.Dtos;
using TypeShark2.Shared.Services;

namespace TypeShark2.Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGamesService _gamesService;
        private readonly IGameEngine _gameEngine;

        public GameHub(IGamesService gamesService, IGameEngine gameEngine)
        {
            _gamesService = gamesService;
            _gameEngine = gameEngine;
        }

        public async Task JoinGame(int gameId, PlayerDto playerDto)
        {
            Context.Items["UserName"] = playerDto.Name;
            var gameDto = _gamesService.GetGameDto(gameId);
            if (gameDto == null) throw new ArgumentException("Invalid game id " + gameId);
            _gamesService.JoinGame(gameDto, playerDto);

            await SendToGroup("PlayerAdded", gameId, gameDto.Players);
        }

        public async Task OnKeyPress(int gameId, string key)
        {
            var userName = Context.Items["UserName"] as string;
            var gameState = _gamesService.GetGameState(gameId);
            var changedSharks = await _gameEngine.OnKeyPress(gameState, key, userName);
            if (changedSharks.Any())
            {
                await SendToGroup("SharksChanged", gameId, changedSharks);
            }
        }

        private async Task SendToGroup<T>(string method, int? gameId, T arg)
        {
            if (gameId == null) throw new ArgumentNullException(nameof(gameId));
            var groupName = GetGroupName(gameId.Value);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync(method, arg);
        }

        public static string GetGroupName(int groupId)
        {
            return groupId.ToString();
        }

        public async Task ToggleGameState(int gameId)
        {
            var gameState = _gamesService.GetGameState(gameId);
            if (gameState == null) throw new ArgumentException("Invalid game id " + gameId);

            await _gameEngine.ToggleGameState(gameState);
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
