using ProjectB.Database;
using ProjectB.Models.Movies;
using ProjectB.DataAccess;
using ProjectB.Presentation;
namespace ProjectB;

class Program
{
    static void Main()
    {
        // test 
        // Console.WriteLine("Main started...");
        
        DbFactory.InitializeDatabase();
        
        var movieRepo = new MovieRepository();
        MovieList movieList = new MovieList(movieRepo);

        // call the method to view movies
        movieList.OpenUserMenu();
        
        foreach (Movie movie in movieRepo.GetBestAndNewestMovies())
        {
            Console.WriteLine($"{movie}");
        }
    }
}