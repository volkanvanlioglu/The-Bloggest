using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBloggest.Data;
using TheBloggest.Data.Models;

namespace TheBloggest.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context) => _context = context;

        // ✅ Public: anyone can read ApplicationUsers
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> Get() => await _context.Users.ToListAsync();

        // ✅ Public
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApplicationUser>> Get(Guid id)
        {
            var ApplicationUser = await _context.Users.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id.ToString());
            return ApplicationUser == null ? NotFound() : ApplicationUser;
        }

        // 🔒 Users and Admins can create ApplicationUsers
        //[HttpPost]
        //[Authorize(Roles = "User,Admin")]
        //public async Task<ActionResult<ApplicationUser>> Create(ApplicationUser ApplicationUser)
        //{
        //    // Set the author ID from the current user
        //    ApplicationUser.Id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(ApplicationUser.Id))
        //        return Unauthorized();

        //    _context.Users.Add(ApplicationUser);
        //    await _context.SaveChangesAsync();
        //    return CreatedAtAction(nameof(GetByIdAsync), new { id = ApplicationUser.Id }, ApplicationUser);
        //}

        // 🔒 Users can edit their own ApplicationUsers, Admins can edit any ApplicationUser
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ApplicationUser ApplicationUser)
        {
            if (id != Guid.Parse(ApplicationUser.Id)) return BadRequest();

            var existingApplicationUser = await _context.Users.FindAsync(id.ToString());
            if (existingApplicationUser == null) return NotFound();

            // Update the ApplicationUser
            existingApplicationUser.DisplayName = ApplicationUser.DisplayName;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //// 🔒 Users can delete their own ApplicationUsers, Admins can delete any ApplicationUser
        //[HttpDelete("{id}")]
        //[Authorize(Roles = "User,Admin")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var ApplicationUser = await _context.Users.FindAsync(id);
        //    if (ApplicationUser == null) return NotFound();

        //    var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //    var isAdmin = User.IsInRole("Admin");

        //    // Users can only delete their own ApplicationUsers, admins can delete any ApplicationUser
        //    if (!isAdmin && ApplicationUser.AuthorId != currentUserId)
        //        return Forbid();

        //    _context.Users.Remove(ApplicationUser);
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}
    }
}