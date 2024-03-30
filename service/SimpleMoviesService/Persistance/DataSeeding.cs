    using CsvHelper;
using CsvHelper.Configuration.Attributes;
using SimpleMoviesService.Models;
using System.Globalization;
using System.Reflection;

namespace SimpleMoviesService.Persistance;

public static class DataSeeding
{
    public static void SeedData(this MoviesDbContext context)
    {
        if (context.Movies.Any()) return;

        var executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var seedDataPath = $"{executingDirectory}/Persistance/SeedData.csv";

        var seedData = new List<SeedDataRow>();

        using (var reader = new StreamReader(seedDataPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<SeedDataRow>().ToList();
            seedData.AddRange(records);
        }

        var uniqueGenres = seedData
            .SelectMany(seedDataRow => seedDataRow.Genres.Split(','))
            .Select(genre => genre.Trim())
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        context.SeedGenres(uniqueGenres);
        context.SeedMovies(seedData);

        return;
    }

    private static void SeedMovies(this MoviesDbContext context, List<SeedDataRow> seedData)
    {
        var genres = context.Genres.ToList();
        var movies = seedData.Select(seedDataRow =>
        {
            var movie = new Movie
            {
                ReleaseDate = seedDataRow.ReleaseDate,
                Title = seedDataRow.Title,
                Overview = seedDataRow.Overview,
                Popularity = seedDataRow.Popularity,
                VoteCount = seedDataRow.VoteCount,
                VoteAverage = seedDataRow.VoteAverage,
                OriginalLanguage = seedDataRow.OriginalLanguage,
                PosterUrl = seedDataRow.PosterUrl
            };

            var seedMovieGenres = seedDataRow.Genres
                .Split(",")
                .Select(genre => genre.Trim());

            movie.Genres = context.Genres
                .Where(genre => seedMovieGenres.Contains(genre.Name))
                .ToList();

            return movie;
        });
        context.Movies.AddRange(movies);
        context.SaveChanges();
    }

    private static void SeedGenres(this MoviesDbContext context, IEnumerable<string> genres)
    {
        var existingGenres = context.Genres.ToList();
        var newGenres = genres
            .Where(genre => !existingGenres.Any(existingGenre => existingGenre.Name == genre))
            .Select(genre => new Genre { Name = genre });

        context.Genres.AddRange(newGenres);
        context.SaveChanges();
    }

    private class SeedDataRow
    {
        [Index(0)]
        public DateTime ReleaseDate { get; set; }

        [Index(1)]
        public string Title { get; set; } = string.Empty;

        [Index(2)]
        public string Overview { get; set; } = string.Empty;

        [Index(3)]
        public decimal Popularity { get; set; }

        [Index(4)]
        public int VoteCount { get; set; }

        [Index(5)]
        public decimal VoteAverage { get; set; }

        [Index(6)]
        public string OriginalLanguage { get; set; } = string.Empty;

        [Index(7)]
        public string Genres { get; set; } = string.Empty;

        [Index(8)]
        public string PosterUrl { get; set; } = string.Empty;

    }
}
