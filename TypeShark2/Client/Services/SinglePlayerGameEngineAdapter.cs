using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeShark2.Shared.Dtos;
using TypeShark2.Shared.Services;

namespace TypeShark2.Client.Services
{
    public class SinglePlayerGameEngineAdapter : IGameEngineAdapter, IGameEngineEventHandler
    {
        public event EventHandler<SharkDto> SharkAdded;
        public event EventHandler<List<SharkDto>> SharksChanged;
        public event EventHandler<GameDto> GameOver;
        public event EventHandler<GameDto> GameChanged;
        public event EventHandler<List<PlayerDto>> PlayerAdded;

        private readonly IGameEngine _gameEngine;
        private readonly GameState _gameState;

        public SinglePlayerGameEngineAdapter()
        {
            _gameEngine = new GameEngine(this);
            _gameState = _gameEngine.CreateGame();
        }

        public Task OnInitializeAsync(int? gameId, PlayerDto player)
        {
            return Task.FromResult(true);
        }

        public async Task OnKeyPress(string key)
        {
            await _gameEngine.OnKeyPress(_gameState, key, null);
        }

        public async Task ToggleGameState()
        {
            await _gameEngine.ToggleGameState(_gameState);
        }

        void IGameEngineEventHandler.GameChanged(GameDto gameDto)
        {
            GameChanged?.Invoke(this, gameDto);
        }

        void IGameEngineEventHandler.GameOver(GameDto gameDto)
        {
            GameOver?.Invoke(this, gameDto);
        }

        void IGameEngineEventHandler.SharkAdded(SharkChangedEventArgs args)
        {
            SharkAdded?.Invoke(this, args.SharkDto);
        }

        public void Dispose()
        {
        }
    }
}