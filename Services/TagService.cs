using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class TagService : ITagService
    {
        private const string baseUrl = "api/Tags";
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TagService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Tag>> GetAllAsync() => await _httpClient.GetFromJsonAsync<List<Tag>>($"{baseUrl}/Get") ?? [];

        public async Task<Tag?> GetByIdAsync(int id) => await _httpClient.GetFromJsonAsync<Tag>($"{baseUrl}/Get/{id}");

        public async Task<Tag?> CreateAsync(Tag entity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/Create", entity);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Tag>(): null;
        }

        public async Task<bool> UpdateAsync(int id, Tag entity)
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