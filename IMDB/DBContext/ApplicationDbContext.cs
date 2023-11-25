using IMDB.AuthorizationFields;
using IMDB.FilmElements;
using IMDB.UserModel;
using Microsoft.EntityFrameworkCore;

namespace IMDB.DBContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<MovieActionsDB> UserArea{get; set;}
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }          
        public DbSet<UserBan> UserBans { get; set; }     
        public DbSet<Userr> Userss  => Set<Userr>();

        public string Id { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSqlLocalDb;Database=IMDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }     
    }
}
