using ProjectB.Database;
using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class Cinema
{
    // private readonly BusinessLogic.CinemaService _service;
    private Dictionary<string, string> MenuOptions;
    private Menu SelectMenu;
    
    string Logo = @"
 ____             __               ____                                              __             
/\  _`\          /\ \__           /\  _`\    __                                     /\ \            
\ \ \L\ \  __  __\ \ ,_\    __    \ \ \/\_\ /\_\    ___      __    ___ ___      __  \ \/      ____  
 \ \  _ <'/\ \/\ \\ \ \/  /'__`\   \ \ \/_/_\/\ \ /' _ `\  /'__`\/' __` __`\  /'__`\ \/      /',__\ 
  \ \ \L\ \ \ \_\ \\ \ \_/\  __/    \ \ \L\ \\ \ \/\ \/\ \/\  __//\ \/\ \/\ \/\ \L\.\_      /\__, `\
   \ \____/\/`____ \\ \__\ \____\    \ \____/ \ \_\ \_\ \_\ \____\ \_\ \_\ \_\ \__/.\_\     \/\____/
    \/___/  `/___/> \\/__/\/____/     \/___/   \/_/\/_/\/_/\/____/\/_/\/_/\/_/\/__/\/_/      \/___/ 
               /\___/                                                                               
               \/__/                                                                                       

Welcome customer!
Use Up & Down keys to select an option.
                ";
    
    public Cinema()
    {
        // _service = new BusinessLogic.CinemaService();
        DbFactory.InitializeDatabase();
        MenuOptions = new()
        {
            { "UP", "Upcoming Movies" },
            { "AU", "About us" },
            { "LI", "Login" },
            { "RE", "Register" },
            { "EX", "Exit" },
            { "CM", "Create Movie (admins)" },
        };
        
        SelectMenu = new Menu(Logo, MenuOptions);
    }

    public void Run()
    {
        string selectedOption = SelectMenu.Run();
        
        // Code block for keyPressed cases
        Console.Clear();
        switch (selectedOption)
        {
            case "RE":
                UserCreation.CreateUser();
                break;
            case "LI":
                ConsoleMethods.Error("Not implemented");
                break;
            case "UP":
                MovieList movieList = new MovieList();
                movieList.Run();
                break;
            case "AU":
                AboutUs aboutUs = new AboutUs();
                aboutUs.Run();
                break;
            case "EX":
                return;
            case "CM":
                CreateMovie.Create();
                break;
        }
    }
}