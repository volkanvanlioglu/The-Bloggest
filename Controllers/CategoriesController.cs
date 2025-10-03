using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBloggest.Data;
using TheBloggest.Data.Models;

namespace TheBloggest.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> Get() => await _context.Categories.ToListAsync();

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            return entity == null ? NotFound() : entity;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> Create(Category entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Category entity)
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
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null) return NotFound();
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}