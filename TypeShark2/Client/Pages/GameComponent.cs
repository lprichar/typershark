using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeShark2.Client.Data;
using TypeShark2.Client.JsInterop;
using TypeShark2.Shared;

namespace TypeShark2.Client.Pages
{
    public partial class GameComponent : IDisposable
    {
        [Inject]
        public IGameContext Context { get; private set; }

        [Parameter]
        public string GameId { get; set; }

        private int? GetGameId() => string.IsNullOrEmpty(GameId) ? (int?)null : int.Parse(GameId);

        public static Game CurrentGame;
        protected static string Message { get; set; }

        private HubConnection _hubConnection;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private async void OnKeyPress(object sender, string key)
        {
            await InvokeAsync(() =>
            {
                CurrentGame.OnKeyPress(key);
                StateHasChanged();
            });
        }

        protected override async Task OnInitializedAsync()
        {
            InteropKeyPress.KeyPress += OnKeyPress;
            GameInit();

            var gameId = GetGameId();
            bool multiPlayer = gameId != null;

            Context.CurrentGame = new GameDto
            {
                Id = gameId,
                Players = new List<PlayerDto>()
            };

            if (multiPlayer)
            {
                SignalRInit();

                await _hubConnection.StartAsync();
                var playerDto = new PlayerDto { Name = Context.Player.PlayerName };
                await _hubConnection.SendAsync("JoinGame", gameId, playerDto);
            }
        }

        private void OnPlayerAdded(List<PlayerDto> playersInGame)
        {
            Context.CurrentGame.Players = playersInGame;
            StateHasChanged();
        }

        private void SignalRInit()
        {
            var absoluteUri = NavigationManager.ToAbsoluteUri("/gamehub");
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(absoluteUri)
                .Build();
            _hubConnection.On<List<PlayerDto>>("PlayerAdded", OnPlayerAdded);
        }

        private void GameInit()
        {
            CurrentGame = new Game();
            CurrentGame.SharkAdded += SharkAdded;
            CurrentGame.GameOver += GameOver;
        }

        private void SharkAdded(object sender, EventArgs e)
        {
            InvokeAsync(this.StateHasChanged);
        }

        private void GameOver(object sender, EventArgs e)
        {
            InvokeAsync(() =>
            {
                Message = "Game over, congratulations you scored " + CurrentGame.Score;
                StateHasChanged();
            });
        }

        public async Task ToggleGameState()
        {
            if (CurrentGame.IsStarted)
            {
                CurrentGame.Stop();
            }
            else
            {
                CurrentGame.Clear();
                Message = "";
                await CurrentGame.Start();
            }
        }

        public void Dispose()
        {
            _ = _hubConnection?.DisposeAsync();
            CurrentGame.SharkAdded -= SharkAdded;
            CurrentGame.GameOver -= GameOver;
        }
    }
}
