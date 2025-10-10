using TheBloggest.Data.Models;

namespace TheBloggest.Interfaces
{
    public interface ISettingsService : IBaseService<Settings>
    {
        Task<List<PageVisibilityOption>> GetPageVisibilityAsync();
        Task SavePageVisibilityAsync(List<PageVisibilityOption> settings);
        List<PageVisibilityOption> GetDefaultPageVisibility();
    }
}