using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class CommentService : ICommentService
    {
        private const string baseUrl = "api/Comments";
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Comment>> GetAllAsync() => await _httpClient.GetFromJsonAsync<List<Comment>>($"{baseUrl}/Get") ?? [];

        public async Task<Comment?> GetByIdAsync(int id) => await _httpClient.GetFromJsonAsync<Comment>($"{baseUrl}/Get/{id}");

        public async Task<Comment?> CreateAsync(Comment entity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/Create", entity);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Comment>() : null;
        }

        public async Task<bool> UpdateAsync(int id, Comment entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"{baseUrl}/Update/{id}", entity);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/Delete/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserAsync(string userId) => await _httpClient.GetFromJsonAsync<IEnumerable<Comment>>($"{baseUrl}/GetCommentsByUser?userId={userId}") ?? [];
    }
}