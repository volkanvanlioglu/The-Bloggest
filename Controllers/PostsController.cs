using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBloggest.Data;
using TheBloggest.Data.Models;

namespace TheBloggest.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PostsController(ApplicationDbContext context) => _context = context;

        // ✅ Public: anyone can read posts
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts() =>
            await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .ToListAsync();

        // ✅ Public
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
            return post == null ? NotFound() : post;
        }

        // 🔒 Users and Admins can create posts
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Post>> Create(Post post)
        {
            // Set the author ID from the current user
            post.AuthorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(post.AuthorId))
                return Unauthorized();

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }

        // 🔒 Users can edit their own posts, Admins can edit any post
        [HttpPut("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Update(int id, Post post)
        {
            if (id != post.Id) return BadRequest();
            
            var existingPost = await _context.Posts.FindAsync(id);
            if (existingPost == null) return NotFound();
            
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            
            // Users can only edit their own posts, admins can edit any post
            if (!isAdmin && existingPost.AuthorId != currentUserId)
                return Forbid();
            
            // Update the post
            existingPost.Title = post.Title;
            existingPost.Content = post.Content;
            existingPost.Excerpt = post.Excerpt;
            existingPost.CoverImageUrl = post.CoverImageUrl;
            existingPost.IsPublished = post.IsPublished;
            existingPost.Slug = post.Slug;
            existingPost.UpdatedAt = DateTime.UtcNow;
            
            // Handle published date
            if (post.IsPublished && !existingPost.PublishedAt.HasValue)
            {
                existingPost.PublishedAt = DateTime.UtcNow;
            }
            else if (!post.IsPublished && existingPost.PublishedAt.HasValue)
            {
                existingPost.PublishedAt = null;
            }
            
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 🔒 Users can delete their own posts, Admins can delete any post
        [HttpDelete("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();
            
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            
            // Users can only delete their own posts, admins can delete any post
            if (!isAdmin && post.AuthorId != currentUserId)
                return Forbid();
            
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 🔒 Get posts by author (for user's own posts)
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByAuthor([FromQuery] string authorId)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            
            // Users can only see their own posts, admins can see any user's posts
            if (!isAdmin && authorId != currentUserId)
                return Forbid();
            
            var posts = await _context.Posts
                .Where(p => p.AuthorId == authorId)
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
                
            return posts;
        }
    }
}
