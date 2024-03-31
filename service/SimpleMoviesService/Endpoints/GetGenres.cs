using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMoviesService.Persistance;

namespace SimpleMoviesService.Endpoints;

public class GetGenresEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("genres", GetGenres)
            .WithSummary("List genres")
            .WithDescription("Get a list of genres")
            .WithTags("Genres")
            .Produces(200, typeof(List<GenreVm>))
            .WithOpenApi();
    }

    public static async Task<IResult> GetGenres([FromServices] IMoviesDbContext dbContext)
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
}

public class GenreVm
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
