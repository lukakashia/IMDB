using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using IMDB.DBContext;
using IMDB.FilmElements;
using System.Security.Claims;

namespace IMDB.Controllers
{
    [Route("api/ratings")]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public RatingController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("submit-rating")]
        public async Task<ActionResult> SubmitRating([FromBody] Rating rating)
        {
            if (rating == null)
            {
                return BadRequest("Invalid rating data.");
            }

            var movie = await _dbContext.Movies.FindAsync(rating.MovieId);
            if (movie == null)
            {
                return NotFound("Movie not found.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingRating = await _dbContext.Ratings
                .FirstOrDefaultAsync(r => r.MovieId == rating.MovieId && r.UserId == userId);

            if (existingRating != null)
            {
                return Conflict("You have already rated this movie.");
            }

            rating.UserId = userId;

            _dbContext.Ratings.Add(rating);
             await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    rating.Id,
                    rating.UserId,
                    rating.MovieId,
                    rating.Value
                });
        }
    }
}
