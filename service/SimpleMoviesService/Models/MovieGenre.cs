namespace SimpleMoviesService.Models;

public class MovieGenre
{
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = new();
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = new();
}
