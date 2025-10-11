using TheBloggest.Data.Models;

namespace TheBloggest.Interfaces
{
    public interface ICommentService : IBaseService<Comment> 
    {
        Task<IEnumerable<Comment>> GetCommentsByUserAsync(string userId);
        Task<IEnumerable<Comment>> GetCommentsByPostAsync(string postId);
    }
}