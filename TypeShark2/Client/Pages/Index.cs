using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using TypeShark2.Client.Data;

namespace TypeShark2.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public IGameContext Context { get; private set; }

        [Inject]
        public ILogger<Index> Logger { get; set; }

        private bool? MultiPlayer = null;

        protected override void OnInitialized()
        {
            Logger.LogInformation("Starting typershark");
        }

        public void OnSetName(object obj)
        {
            // technically subscribing to the event on the child component is enough to call StateHasChanged, but for clarity:
            StateHasChanged();
        }

        private void SelectMultiPlayer()
        {
            MultiPlayer = true;
        }

        // public string RadioValue { get; set; }
        public void RadioSelection(ChangeEventArgs args)
        {
            Context.GameIcon = Enum.Parse<GameIcon>(args.Value.ToString());
        }
    }
}
