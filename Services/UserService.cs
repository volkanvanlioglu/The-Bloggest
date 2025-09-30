using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class UserService : IUserService
    {
        private const string baseUrl = "api/Users";

        //public PostService(HttpClient http) => _http = http;

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            var cookie = _httpContextAccessor.HttpContext?.Request
                .Headers["Cookie"].ToString();

            if (!string.IsNullOrEmpty(cookie))
            {
                _httpClient.DefaultRequestHeaders.Remove("Cookie");
                _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
            }

            return await _httpClient.GetFromJsonAsync<List<ApplicationUser>>($"{baseUrl}/GetApplicationUsers");
        }

        //public async Task<IEnumerable<Post>> GetAllAsync() =>
        //    await _http.GetFromJsonAsync<IEnumerable<Post>>($"{baseUrl}/GetPosts") ?? [];

        public async Task<ApplicationUser?> GetByIdAsync(Guid id)
        {
            var cookie = _httpContextAccessor.HttpContext?.Request
                .Headers["Cookie"].ToString();

            if (!string.IsNullOrEmpty(cookie))
            {
                _httpClient.DefaultRequestHeaders.Remove("Cookie");
                _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
            }

            return await _httpClient.GetFromJsonAsync<ApplicationUser>($"{baseUrl}/GetApplicationUser/{id}");
        }

        public async Task<ApplicationUser?> CreateAsync(ApplicationUser entity)
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
                ? await response.Content.ReadFromJsonAsync<ApplicationUser>()
                : null;
        }

        public async Task<bool> UpdateAsync(Guid id, ApplicationUser entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"{baseUrl}/{id}", entity);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}