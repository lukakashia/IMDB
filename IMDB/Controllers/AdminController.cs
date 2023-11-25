using IMDB.DBContext;
using IMDB.FilmElements;
using IMDB.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/Admin")]

public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public AdminController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpPost("add-movie")]
    public IActionResult AddMovie([FromBody] Movie movie)
    {
        _dbContext.Movies.Add(movie);
        _dbContext.SaveChanges();
        return Ok("Movie added successfully");
    }
    
    [HttpDelete("remove-movie/{id}")]
    public IActionResult RemoveMovie(int id)
    {
        var movie = _dbContext.Movies.Find(id);
        if (movie == null)
        {
            return NotFound("Movie not found");
        }
        var actor = _dbContext.Actors.Find(id);
        var genre = _dbContext.Genres.Find(id);
        var director = _dbContext.Directors.Find(id);

        _dbContext.Movies.Remove(movie);
        _dbContext.Directors.Remove(director);
        _dbContext.Actors.Remove(actor);
        _dbContext.Genres.Remove(genre);
        _dbContext.SaveChanges();
        return Ok("Movie removed successfully");
    }

    [HttpPut("update-movie/{id}")]
    public IActionResult UpdateMovie(int id, [FromBody] Movie updatedMovie)
    {
        var movie = _dbContext.Movies.Find(id);
        if (movie == null)
        {
            return NotFound("Movie not found");
        }

        movie.Title = updatedMovie.Title;
        movie.Description = updatedMovie.Description;

        _dbContext.SaveChanges();
        return Ok("Movie updated successfully");
    }
 
    [HttpPost("add-ban")]
    public IActionResult AddUserBan([FromBody] UserBan userBan)
    {
        _dbContext.UserBans.Add(userBan);
        _dbContext.SaveChanges();
        return Ok("User banned successfully");
    }

    [HttpDelete("remove-ban/{id}")]
    public IActionResult RemoveUserBan(int id)
    {
        var userBan = _dbContext.UserBans.Find(id);
        if (userBan == null)
        {
            return NotFound("User ban not found");
        }

        _dbContext.UserBans.Remove(userBan);
        _dbContext.SaveChanges();
        return Ok("User ban removed successfully");
    }
}
