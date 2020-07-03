using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TypeShark2.Shared;

namespace TypeShark2.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<GameDto> Get()
        {
            return new List<GameDto>
            {
                new GameDto
                {
                    Id = 1,
                    Name = "Bob's Game"
                },
                new GameDto
                {
                    Id = 2,
                    Name = "Sally's House"
                },
            };
        }
    }
}
