using ProjectB.Database;
using ProjectB.Models;
using ProjectB.DataAccess;
namespace ProjectB;

class Program
{
    static void Main()
    {
        DbFactory.InitializeDatabase();
        Menu.RunMenu();
    }
}
