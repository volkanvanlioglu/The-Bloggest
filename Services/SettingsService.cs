using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class SettingsService : ISettingsService
    {
        private const string baseUrl = "api/Settings";
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SettingsService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Settings>> GetAllAsync() => await _httpClient.GetFromJsonAsync<List<Settings>>($"{baseUrl}/Get") ?? [];

        public async Task<Settings?> GetByIdAsync(int id) => await _httpClient.GetFromJsonAsync<Settings>($"{baseUrl}/Get/{id}");

        public async Task<Settings?> CreateAsync(Settings entity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/Create", entity);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Settings>() : null;
        }

        public async Task<bool> UpdateAsync(int id, Settings entity)
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