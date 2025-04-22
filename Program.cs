using Microsoft.Extensions.DependencyInjection;
using ProjectB;
using ProjectB.DataAccess;
using ProjectB.Database;
using ProjectB.Logic;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;
using ProjectB.Presentation;

class Program
{
    private static ServiceProvider Services;
    public static User? CurrentUser { get; set; } = null;

    public static string Logo = @"
 ____             __               ____                                              __             
/\  _`\          /\ \__           /\  _`\    __                                     /\ \            
\ \ \L\ \  __  __\ \ ,_\    __    \ \ \/\_\ /\_\    ___      __    ___ ___      __  \ \/      ____  
 \ \  _ <'/\ \/\ \\ \ \/  /'__`\   \ \ \/_/_\/\ \ /' _ `\  /'__`\/' __` __`\  /'__`\ \/      /',__\ 
  \ \ \L\ \ \ \_\ \\ \ \_/\  __/    \ \ \L\ \\ \ \/\ \/\ \/\  __//\ \/\ \/\ \/\ \L\.\_      /\__, `\
   \ \____/\/`____ \\ \__\ \____\    \ \____/ \ \_\ \_\ \_\ \____\ \_\ \_\ \_\ \__/.\_\     \/\____/
    \/___/  `/___/> \\/__/\/____/     \/___/   \/_/\/_/\/_/\/____/\/_/\/_/\/_/\/__/\/_/      \/___/ 
               /\___/                                                                               
               \/__/                                                                                       
                ";


    static void Main(string[] args)
    {
        InitializeServices();

        while (true)
        {
            Console.Clear();
            switch (GetMainMenuSelection())
            {
                case "RE":
                    var userCreation = new UserCreation(Services);
                    userCreation.CreateUser();
                    break;
                case "UM":
                    MovieList movieList = new MovieList(Services);
                    movieList.Run();
                    break;
                case "CM":
                    var createMovieFlow = new CreateMovieFlow(Services);
                    createMovieFlow.Run();
                    break;
                case "AU":
                    new AboutUs().Run();
                    break;
                case "EX":
                    return;
                case "LO":
                    Logout();
                    break;
                case "LI":
                    var userLogin = new UserLogin(Services);
                    userLogin.Run();
                    break;
                case "CS":
                    var createShowtime = new CreateShowtime(Services);
                    createShowtime.Run();
                    break;
                default:
                    ConsoleMethods.Error("Invalid option.");
                    break;
            }
        }
    }

    private static void InitializeServices()
    {
        DbFactory.InitializeDatabase();

        var services = new ServiceCollection();

        services.AddSingleton<UserRepository>();
        services.AddSingleton<MovieRepository>();
        services.AddSingleton<ShowtimeRepository>();
        services.AddSingleton<SeatRepository>();
        services.AddSingleton<ReservationRepository>();
        services.AddSingleton<AuditoriumRepository>();

        services.AddSingleton<IUserService, UserLogic>();
        services.AddSingleton<IMovieService, MovieLogic>();
        services.AddSingleton<IShowtimeService, ShowtimeLogic>();
        services.AddSingleton<ISeatService, SeatLogic>();
        services.AddSingleton<IReservationService, ReservationLogic>();
        services.AddSingleton<IAuditoriumService, AuditoriumLogic>();

        Services = services.BuildServiceProvider();
    }

    private static string GetMainMenuSelection()
    {
        Dictionary<string, string> menuOptions = new()
        {
            { "UM", "Upcoming Movies" },
            { "AU", "About us" },
        };

        if (CurrentUser != null)
        {
            menuOptions.Add("LO", "Log out");
        
            if (CurrentUser.IsAdmin)
            {
                menuOptions.Add("CM", "Create Movie");
                menuOptions.Add("CS", "Create Showtime");
            }
        }
        else
        {
            menuOptions.Add("RE", "Register");
            menuOptions.Add("LI", "Log in");
        }

        menuOptions.Add("EX", "Exit");
        
        var customerName = CurrentUser is not null ? $"back {CurrentUser.FirstName}" : "";

        var heading = $"""
                       {Logo}
                       Welcome {customerName}to the Byte Cinema!
                       Use Up & Down keys to select an option.
                       
                       """;

        var selectMenu = new Menu(heading, menuOptions);

        return selectMenu.Run();
    }

    public static void StartReservation(Movie movie)
    {
        var reservationFlow = new ReservationFlow(
            Services,
            movie
        );

        reservationFlow.Run();
    }

    public static void Logout()
    {
        CurrentUser = null;
        ConsoleMethods.AnimateLoadingText("Logging out");
        ConsoleMethods.Success("You have been logged out.");
    }
}