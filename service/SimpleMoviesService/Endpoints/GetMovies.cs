using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMoviesService.Models;
using SimpleMoviesService.Persistance;
using SimpleMoviesService.Utils;

namespace SimpleMoviesService.Endpoints;

public static class GetMovies
{
    public static async Task<IResult> Handle([FromServices] IMoviesDbContext dbContext,
        string? search = null, int page = 1, int pageSize = 20, int? genreId = null,
        string? sort = null, bool descending = false, CancellationToken cancellationToken = default)
    {
        if (page < 1) return Results.BadRequest(ProblemDetailsBuilder.BadRequest("Page must be greater than 1"));
        var moviesQuery = dbContext.Movies.AsQueryable();

        if (search is not null)
        {
            moviesQuery = moviesQuery.Where(movie => movie.Title.Contains(search));
        }

        if (genreId is not null)
        {
            moviesQuery = moviesQuery.Where(movie => movie.Genres.Any(movieGenre => movieGenre.Id == genreId));
        }

        if (sort is not null)
        {
            var moviePropertyNames = typeof(Movie).GetProperties().Select(property => property.Name);
            var sortPropertyName = moviePropertyNames.FirstOrDefault(propertyName => propertyName.Equals(sort, StringComparison.OrdinalIgnoreCase));

            if (sortPropertyName is null) return Results.BadRequest(ProblemDetailsBuilder.BadRequest($"{sort} is not a valid sort field"));

            moviesQuery = descending
                ? moviesQuery.OrderByDescending(movie => EF.Property<object>(movie, sortPropertyName))
                : moviesQuery.OrderBy(movie => EF.Property<object>(movie, sortPropertyName));
        }

        var pagedMovies = await moviesQuery
            .Include(movie => movie.Genres)
            .ToPagedListAsync(page, pageSize, cancellationToken);

        var response = new PagedList<MovieVm>()
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = pagedMovies.TotalItems,
            TotalPages = pagedMovies.TotalPages,
            Items = pagedMovies.Items
                .Select(movie => MovieVm.FromMovie(movie))
                .ToList()
        };

        return Results.Ok(response);
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
}
