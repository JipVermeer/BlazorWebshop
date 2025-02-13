using Microsoft.EntityFrameworkCore;
using Webshop.Models;

namespace Webshop.Data
{
    public class DataContext : DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VideoGame>().HasData(
                new VideoGame { Id = 1, Title = "Cyberpunk 2077", Publisher = "CD Projekt", ReleaseYear = 2020 },
                new VideoGame { Id = 2, Title = "Elden Ring", Publisher = "FromSoftware", ReleaseYear = 2022 },
                new VideoGame { Id = 3, Title = "The Legend of Zelda: Ocarina of Time", Publisher = "Nintendo", ReleaseYear = 1998 }
                );
        }

        public DbSet<VideoGame> VideoGames { get; set; }
    }

}

