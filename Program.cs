using ProjectB.Database;
using ProjectB.Models;
namespace ProjectB;

class Program
{
    static void Main()
    {
        DbFactory.InitializeDatabase();
        Menu.RunMenu();
    }
}