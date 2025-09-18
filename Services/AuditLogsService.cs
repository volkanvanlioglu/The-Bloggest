using TheBloggest.Data.Models;
using TheBloggest.Interfaces;

namespace TheBloggest.Services
{
    public class AuditLogsService : IAuditLogsService
    {
        private readonly HttpClient _http;
        private const string baseUrl = "api/auditlogs";

        public AuditLogsService(HttpClient http) => _http = http;

        public async Task<IEnumerable<AuditLogs>> GetAllAsync() =>
            await _http.GetFromJsonAsync<IEnumerable<AuditLogs>>($"{baseUrl}/Get") ?? [];

        public async Task<AuditLogs?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<AuditLogs>($"{baseUrl}/{id}");

        public async Task<AuditLogs?> CreateAsync(AuditLogs entity)
        {
            var response = await _http.PostAsJsonAsync(baseUrl, entity);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<AuditLogs>()
                : null;
        }

        public async Task<bool> UpdateAsync(int id, AuditLogs entity)
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