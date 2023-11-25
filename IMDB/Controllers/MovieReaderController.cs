using Microsoft.AspNetCore.Mvc;
using IMDB.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[Route("api/movies")]
[ApiController]
public class MovieReaderController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public MovieReaderController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [AllowAnonymous] 
    public async Task<IActionResult> GetMovies()
    {
        var movies = await _dbContext.Movies.ToListAsync();
        var movie =  _dbContext.Movies
               .Include(m => m.Director)
               .Include(m => m.Genres)
               .Include(m => m.Actors)
               .ToList();
        return Ok(movies);
    }

}
