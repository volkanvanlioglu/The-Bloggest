using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class MediaLibraryService : IMediaLibraryService
    {
        private const string baseUrl = "api/MediaLibrary";
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MediaLibraryService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<MediaLibrary>> GetAllAsync() => await _httpClient.GetFromJsonAsync<List<MediaLibrary>>($"{baseUrl}/Get") ?? [];

        public async Task<MediaLibrary?> GetByIdAsync(int id) => await _httpClient.GetFromJsonAsync<MediaLibrary>($"{baseUrl}/Get/{id}");

        public async Task<MediaLibrary?> CreateAsync(MediaLibrary entity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/Create", entity);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<MediaLibrary>() : null;
        }

        public async Task<bool> UpdateAsync(int id, MediaLibrary entity)
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