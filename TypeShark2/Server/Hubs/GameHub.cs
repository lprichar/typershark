using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using TypeShark2.Server.Services;
using TypeShark2.Shared;

namespace TypeShark2.Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGamesService _gamesService;

        public GameHub(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        public async Task JoinGame(int gameId, PlayerDto playerDto)
        {
            var gameDto = _gamesService.GetGame(gameId);
            if (gameDto == null) throw new ArgumentException("Invalid game id " + gameId);
            _gamesService.JoinGame(gameDto, playerDto);

            var groupName = gameId.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("PlayerAdded", gameDto.Players);
        }
    }
}
