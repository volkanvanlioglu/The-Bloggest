using System.Net.Http;
using System.Net.Http.Json;
using TheBloggest.Data;
using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class PostService
    {
        private readonly HttpClient _http;
        private const string baseUrl = "api/Posts";

        //public PostService(HttpClient http) => _http = http;

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            var cookie = _httpContextAccessor.HttpContext?.Request
                .Headers["Cookie"].ToString();

            if (!string.IsNullOrEmpty(cookie))
            {
                _httpClient.DefaultRequestHeaders.Remove("Cookie");
                _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
            }

            return await _httpClient.GetFromJsonAsync<List<Post>>("api/Posts/GetPosts");
        }

        //public async Task<IEnumerable<Post>> GetAllAsync() =>
        //    await _http.GetFromJsonAsync<IEnumerable<Post>>($"{baseUrl}/GetPosts") ?? [];

        public async Task<Post?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<Post>($"{baseUrl}/{id}");

        public async Task<Post?> CreateAsync(Post entity)
        {
            var cookie = _httpContextAccessor.HttpContext?.Request
                .Headers["Cookie"].ToString();

            if (!string.IsNullOrEmpty(cookie))
            {
                _httpClient.DefaultRequestHeaders.Remove("Cookie");
                _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
            }

            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/Create", entity);

            //var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/Create", entity);
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