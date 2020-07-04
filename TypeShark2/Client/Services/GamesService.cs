using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TypeShark2.Shared;

namespace TypeShark2.Client.Services
{
    public interface IGamesService
    {
        Task<IList<GameDto>> GetGames();
        Task CreateGame(GameDto game);
    }

    public class GamesService : IGamesService
    {
        private readonly HttpClient _httpClient;

        public GamesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateGame(GameDto game)
        {
            await _httpClient.PostAsJsonAsync("api/games", game);
        }

        public async Task<IList<GameDto>> GetGames()
        {
            var gamesStream = await _httpClient.GetStreamAsync("api/games");
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await JsonSerializer.DeserializeAsync<List<GameDto>>(gamesStream, jsonOptions);
        }
    }
}
