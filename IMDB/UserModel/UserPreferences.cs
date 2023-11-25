using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMDB.UserModel
{
    public class UserPreferences
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? FavoriteActors { get; set; } 
        public string? FavoriteGenres { get; set; }
        public  string? FavoriteDirectors { get; set; }
    }
}
