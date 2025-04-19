using ProjectB.Database;
using ProjectB.Models;
using ProjectB.DataAccess;
using ProjectB.Presentation;
namespace ProjectB;

class Program
{
    static void Main()
    {
        
        DbFactory.InitializeDatabase();
        
        var movieRepo = new MovieRepository();
        MovieList movieList = new MovieList(movieRepo);

        Menu.RunMenu();
        
    }
}
