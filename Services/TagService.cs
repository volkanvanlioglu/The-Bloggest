using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class TagService : ITagService
    {
        private readonly HttpClient _http;
        private const string baseUrl = "api/tags";

        public TagService(HttpClient http) => _http = http;

        public async Task<List<Tag>> GetAllAsync() =>
            await _http.GetFromJsonAsync<List<Tag>>($"{baseUrl}/Get") ?? [];

        public async Task<Tag?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<Tag>($"{baseUrl}/{id}");

        public async Task<Tag?> CreateAsync(Tag entity)
        {
            var response = await _http.PostAsJsonAsync(baseUrl, entity);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<Tag>()
                : null;
        }

        public async Task<bool> UpdateAsync(int id, Tag entity)
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