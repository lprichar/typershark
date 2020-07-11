using Microsoft.AspNetCore.Components;
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
            if (IsMultiPlayer && Context.Player == null)
            {
                NavigationManager.NavigateTo("/");
            }

            InteropKeyPress.KeyPress += OnKeyPress;
            GameInit();

            var gameId = GetGameId();

            Context.CurrentGame = new GameDto
            {
                Id = gameId,
                Players = new List<PlayerDto>(),
                Sharks = new List<SharkDto>()
            };

            await _gameEngine.OnInitializeAsync(gameId, Context.Player);
        }

        private void OnPlayerAdded(object sender, List<PlayerDto> playersInGame)
        {
            Context.CurrentGame.Players = playersInGame;
            StateHasChanged();
        }

        private void GameInit()
        {
            if (IsMultiPlayer)
            {
                _gameEngine = new MultiPlayerGameEngine(NavigationManager);
            }
            else
            {
                _gameEngine = new SinglePlayerGameEngine();
            }

            _gameEngine.SharkAdded += SharkAdded;
            _gameEngine.GameOver += GameOver;
            _gameEngine.GameChanged += GameChanged;
            _gameEngine.PlayerAdded += OnPlayerAdded;
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
            InteropKeyPress.KeyPress -= OnKeyPress;
            _gameEngine.SharkAdded -= SharkAdded;
            _gameEngine.GameOver -= GameOver;
            _gameEngine.GameChanged -= GameChanged;
            _gameEngine.PlayerAdded -= OnPlayerAdded;
            _gameEngine.Dispose();
        }
    }
}
