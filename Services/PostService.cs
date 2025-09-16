using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class PostService : IPostService
    {
        private readonly HttpClient _http;
        private const string baseUrl = "api/posts";

        public PostService(HttpClient http) => _http = http;

        public async Task<IEnumerable<Post>> GetAllAsync() =>
            await _http.GetFromJsonAsync<IEnumerable<Post>>(baseUrl) ?? [];

        public async Task<Post?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<Post>($"{baseUrl}/{id}");

        public async Task<Post?> CreateAsync(Post entity)
        {
            var response = await _http.PostAsJsonAsync(baseUrl, entity);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<Post>()
                : null;
        }

        public async Task<bool> UpdateAsync(int id, Post entity)
        {
            var response = await _http.PutAsJsonAsync($"{baseUrl}/{id}", entity);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"{baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Post>> GetPostsByAuthorAsync(string authorId) =>
            await _http.GetFromJsonAsync<IEnumerable<Post>>($"{baseUrl}?authorId={authorId}") ?? [];
    }
}