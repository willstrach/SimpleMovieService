using Microsoft.EntityFrameworkCore;
using SimpleMoviesService.Models;

namespace SimpleMoviesService.Persistance;

public interface IMoviesDbContext
{
    DbSet<Movie> Movies { get; set; }
    DbSet<Genre> Genres { get; set; }
}
