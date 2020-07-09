using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeShark2.Server.Hubs;
using TypeShark2.Shared;

namespace TypeShark2.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IHubContext<GameHub> _gameHub;
        private static readonly List<GameDto> _games = new List<GameDto>();

        public GamesController(IHubContext<GameHub> hubContext)
        {
            _gameHub = hubContext;
        }

        [HttpPost]
        public GameDto Create([FromBody] GameDto game)
        {
            game.Id = _games.Count + 1;
            _games.Add(game);
            return game;
        }

        [HttpPut("{gameId}")]
        [ProducesResponseType(200, Type = typeof(GameDto))]
        public async Task<ObjectResult> Join(int gameId, [FromBody] GameDto game)
        {
            var gameDto = _games.FirstOrDefault(i => i.Id == gameId);
            if (gameDto == null) return new NotFoundObjectResult(null);
            var playerDto = game.Players.First();
            gameDto.Players.Add(playerDto);

            await GameHub.AddPlayer(_gameHub.Clients.All, playerDto);

            return Ok(gameDto);
        }

        [HttpGet]
        public IEnumerable<GameDto> Get()
        {
            return _games;
        }
    }
}
