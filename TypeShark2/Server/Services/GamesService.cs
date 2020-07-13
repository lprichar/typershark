using System.Collections.Generic;
using System.Linq;
using TypeShark2.Shared.Dtos;
using TypeShark2.Shared.Services;

namespace TypeShark2.Server.Services
{
    public interface IGamesService
    {
        GameDto Create(GameDto game);
        IEnumerable<GameDto> GetAll();
        void JoinGame(GameDto gameDto, PlayerDto playerDto);
        GameState GetGameState(int gameId);
        GameDto GetGameDto(int gameId);
    }

    public class GamesService : IGamesService
    {
        private readonly IGameEngine _gameEngine;

        // GamesService is singleton so this is essentially static
        private readonly List<GameState> _games = new List<GameState>();

        public GamesService(IGameEngine gameEngine)
        {
            _gameEngine = gameEngine;
        }

        public GameDto Create(GameDto game)
        {
            var gameState = _gameEngine.CreateGame();
            game.Id = _games.Count + 1;
            gameState.GameDto = game;
            _games.Add(gameState);
            return game;
        }

        public IEnumerable<GameDto> GetAll()
        {
            return _games.Select(i => i.GameDto);
        }

        public void JoinGame(GameDto gameDto, PlayerDto playerDto)
        {
            if (gameDto.Players.Any(p => p.Name == playerDto.Name)) return;
            gameDto.Players.Add(playerDto);
        }

        public GameState GetGameState(int gameId)
        {
            return _games.FirstOrDefault(i => i.GameDto.Id == gameId);
        }

        public GameDto GetGameDto(int gameId)
        {
            return GetGameState(gameId).GameDto;
        }
    }
}
