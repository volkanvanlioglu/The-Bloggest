using TheBloggest.Data.Models;

namespace TheBloggest.Interfaces
{
    public interface IPostService : IBaseService<Post>
    {
        Task<IEnumerable<Post>> GetPostsByAuthorAsync(string authorId);
    }
}