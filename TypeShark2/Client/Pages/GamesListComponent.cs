using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeShark2.Client.Services;
using TypeShark2.Shared;

namespace TypeShark2.Client.Pages
{
    public partial class GamesListComponent
    {
        private IEnumerable<GameDto> Games { get; set; }

        [Inject]
        protected IGamesService GamesService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Games = await GamesService.GetGames();
        }
    }
}
