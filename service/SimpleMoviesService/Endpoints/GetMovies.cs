using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMoviesService.Models;
using SimpleMoviesService.Persistance;
using SimpleMoviesService.Utils;

namespace SimpleMoviesService.Endpoints;

public class GetMoviesEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("movies", GetMovies)
            .WithSummary("List movies")
            .WithDescription("Get a paginated list of movies")
            .WithTags("Movies")
            .Produces(200, typeof(PagedList<MovieVm>))
            .WithOpenApi();
    }

    public static async Task<IResult> GetMovies([FromServices] IMoviesDbContext context,
        [AsParameters] GetMoviesQuery query, CancellationToken cancellationToken)
    {
        var queryValidator = new GetMoviesQueryValidator();
        var validationResult = await queryValidator.ValidateAsync(query);

        if (validationResult is not null && !validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(error => $"{error.PropertyName}: {error.ErrorMessage}")
            .ToArray();

            return Results.BadRequest(ProblemDetailsBuilder.BadRequest(errors));
        }

        var moviesQuery = context.Movies
            .AsNoTracking()
            .Where(movie => query.Search == null || movie.Title.Contains(query.Search))
            .Where(movie => query.GenreId == null || movie.Genres.Any(genre => genre.Id == query.GenreId));


        if (query.Sort is not null)
        {
            var moviePropertyNames = typeof(Movie).GetProperties().Select(property => property.Name);
            var sortPropertyName = moviePropertyNames.First(propertyName => propertyName.Equals(query.Sort, StringComparison.OrdinalIgnoreCase));

            moviesQuery = query.Descending ?? false
                ? moviesQuery.OrderByDescending(movie => EF.Property<object>(movie, sortPropertyName))
                : moviesQuery.OrderBy(movie => EF.Property<object>(movie, sortPropertyName));
        }

        var pagedMovies = await moviesQuery
            .Include(movie => movie.Genres)
            .ToPagedListAsync(query.Page ?? 1, query.PageSize ?? 20, cancellationToken);

        var response = new PagedList<MovieVm>()
        {
            Page = pagedMovies.Page,
            PageSize = pagedMovies.PageSize,
            TotalItems = pagedMovies.TotalItems,
            TotalPages = pagedMovies.TotalPages,
            Items = pagedMovies.Items
                .Select(movie => MovieVm.FromMovie(movie))
                .ToList()
        };

        return Results.Ok(response);
    }
}

public class GetMoviesQueryValidator : AbstractValidator<GetMoviesQuery>
{

    public GetMoviesQueryValidator()
    {
        RuleFor(query => query.Page).GreaterThan(0);
        RuleFor(query => query.PageSize).GreaterThan(0);
        RuleFor(query => query.Sort)
            .Must(BeValidSortField)
            .WithMessage("Invalid sort field");
    }

    private static readonly string[] _sortFields = ["id", "title", "releasedate", "popularity", "votecount", "voteaverage"];
    private bool BeValidSortField(string? sort)
        => sort is null || _sortFields.Contains(sort);
}

public class GetMoviesQuery
{
    public string? Search { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public int? GenreId { get; set; }
    public string? Sort { get; set; }
    public bool? Descending { get; set; }
}

public class MovieVm
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }
    public decimal Popularity { get; set; }
    public int VoteCount { get; set; }
    public decimal VoteAverage { get; set; }
    public string OriginalLanguage { get; set; } = string.Empty;
    public string PosterUrl { get; set; } = string.Empty;
    public List<MovieGenreVm> Genres { get; set; } = [];

    public static MovieVm FromMovie(Movie movie)
    {
        return new MovieVm
        {
            Id = movie.Id,
            Title = movie.Title,
            Overview = movie.Overview,
            ReleaseDate = movie.ReleaseDate,
            Popularity = movie.Popularity,
            VoteCount = movie.VoteCount,
            VoteAverage = movie.VoteAverage,
            OriginalLanguage = movie.OriginalLanguage,
            PosterUrl = movie.PosterUrl,
            Genres = movie.Genres.Select(genre => new MovieGenreVm
            {
                Id = genre.Id,
                Name = genre.Name
            }).ToList()
        };
    }
}
public class MovieGenreVm
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}