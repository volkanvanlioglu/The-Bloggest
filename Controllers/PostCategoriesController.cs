using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBloggest.Data;
using TheBloggest.Data.Models;

namespace TheBloggest.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PostCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PostCategoriesController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PostCategory>>> Get() => await _context.PostCategories.Include(pc => pc.Post).Include(pc => pc.Category).ToListAsync();

        [HttpGet("{postId}/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<PostCategory>> Get(int postId, int categoryId)
        {
            var postCategory = await _context.PostCategories.Include(pc => pc.Post).Include(pc => pc.Category).FirstOrDefaultAsync(pc => pc.PostId == postId && pc.CategoryId == categoryId);
            return postCategory == null ? NotFound() : postCategory;
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<PostCategory>> Create(PostCategory postCategory)
        {
            _context.PostCategories.Add(postCategory);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { postId = postCategory.PostId, categoryId = postCategory.CategoryId }, postCategory);
        }

        [HttpPut("{postId}/{categoryId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Update(int postId, int categoryId, PostCategory postCategory)
        {
            if (postId != postCategory.PostId || categoryId != postCategory.CategoryId)
                return BadRequest();

            _context.Entry(postCategory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{postId}/{categoryId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete(int postId, int categoryId)
        {
            var postCategory = await _context.PostCategories.FindAsync(postId, categoryId);
            if (postCategory == null) return NotFound();

            _context.PostCategories.Remove(postCategory);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}