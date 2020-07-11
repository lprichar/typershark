using System.Collections.Generic;
using System.Linq;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Server.Services
{
    public interface IGamesService
    {
        GameDto Create(GameDto game);
        IEnumerable<GameDto> GetAll();
        void JoinGame(GameDto gameDto, PlayerDto playerDto);
        GameDto GetGame(int gameId);
    }

    public class GamesService : IGamesService
    {
        // GamesService is singleton so this is essentially static
        private readonly List<GameDto> _games = new List<GameDto>();

        public GameDto Create(GameDto game)
        {
            game.Id = _games.Count + 1;
            _games.Add(game);
            return game;
        }

        public IEnumerable<GameDto> GetAll()
        {
            return _games;
        }

        public void JoinGame(GameDto gameDto, PlayerDto playerDto)
        {
            if (gameDto.Players.Any(p => p.Name == playerDto.Name)) return;
            gameDto.Players.Add(playerDto);
        }

        public GameDto GetGame(int gameId)
        {
            return _games.FirstOrDefault(i => i.Id == gameId);
        }
    }
}
