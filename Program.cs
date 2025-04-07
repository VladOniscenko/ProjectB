using ProjectB.Database;
using ProjectB.Models.Movies;
using ProjectB.DataAccess;
namespace ProjectB;

class Program
{
    static void Main()
    {
        // test 
        Console.WriteLine("Main started...");
        
        DbFactory.InitializeDatabase();
        
        var movieRepo = new MovieRepository();
        // call the method to view movies
        MovieList.ViewMovies();
        
        foreach (Movie movie in movieRepo.GetBestAndNewestMovies())
        {
            Console.WriteLine($"{movie}");
        }
    }
}