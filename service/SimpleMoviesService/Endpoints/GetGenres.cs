using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMoviesService.Persistance;

namespace SimpleMoviesService.Endpoints;

public static class GetGenres
{
    public static async Task<IResult> Handle([FromServices] IMoviesDbContext dbContext)
    {
        var genres = await dbContext.Genres
            .OrderBy(genre => genre.Name)
            .Select(genre => new GenreVm
            {
                Id = genre.Id,
                Name = genre.Name
            })
            .ToListAsync();

        return Results.Ok(genres);
    }

    public class GenreVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
