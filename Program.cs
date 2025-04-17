using ProjectB.Database;
namespace ProjectB;

class Program
{
    static void Main()
    {
        DbFactory.InitializeDatabase();
        Menu.RunMenu();
    }
}