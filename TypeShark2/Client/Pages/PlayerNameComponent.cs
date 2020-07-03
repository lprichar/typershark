using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using TypeShark2.Client.Data;

namespace TypeShark2.Client.Pages
{
    public partial class PlayerNameComponent
    {
        [Inject]
        private IGameContext Context { get; set; }

        private Player TempPlayer { get; set; } = new Player();

        [Parameter]
        public EventCallback OnSetName { get; set; }

        private async Task SetName()
        {
            Context.Player = TempPlayer;
            await OnSetName.InvokeAsync(null);
        }
    }
}
