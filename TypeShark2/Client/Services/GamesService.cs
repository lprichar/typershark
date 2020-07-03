using System.Collections.Generic;
using System.Threading.Tasks;
using TypeShark2.Shared;

namespace TypeShark2.Client.Services
{
    public interface IGamesService
    {
        Task<IEnumerable<GameDto>> GetGames();
    }

    public class GamesService : IGamesService
    {
        public async Task<IEnumerable<GameDto>> GetGames()
        {
            await Task.Yield();

            return new List<GameDto>
            {
                new GameDto
                {
                    Id = 5,
                    Name = "5"
                },
                new GameDto
                {
                    Id = 6,
                    Name = "6"
                }
            };
        }
    }
}
