using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using TypeShark2.Client.Data;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public IGameContext Context { get; private set; }

        [Inject]
        public ILogger<Index> Logger { get; set; }

        private PlayerDto TempPlayer { get; } = new PlayerDto();

        private bool? MultiPlayer = null;

        protected override void OnInitialized()
        {
            Logger.LogInformation("Starting typershark");
        }

        public void SetName()
        {
            Context.Player = TempPlayer;
        }

        private void SelectMultiPlayer()
        {
            MultiPlayer = true;
        }
    }
}
