using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class PostService : IPostService
    {
        private const string baseUrl = "api/Posts";
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Post>> GetAllAsync() => await _httpClient.GetFromJsonAsync<List<Post>>($"{baseUrl}/Get");

        public async Task<Post?> GetByIdAsync(int id) => await _httpClient.GetFromJsonAsync<Post>($"{baseUrl}/Get/{id}");

        public async Task<Post?> CreateAsync(Post entity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/Create", entity);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Post>() : null;
        }

        public async Task<bool> UpdateAsync(int id, Post entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"{baseUrl}/Update/{id}", entity);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/Delete/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Post>> GetPostsByAuthorAsync(string authorId) => await _httpClient.GetFromJsonAsync<IEnumerable<Post>>($"{baseUrl}/GetPostsByAuthor?authorId={authorId}") ?? [];
    }
}