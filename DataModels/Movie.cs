namespace CinemaApp.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Runtime { get; set; }
    public string Actors { get; set; }
    public double Rating { get; set; }
    public string Genre { get; set; }
    public int AgeRestriction { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Country { get; set; }

    // this constructor overload is needed for dapper
    public Movie() { }

    public Movie(string title, string description, int runtime, string actors, double rating, string genre, int ageRestriction, DateTime releaseDate, string country)
    {
        Title = title;
        Description = description;
        Runtime = runtime;
        Actors = actors;
        Rating = rating;
        Genre = genre;
        AgeRestriction = ageRestriction;
        ReleaseDate = releaseDate;
        Country = country;
    }
}