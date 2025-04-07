namespace ProjectB.Models.Movies;

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

    public override string ToString()
    {
        // return $"{Id}: {Title} ({ReleaseDate.Year}) - {Genre} - {Ra        
        return $"{Title} ({ReleaseDate.Year})";

    }
}