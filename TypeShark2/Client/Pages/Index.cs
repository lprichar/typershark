using Microsoft.AspNetCore.Components;
using TypeShark2.Client.Data;

namespace TypeShark2.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public IGameContext Context { get; private set; }

        private bool? MultiPlayer = null;

        public void OnSetName(object obj)
        {
            // technically subscribing to the event on the child component is enough to call StateHasChanged, but for clarity:
            StateHasChanged();
        }

        private void SelectMultiPlayer()
        {
            MultiPlayer = true;
        }
    }
}
