using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBloggest.Data;
using TheBloggest.Data.Models;

namespace TheBloggest.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MediaLibrariesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public MediaLibrariesController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MediaLibrary>>> Get() => await _context.MediaLibraries.Include(m => m.UploadedBy).ToListAsync();

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<MediaLibrary>> Get(int id)
        {
            var entity = await _context.MediaLibraries.Include(m => m.UploadedBy).FirstOrDefaultAsync(m => m.Id == id);
            return entity == null ? NotFound() : entity;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MediaLibrary>> Create(MediaLibrary entity)
        {
            _context.MediaLibraries.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, MediaLibrary entity)
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
            var entity = await _context.MediaLibraries.FindAsync(id);
            if (entity == null) return NotFound();
            _context.MediaLibraries.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}