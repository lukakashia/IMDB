using IMDB.DBContext;
using IMDB.FilmElements;
using IMDB.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MovieManagementApi.Controllers
{
    [ApiController]
    [Route("api/movie")]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
   
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.Genres)
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }         
    }
}
