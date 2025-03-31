using ProjectB.Database;
using ProjectB.Movies;
namespace ProjectB;

class Program
{
    static void Main()
    {

        DbFactory.InitializeDatabase();
        
        var movieRepo = new MovieRepository();
        
        // movieRepo.AddMovie(new Movie 
        // { 
        //     Title = "Inception", 
        //     Description = "A thief who enters the dreams of others to steal secrets.",
        //     Runtime = 148,
        //     Actors = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page",
        //     Rating = 8.8,
        //     Genre = "Sci-Fi",
        //     AgeRestriction = "PG-13",
        //     ReleaseDate = new DateTime(2010, 7, 16),
        //     Country = "USA"
        // });
        
        foreach (Movie movie in movieRepo.GetAllMovies())
        {
            Console.WriteLine($"{movie}");
        }
    }
}