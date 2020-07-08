using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TypeShark2.Shared;

namespace TypeShark2.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private static List<GameDto> _games = new List<GameDto>();

        [HttpPost]
        public GameDto Create([FromBody] GameDto game)
        {
            game.Id = _games.Count + 1;
            _games.Add(game);
            return game;
        }

        [HttpPut("{gameId}")]
        [ProducesResponseType(200, Type = typeof(GameDto))]
        public ObjectResult Join(int gameId, [FromBody] GameDto game)
        {
            var gameDto = _games.FirstOrDefault(i => i.Id == game.Id);
            if (gameDto == null) return new NotFoundObjectResult(null);
            gameDto.Players.Add(game.Players.First());
            return Ok(gameDto);
        }

        [HttpGet]
        public IEnumerable<GameDto> Get()
        {
            return _games;
        }
    }
}
