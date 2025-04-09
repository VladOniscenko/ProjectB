using ProjectB.Database;
using ProjectB.Models.Movies;
using ProjectB.DataAccess;
namespace ProjectB;

class Program
{
    static void Main()
    {   
        Menu.RunMenu();

        DbFactory.InitializeDatabase();
        
        var movieRepo = new MovieRepository();

        Menu.Create(movieRepo);
        
    }
    
    
}
