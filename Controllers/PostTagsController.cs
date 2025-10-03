using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBloggest.Data;
using TheBloggest.Data.Models;

namespace TheBloggest.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PostTagsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public PostTagsController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PostTag>>> Get() =>
            await _context.PostTags
                .Include(pt => pt.Post)
                .Include(pt => pt.Tag)
                .ToListAsync();

        [HttpGet("{postId}/{tagId}")]
        [AllowAnonymous]
        public async Task<ActionResult<PostTag>> Get(int postId, int tagId)
        {
            var postTag = await _context.PostTags
                .Include(pt => pt.Post)
                .Include(pt => pt.Tag)
                .FirstOrDefaultAsync(pt => pt.PostId == postId && pt.TagId == tagId);
            return postTag == null ? NotFound() : postTag;
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<PostTag>> Create(PostTag postTag)
        {
            _context.PostTags.Add(postTag);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { postId = postTag.PostId, tagId = postTag.TagId }, postTag);
        }

        [HttpPut("{postId}/{tagId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Update(int postId, int tagId, PostTag postTag)
        {
            if (postId != postTag.PostId || tagId != postTag.TagId)
                return BadRequest();

            _context.Entry(postTag).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{postId}/{tagId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete(int postId, int tagId)
        {
            var postTag = await _context.PostTags.FindAsync(postId, tagId);
            if (postTag == null) return NotFound();

            _context.PostTags.Remove(postTag);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}