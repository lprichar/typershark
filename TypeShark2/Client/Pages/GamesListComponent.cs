using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeShark2.Client.Data;
using TypeShark2.Client.Services;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Client.Pages
{
    public partial class GamesListComponent
    {
        private IList<GameDto> PublicGames { get; set; }

        private GameDto NewGame { get; set; } = new GameDto();

        [Inject]
        protected IGameContext Context { get; set; }

        [Inject]
        protected IGamesService GamesService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PublicGames = await GamesService.GetGames();
        }

        protected async Task CreateGame()
        {
            NewGame.Players = new List<PlayerDto>
            {
                new PlayerDto { Name = Context.Player.PlayerName }
            };
            var newGame = await GamesService.CreateGame(NewGame);
            Context.CurrentGame = newGame;
            NavigationManager.NavigateTo($"/game/{newGame.Id}");
        }
    }
}
