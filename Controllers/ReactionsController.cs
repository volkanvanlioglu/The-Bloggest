using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBloggest.Data;
using TheBloggest.Data.Models;

namespace TheBloggest.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReactionsController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Reaction>>> Get() => await _context.Reactions.Include(r => r.User).Include(r => r.Post).ToListAsync();

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Reaction>> Get(int id)
        {
            var entity = await _context.Reactions.Include(r => r.User).Include(r => r.Post).FirstOrDefaultAsync(r => r.Id == id);
            return entity == null ? NotFound() : entity;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Reaction>> Create(Reaction entity)
        {
            _context.Reactions.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Reaction entity)
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
            var entity = await _context.Reactions.FindAsync(id);
            if (entity == null) return NotFound();
            _context.Reactions.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}