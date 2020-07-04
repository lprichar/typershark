using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TypeShark2.Shared;

namespace TypeShark2.Client.Services
{
    public interface IGamesService
    {
        Task<IList<GameDto>> GetGames();
    }

    public class GamesService : IGamesService
    {
        private readonly HttpClient _httpClient;

        public GamesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IList<GameDto>> GetGames()
        {
            var gamesStream = await _httpClient.GetStreamAsync("games");
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await JsonSerializer.DeserializeAsync<List<GameDto>>(gamesStream, jsonOptions);
        }
    }
}
