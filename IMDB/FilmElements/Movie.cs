using IMDB.UserModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace IMDB.FilmElements
{
    public class Movie 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DirectorId { get; set; }
        public Director Director { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Actor> Actors { get; set; }
    }
}
