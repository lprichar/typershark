using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TypeShark2.Server.Services;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        [HttpPost]
        public GameDto Create([FromBody] GameDto game)
        {
            return _gamesService.Create(game);
        }

        [HttpGet]
        public IEnumerable<GameDto> Get()
        {
            return _gamesService.GetAll();
        }
    }
}
