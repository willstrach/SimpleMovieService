using Microsoft.EntityFrameworkCore;
using SimpleMoviesService.Models;

namespace SimpleMoviesService.Persistance;

public class MoviesDbContext(DbContextOptions<MoviesDbContext> options) : DbContext(options), IMoviesDbContext
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
}
