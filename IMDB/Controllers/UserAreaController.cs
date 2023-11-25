using IMDB.DBContext;
using IMDB.FilmElements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IMDB.Controllers
{

    [ApiController]    
    [Route("api/UserArea")]
    public class UserAreaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserAreaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("UserMarks")]
        public async Task<ActionResult> SubmitRating([FromBody] MovieActionsDB actionsdb)
        {
            if (actionsdb == null)
            {
                return BadRequest("Invalid rating data.");
            }

            var movie = await _context.Movies.FindAsync(actionsdb.MovieId);
            if (movie == null)
            {
                return NotFound("Movie not found.");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingAction = await _context.UserArea
                .FirstOrDefaultAsync(r => r.MovieId == actionsdb.MovieId && r.UserId == userId);

            if (existingAction != null)
            {
                return Conflict("You have already rated this movie.");
            }

            actionsdb.UserId = userId;

            _context.UserArea.Add(actionsdb);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                actionsdb.Id,
                actionsdb.UserId,
                actionsdb.MovieId,
                actionsdb.MovieTitle,
                actionsdb.IsWatched,
                actionsdb.IsCurrentlyWatching,
                actionsdb.WantToWatch
            });
        }
    }
}
