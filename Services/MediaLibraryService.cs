using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class MediaLibraryService : IMediaLibraryService
    {
        private readonly HttpClient _http;
        private const string baseUrl = "api/mediafiles";

        public MediaLibraryService(HttpClient http) => _http = http;

        public async Task<IEnumerable<MediaLibrary>> GetAllAsync() =>
            await _http.GetFromJsonAsync<IEnumerable<MediaLibrary>>($"{baseUrl}/Get") ?? [];

        public async Task<MediaLibrary?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<MediaLibrary>($"{baseUrl}/{id}");

        public async Task<MediaLibrary?> CreateAsync(MediaLibrary entity)
        {
            var response = await _http.PostAsJsonAsync(baseUrl, entity);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<MediaLibrary>()
                : null;
        }

        public async Task<bool> UpdateAsync(int id, MediaLibrary entity)
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