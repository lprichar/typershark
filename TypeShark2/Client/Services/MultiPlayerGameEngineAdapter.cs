using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Client.Services
{
    public class MultiPlayerGameEngineAdapter : IGameEngineAdapter
    {
        public event EventHandler<List<PlayerDto>> PlayerAdded;
        public event EventHandler<SharkDto> SharkAdded;
        public event EventHandler<List<SharkDto>> SharksChanged;
        public event EventHandler<GameDto> GameOver;
        public event EventHandler<GameDto> GameChanged;

        private readonly NavigationManager _navigationManager;
        private readonly ILogger _logger;
        private HubConnection _hubConnection;
        private int _gameId;

        public async Task OnInitializeAsync(int? gameId, PlayerDto player)
        {
            if (gameId == null) throw new ArgumentNullException(nameof(gameId));

            _logger.LogInformation("Initializing SignalR connection for multi-player game");
            _gameId = gameId.Value;
            await SignalRInit();
            await JoinGame(gameId, player);
        }

        private async Task JoinGame(int? gameId, PlayerDto player)
        {
            await _hubConnection.SendAsync("JoinGame", gameId, player);
        }

        public MultiPlayerGameEngineAdapter(NavigationManager navigationManager, ILogger logger)
        {
            _navigationManager = navigationManager;
            _logger = logger;
        }

        private async Task SignalRInit()
        {
            var absoluteUri = _navigationManager.ToAbsoluteUri("/gamehub");
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(absoluteUri)
                .Build();
            _hubConnection.On<List<PlayerDto>>("PlayerAdded", OnPlayerAdded);
            _hubConnection.On<SharkDto>("SharkAdded", OnSharkAdded);
            _hubConnection.On<List<SharkDto>>("SharksChanged", OnSharkChanged);
            _hubConnection.On<GameDto>("GameChanged", OnGameChanged);
            _hubConnection.On<GameDto>("GameOver", OnGameOver);
            await _hubConnection.StartAsync();
        }

        private void OnSharkChanged(List<SharkDto> sharkDto)
        {
            _logger.LogInformation("OnSharkChanged");
            SharksChanged?.Invoke(this, sharkDto);
        }

        private void OnGameOver(GameDto gameDto)
        {
            GameOver?.Invoke(this, gameDto);
        }

        private void OnGameChanged(GameDto gameDto)
        {
            GameChanged?.Invoke(this, gameDto);
        }

        private void OnSharkAdded(SharkDto shark)
        {
            SharkAdded?.Invoke(this, shark);
        }

        private void OnPlayerAdded(List<PlayerDto> players)
        {
            PlayerAdded?.Invoke(this, players);
        }

        public async Task OnKeyPress(string key)
        {
            await _hubConnection.SendAsync("OnKeyPress", _gameId, key);
        }

        public async Task ToggleGameState()
        {
            await _hubConnection.SendAsync("ToggleGameState", _gameId);
        }

        public void Dispose()
        {
            _ = _hubConnection?.DisposeAsync();
        }
    }
}