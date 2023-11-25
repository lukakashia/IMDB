using System.Security.Cryptography;

namespace IMDB.FilmElements
{
    public class MovieActionsDB
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public bool IsWatched { get; set; }
        public bool IsCurrentlyWatching { get; set; }
        public bool WantToWatch { get; set; }
    }
}
