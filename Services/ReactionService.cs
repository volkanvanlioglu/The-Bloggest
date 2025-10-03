using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class ReactionService : IReactionService
    {
        private const string baseUrl = "api/Reactions";
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReactionService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

            var cookie = _httpContextAccessor.HttpContext?.Request.Headers["Cookie"].ToString();

            if (!string.IsNullOrEmpty(cookie))
            {
                _httpClient.DefaultRequestHeaders.Remove("Cookie");
                _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
            }
        }

        public async Task<List<Reaction>> GetAllAsync() => await _httpClient.GetFromJsonAsync<List<Reaction>>($"{baseUrl}/Get") ?? [];

        public async Task<Reaction?> GetByIdAsync(int id) => await _httpClient.GetFromJsonAsync<Reaction>($"{baseUrl}/Get/{id}");

        public async Task<Reaction?> CreateAsync(Reaction entity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/Create", entity);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Reaction>() : null;
        }

        public async Task<bool> UpdateAsync(int id, Reaction entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"{baseUrl}/Update/{id}", entity);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/Delete/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}