using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TypeShark2.Shared;

namespace TypeShark2.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private static List<GameDto> _games = new List<GameDto>();

        [HttpPost]
        public void Create([FromBody] GameDto game)
        {
            game.Id = _games.Count + 1;
            _games.Add(game);
        }

        [HttpGet]
        public IEnumerable<GameDto> Get()
        {
            return _games;
        }
    }
}
