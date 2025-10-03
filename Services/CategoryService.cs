using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class CategoryService : ICategoryService
    {
        private const string baseUrl = "api/Categories";
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Category>> GetAllAsync() => await _httpClient.GetFromJsonAsync<List<Category>>($"{baseUrl}/Get") ?? [];

        public async Task<Category?> GetByIdAsync(int id) => await _httpClient.GetFromJsonAsync<Category>($"{baseUrl}/Get/{id}");

        public async Task<Category?> CreateAsync(Category entity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/Create", entity);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Category>() : null;
        }

        public async Task<bool> UpdateAsync(int id, Category entity)
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