using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeShark2.Client.Data;
using TypeShark2.Client.JsInterop;
using TypeShark2.Client.Services;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Client.Pages
{
    public partial class GameComponent : IDisposable
    {
        [Inject]
        private IGameContext Context { get; set; }

        protected GameDto CurrentGame => Context.CurrentGame;

        [Parameter]
        public string GameId { get; set; }

        private HubConnection _hubConnection;
        private IGameEngineAdapter _gameEngine;
        private int? GetGameId() => string.IsNullOrEmpty(GameId) ? (int?)null : int.Parse(GameId);
        private bool IsMultiPlayer => !string.IsNullOrEmpty(GameId);

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private async void OnKeyPress(object sender, string key)
        {
            await InvokeAsync(async () =>
            {
                await _gameEngine.OnKeyPress(key);
                StateHasChanged();
            });
        }

        protected override async Task OnInitializedAsync()
        {
            InteropKeyPress.KeyPress += OnKeyPress;
            GameInit();

            var gameId = GetGameId();

            Context.CurrentGame = new GameDto
            {
                Id = gameId,
                Players = new List<PlayerDto>(),
                Sharks = new List<SharkDto>()
            };

            if (IsMultiPlayer)
            {
                await SignalRInit();
                await JoinGame(gameId);
            }
        }

        private async Task JoinGame(int? gameId)
        {
            var playerDto = new PlayerDto { Name = Context.Player.PlayerName };
            await _hubConnection.SendAsync("JoinGame", gameId, playerDto);
        }

        private void OnPlayerAdded(List<PlayerDto> playersInGame)
        {
            Context.CurrentGame.Players = playersInGame;
            StateHasChanged();
        }

        private async Task SignalRInit()
        {
            var absoluteUri = NavigationManager.ToAbsoluteUri("/gamehub");
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(absoluteUri)
                .Build();
            _hubConnection.On<List<PlayerDto>>("PlayerAdded", OnPlayerAdded);
            await _hubConnection.StartAsync();
        }

        private void GameInit()
        {
            if (IsMultiPlayer)
            {
                throw new NotImplementedException("Implement MultiPlayerGameEngine");
                //_gameEngine = new MultiPlayerGameEngine();
            }
            else
            {
                _gameEngine = new SinglePlayerGameEngine();
            }

            _gameEngine.SharkAdded += SharkAdded;
            _gameEngine.GameOver += GameOver;
            _gameEngine.GameChanged += GameChanged;
        }

        private void GameChanged(object sender, GameDto game)
        {
            Context.CurrentGame.IsStarted = game.IsStarted;
            Context.CurrentGame.Message = game.Message;
            Context.CurrentGame.IsEasy = game.IsEasy;
            Context.CurrentGame.Score = game.Score;
        }

        private void SharkAdded(object sender, SharkDto shark)
        {
            Context.CurrentGame.Sharks.Add(shark);
            InvokeAsync(StateHasChanged);
        }

        private void GameOver(object sender, GameDto gameDto)
        {
            Context.CurrentGame.Sharks = new List<SharkDto>();
            Context.CurrentGame.Message = gameDto.Message;
            Context.CurrentGame.IsStarted = gameDto.IsStarted;
            InvokeAsync(StateHasChanged);
        }

        public async Task ToggleGameState()
        {
            await _gameEngine.ToggleGameState();
        }

        public void Dispose()
        {
            _ = _hubConnection?.DisposeAsync();
            InteropKeyPress.KeyPress -= OnKeyPress;
            _gameEngine.SharkAdded -= SharkAdded;
            _gameEngine.GameOver -= GameOver;
        }
    }
}
