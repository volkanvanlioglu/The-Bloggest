using TheBloggest.Data.Models;

namespace TheBloggest.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Guid id, ApplicationUser entity);
        Task<bool> DeleteAsync(Guid id);
    }
}