using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class SettingsService : ISettingsService
    {
        private const string baseUrl = "api/Settings";
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        // In-memory storage for page visibility settings (for now)
        private static List<PageVisibilityOption> _pageVisibilitySettings = new()
        {
            new PageVisibilityOption { PageName = "Dashboard", IsVisible = true },
            new PageVisibilityOption { PageName = "Posts", IsVisible = true },
            new PageVisibilityOption { PageName = "Comments", IsVisible = false },
            new PageVisibilityOption { PageName = "Users", IsVisible = true },
            new PageVisibilityOption { PageName = "Profile", IsVisible = true },
            new PageVisibilityOption { PageName = "Analytics", IsVisible = false }
        };

        public SettingsService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        // Original settings methods
        public async Task<List<Settings>> GetAllAsync() => await _httpClient.GetFromJsonAsync<List<Settings>>($"{baseUrl}/Get") ?? [];

        public async Task<Settings?> GetByIdAsync(int id) => await _httpClient.GetFromJsonAsync<Settings>($"{baseUrl}/Get/{id}");

        public async Task<Settings?> CreateAsync(Settings entity)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/Create", entity);
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<Settings>() : null;
        }

        public async Task<bool> UpdateAsync(int id, Settings entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"{baseUrl}/Update/{id}", entity);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseUrl}/Delete/{id}");
            return response.IsSuccessStatusCode;
        }

        // Page visibility methods
        public async Task<List<PageVisibilityOption>> GetPageVisibilityAsync()
        {
            // Simulate async operation
            await Task.Delay(100);
            return new List<PageVisibilityOption>(_pageVisibilitySettings);
        }

        public async Task SavePageVisibilityAsync(List<PageVisibilityOption> settings)
        {
            // Simulate async operation
            await Task.Delay(200);
            _pageVisibilitySettings = new List<PageVisibilityOption>(settings);
        }

        public List<PageVisibilityOption> GetDefaultPageVisibility()
        {
            return new List<PageVisibilityOption>
            {
                new PageVisibilityOption { PageName = "Dashboard", IsVisible = true },
                new PageVisibilityOption { PageName = "Posts", IsVisible = true },
                new PageVisibilityOption { PageName = "Comments", IsVisible = false },
                new PageVisibilityOption { PageName = "Users", IsVisible = true },
                new PageVisibilityOption { PageName = "Profile", IsVisible = true },
                new PageVisibilityOption { PageName = "Analytics", IsVisible = false }
            };
        }
    }
}