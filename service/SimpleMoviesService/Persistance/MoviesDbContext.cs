using Microsoft.EntityFrameworkCore;
using SimpleMoviesService.Models;

namespace SimpleMoviesService.Persistance;

public class MoviesDbContext(DbContextOptions<MoviesDbContext> options) : DbContext(options), IMoviesDbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Movie>()
            .Property(movie => movie.Popularity)
            .HasColumnType("decimal(8, 4)");


        builder.Entity<Movie>()
            .Property(movie => movie.VoteAverage)
            .HasColumnType("decimal(5, 2)");
    }
}
