namespace SimpleMoviesService.Models;

public class Movie
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
    public List<Genre> Genres { get; set; } = [];
}
