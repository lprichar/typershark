using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TypeShark2.Shared;

namespace TypeShark2.Server.Hubs
{
    public class GameHub : Hub
    {
        public async Task AddPlayer(PlayerDto playerDto)
        {
            await AddPlayer(Clients.All, playerDto);
        }

        public static async Task AddPlayer(IClientProxy clients, PlayerDto playerDto)
        {
            await clients.SendAsync("PlayerAdded", playerDto.Name);
        }
    }
}
