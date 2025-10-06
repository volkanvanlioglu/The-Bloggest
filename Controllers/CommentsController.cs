using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBloggest.Data;
using TheBloggest.Data.Models;

namespace TheBloggest.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CommentsController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Comment>>> Get() => await _context.Comments.Include(c => c.User).Include(c => c.Post).ToListAsync();

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Comment>> Get(int id)
        {
            var entity = await _context.Comments.FindAsync(id);
            return entity == null ? NotFound() : entity;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> Create(Comment entity)
        {
            _context.Comments.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Comment entity)
        {
            if (id != entity.Id) return BadRequest();
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Comments.FindAsync(id);
            if (entity == null) return NotFound();
            _context.Comments.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 🔒 Get posts by author (for user's own posts)
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByUserAsync([FromQuery] string userId)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            // Users can only see their own posts, admins can see any user's posts
            if (!isAdmin && userId != currentUserId)
                return Forbid();

            var posts = await _context.Comments.Where(p => p.UserId == userId).Include(p => p.User).OrderByDescending(p => p.CreatedAt).ToListAsync();

            return posts;
        }
    }
}