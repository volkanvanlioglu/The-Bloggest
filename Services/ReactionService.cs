using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class ReactionService : IReactionService
    {
        private readonly HttpClient _http;
        private const string baseUrl = "api/reactions";

        public ReactionService(HttpClient http) => _http = http;

        public async Task<IEnumerable<Reaction>> GetAllAsync() =>
            await _http.GetFromJsonAsync<IEnumerable<Reaction>>(baseUrl) ?? [];

        public async Task<Reaction?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<Reaction>($"{baseUrl}/{id}");

        public async Task<Reaction?> CreateAsync(Reaction entity)
        {
            var response = await _http.PostAsJsonAsync(baseUrl, entity);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<Reaction>()
                : null;
        }

        public async Task<bool> UpdateAsync(int id, Reaction entity)
        {
            var response = await _http.PutAsJsonAsync($"{baseUrl}/{id}", entity);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"{baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}