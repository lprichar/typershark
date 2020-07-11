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
        Task<GameDto> CreateGame(GameDto game);
    }

    public class GamesProxy : IGamesService
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private readonly HttpClient _httpClient;

        public GamesProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GameDto> CreateGame(GameDto game)
        {
            var response = await _httpClient.PostAsJsonAsync("api/games", game);
            var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<GameDto>(responseStream, _jsonOptions);
        }

        public async Task<IList<GameDto>> GetGames()
        {
            return await _httpClient.GetFromJsonAsync<List<GameDto>>("api/games");
        }
    }
}
