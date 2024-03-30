using SimpleMoviesService.Models;

namespace SimpleMoviesService.Endpoints;

public static class EndpointMapping
{
    public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("movies", GetMovies.Handle)
            .WithSummary("List movies")
            .WithDescription("Get a paginated list of movies")
            .WithTags("Movies")
            .Produces(200, typeof(PagedList<GetMovies.MovieVm>))
            .WithOpenApi();

        app.MapGet("genres", GetGenres.Handle)
            .WithSummary("List genres")
            .WithDescription("Get a list of genres")
            .WithTags("Genres")
            .Produces(200, typeof(List<GetGenres.GenreVm>))
            .WithOpenApi();

        return app;
    }
}
