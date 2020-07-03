using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeShark2.Client.Services;
using TypeShark2.Shared;

namespace TypeShark2.Client.Pages
{
    public partial class GamesListComponent
    {
        private IEnumerable<GameDto> PublicGames { get; set; }

        [Inject]
        protected IGamesService GamesService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PublicGames = await GamesService.GetGames();
        }

        protected void NewGame()
        {
            // todo: create new game, upload
            NavigationManager.NavigateTo("/game");
        }
    }
}
