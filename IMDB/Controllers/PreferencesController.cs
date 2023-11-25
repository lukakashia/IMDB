using Azure.Core;
using IMDB.DBContext;
using IMDB.FilmElements;
using IMDB.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;


[Authorize]
[Route("api/preferences")]

public class PreferencesController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public PreferencesController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserPreferences()
    {
   
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var preferences = await _dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == userId);

        return Ok(preferences);
    }
    [HttpPost("add-preference")]
    public async Task<IActionResult> AddUserPreference([FromBody] AddUserPreference model)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("You must be authenticated to set preferences.");
        }

        var existingPreferences = await _dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == userId);

        List<string> actorPreferences = new List<string>();
        List<string> genrePreferences = new List<string>();
        List<string> directorPreferences = new List<string>();

        if (existingPreferences != null)
        {
            actorPreferences = JsonConvert.DeserializeObject<List<string>>(existingPreferences.FavoriteActors);
            genrePreferences = JsonConvert.DeserializeObject<List<string>>(existingPreferences.FavoriteGenres);
            directorPreferences = JsonConvert.DeserializeObject<List<string>>(existingPreferences.FavoriteDirectors);
        }

        if (actorPreferences.Contains(model.Actor))
        {
            throw new InvalidOperationException("The actor already exists in your preferences.");
        }

        if (genrePreferences.Contains(model.Genre))
        {
            throw new InvalidOperationException("The genre already exists in your preferences.");
        }

        if (directorPreferences.Contains(model.Director))
        {
            throw new InvalidOperationException("The director already exists in your preferences.");
        }

        actorPreferences.Add(model.Actor);
        genrePreferences.Add(model.Genre);
        directorPreferences.Add(model.Director);

        existingPreferences = existingPreferences ?? new UserPreferences
        {
            UserId = userId
        };

        existingPreferences.FavoriteActors = JsonConvert.SerializeObject(actorPreferences);
        existingPreferences.FavoriteGenres = JsonConvert.SerializeObject(genrePreferences);
        existingPreferences.FavoriteDirectors = JsonConvert.SerializeObject(directorPreferences);

        if (existingPreferences.Id == 0)
        {
            _dbContext.UserPreferences.Add(existingPreferences);
        }

        await _dbContext.SaveChangesAsync();

        return Ok();
    }


    [HttpPost("remove-preferences")]
    public async Task<IActionResult> RemovePreference([FromBody] AddUserPreference model)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("You must be authenticated to manage preferences.");
        }

        var existingPreferences = await _dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (existingPreferences == null)
        {
            return BadRequest("No preferences found for the user.");
        }

        List<string> actorPreferences = JsonConvert.DeserializeObject<List<string>>(existingPreferences.FavoriteActors);

        if (!actorPreferences.Contains(model.Actor))
        {
            throw new InvalidOperationException("This actor doesn't exist in your preferences.");
        }

        actorPreferences.Remove(model.Actor);

        existingPreferences.FavoriteActors = JsonConvert.SerializeObject(actorPreferences);

        await _dbContext.SaveChangesAsync();

        return Ok();
    }


    [HttpGet("recommended")]
    public async Task<IActionResult> GetRecommendedMovies()
    {
        var userId = User.Identity.Name;
        var movies = _dbContext.Movies
            .Include(m => m.Director)
            .Include(m => m.Genres)
            .Include(m => m.Actors)
            .ToList();

        var userPreferences = await _dbContext.UserPreferences
            .FirstOrDefaultAsync(p => p.UserId == userId);

        var query = _dbContext.Movies.AsQueryable();

        if (userPreferences != null)
        {
            double actorWeight = 0.6;
            double genreWeight = 0.3;
            double directorWeight = 0.1;

            query = query.OrderBy(m =>
                (actorWeight * m.Actors.Count(actor => userPreferences.FavoriteActors.Contains(actor.Name))) +
                (genreWeight * m.Genres.Count(genre => userPreferences.FavoriteGenres.Contains(genre.Name))) +
                (directorWeight * (userPreferences.FavoriteDirectors.Contains(m.Director.Name) ? 1 : 0))
            );
        }

        var recommendedMovies = await query.ToListAsync();
        return Ok(recommendedMovies);
    }

}
