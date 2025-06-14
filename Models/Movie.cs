namespace ProjectB.Models;

public class Movie : BaseModel
{
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
        return $"{Title} ({ReleaseDate.Year})";
    }
    
    public string RatingStars()
    {
        return $"[{new string('*', Convert.ToInt32(Rating) / 2).PadRight(5)}]";
    }
}