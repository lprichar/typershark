using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
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
                this.StateHasChanged();
            });
        }

        protected override async Task OnInitializedAsync()
        {
            InteropKeyPress.KeyPress += OnKeyPress;
            GameInit();

            var absoluteUri = NavigationManager.ToAbsoluteUri("/gamehub");
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(absoluteUri)
                .Build();

            _hubConnection.On<string>("PlayerAdded", playerName =>
            {
                Context.CurrentGame.Players.Add(new PlayerDto
                {
                    Name = playerName
                });
                StateHasChanged();
            });

            await _hubConnection.StartAsync();
        }

        private void GameInit()
        {
            if (CurrentGame == null)
            {
                CurrentGame = new Game();
                Console.WriteLine("Starting a new game");
            }
            else
            {
                Console.WriteLine("Game already in progress");
            }
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
                this.StateHasChanged();
            });
        }

        private void Test()
        {
            CurrentGame.Test();
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
        }
    }
}
