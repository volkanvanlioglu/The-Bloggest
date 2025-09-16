using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;
        private const string baseUrl = "api/categories";

        public CategoryService(HttpClient http) => _http = http;

        public async Task<IEnumerable<Category>> GetAllAsync() =>
            await _http.GetFromJsonAsync<IEnumerable<Category>>(baseUrl) ?? [];

        public async Task<Category?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<Category>($"{baseUrl}/{id}");

        public async Task<Category?> CreateAsync(Category entity)
        {
            var response = await _http.PostAsJsonAsync(baseUrl, entity);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<Category>()
                : null;
        }

        public async Task<bool> UpdateAsync(int id, Category entity)
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