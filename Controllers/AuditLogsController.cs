using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBloggest.Data;
using TheBloggest.Data.Models;

namespace TheBloggest.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class AuditLogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AuditLogsController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLogs>>> Get() => await _context.AuditLogs.Include(a => a.User).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<AuditLogs>> Get(int id)
        {
            var entity = await _context.AuditLogs.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);
            return entity == null ? NotFound() : entity;
        }

        [HttpPost]
        public async Task<ActionResult<AuditLogs>> Create(AuditLogs entity)
        {
            _context.AuditLogs.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AuditLogs entity)
        {
            if (id != entity.Id) return BadRequest();
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.AuditLogs.FindAsync(id);
            if (entity == null) return NotFound();
            _context.AuditLogs.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}