using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly HttpClient _http;
        private const string baseUrl = "api/settings";

        public SettingsService(HttpClient http) => _http = http;

        public async Task<IEnumerable<Settings>> GetAllAsync() =>
            await _http.GetFromJsonAsync<IEnumerable<Settings>>($"{baseUrl}/Get") ?? [];

        public async Task<Settings?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<Settings>($"{baseUrl}/{id}");

        public async Task<Settings?> CreateAsync(Settings entity)
        {
            var response = await _http.PostAsJsonAsync(baseUrl, entity);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<Settings>()
                : null;
        }

        public async Task<bool> UpdateAsync(int id, Settings entity)
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