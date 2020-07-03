using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using TypeShark2.Client.Data;

namespace TypeShark2.Client.Pages
{
    public partial class PlayerNameComponent
    {
        [Inject]
        public IGameContext Context { get; set; }

        public string TempName { get; set; }

        [Parameter]
        public EventCallback OnSetName { get; set; }

        public async Task SetName()
        {
            Context.PlayerName = TempName;
            await OnSetName.InvokeAsync(null);
        }
    }
}
