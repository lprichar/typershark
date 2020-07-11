using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeShark2.Shared.Dtos;
using TypeShark2.Shared.Services;

namespace TypeShark2.Client.Services
{
    public interface IGameEngineAdapter : IDisposable
    {
        Task OnKeyPress(string key);
        Task ToggleGameState();
        event EventHandler<SharkDto> SharkAdded;
        event EventHandler<GameDto> GameOver;
        event EventHandler<GameDto> GameChanged;
        event EventHandler<List<PlayerDto>> PlayerAdded;
        Task OnInitializeAsync(int? gameId, PlayerDto player);
    }

    public class SinglePlayerGameEngine : IGameEngineAdapter
    {
        public event EventHandler<SharkDto> SharkAdded;
        public event EventHandler<GameDto> GameOver;
        public event EventHandler<GameDto> GameChanged;
        public event EventHandler<List<PlayerDto>> PlayerAdded;

        private readonly GameEngine _gameEngine;
        private readonly GameState _gameState;

        public SinglePlayerGameEngine()
        {
            _gameEngine = new GameEngine();
            _gameState = _gameEngine.CreateGame();
        }

        public Task OnInitializeAsync(int? gameId, PlayerDto player)
        {
            _gameEngine.SharkAdded += OnSharkAdded;
            _gameEngine.GameOver += OnGameOver;
            _gameEngine.GameChanged += OnGameChanged;
            return Task.FromResult(true);
        }

        private void OnGameChanged(object sender, GameDto gameDto)
        {
            GameChanged?.Invoke(sender, gameDto);
        }

        private void OnGameOver(object sender, EventArgs e)
        {
            _gameState.GameDto.Message = "Game over, congratulations you scored " + _gameState.GameDto.Score;
            GameOver?.Invoke(sender, _gameState.GameDto);
        }

        private void OnSharkAdded(object sender, SharkDto shark)
        {
            SharkAdded?.Invoke(sender, shark);
        }

        public void Dispose()
        {
            _gameEngine.SharkAdded -= OnSharkAdded;
            _gameEngine.GameOver -= OnGameOver;
            _gameEngine.GameChanged -= OnGameChanged;
        }

        public async Task OnKeyPress(string key)
        {
            await _gameEngine.OnKeyPress(_gameState, key);
        }

        public async Task ToggleGameState()
        {
            await _gameEngine.ToggleGameState(_gameState);
        }
    }

    public class MultiPlayerGameEngine : IGameEngineAdapter
    {
        public event EventHandler<List<PlayerDto>> PlayerAdded;

        public async Task OnInitializeAsync(int? gameId, PlayerDto player)
        {
            await SignalRInit();
            await JoinGame(gameId, player);
        }

        private async Task JoinGame(int? gameId, PlayerDto player)
        {
            await _hubConnection.SendAsync("JoinGame", gameId, player);
        }

        private readonly NavigationManager _navigationManager;

        public MultiPlayerGameEngine(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        private HubConnection _hubConnection;

        private async Task SignalRInit()
        {
            var absoluteUri = _navigationManager.ToAbsoluteUri("/gamehub");
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(absoluteUri)
                .Build();
            _hubConnection.On<List<PlayerDto>>("PlayerAdded", OnPlayerAdded);
            await _hubConnection.StartAsync();
        }

        private void OnPlayerAdded(List<PlayerDto> players)
        {
            PlayerAdded?.Invoke(this, players);
        }

        public Task OnKeyPress(string key)
        {
            throw new NotImplementedException();
        }

        public Task ToggleGameState()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<SharkDto> SharkAdded;
        public event EventHandler<GameDto> GameOver;
        public event EventHandler<GameDto> GameChanged;

        public void Dispose()
        {
            _ = _hubConnection?.DisposeAsync();
        }
    }
}
