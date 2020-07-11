using System;
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
    }

    public class SinglePlayerGameEngine : IGameEngineAdapter
    {
        public event EventHandler<SharkDto> SharkAdded;
        public event EventHandler<GameDto> GameOver;
        public event EventHandler<GameDto> GameChanged;

        private readonly GameEngine _gameEngine;
        private readonly GameState _gameState;

        public SinglePlayerGameEngine()
        {
            _gameEngine = new GameEngine();
            _gameEngine.SharkAdded += OnSharkAdded;
            _gameEngine.GameOver += OnGameOver;
            _gameEngine.GameChanged += OnGameChanged;

            _gameState = _gameEngine.CreateGame();
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

    //public class MultiPlayerGameEngine : IGameEngineAdapter
    //{
    //}
}
