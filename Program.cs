using System.Text;
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
    public static ServiceProvider Services { get; private set; }
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
        Console.OutputEncoding = Encoding.UTF8;
        
        while (true)
        {
            Console.Clear();
            switch (GetMainMenuSelection())
            {
                case "RE":
                    var userCreation = new UserCreation();
                    userCreation.CreateUser();
                    break;
                case "SM":
                    SearchMovie searchMovie = new SearchMovie();
                    searchMovie.SearchForMovies();
                    break;
                case "UM":
                    MovieList movieList = new MovieList();
                    movieList.Run();
                    break;
                case "CM":
                    var createMovieFlow = new CreateMovieFlow();
                    createMovieFlow.Run();
                    break;
                case "AU":
                    new AboutUs().Run();
                    break;
                case "VP":
                    var viewProfile = new ViewReservation();
                    viewProfile.Run();
                    break;
                case "EX":
                    return;
                case "LO":
                    Logout();
                    break;
                case "LI":
                    var userLogin = new UserLogin();
                    userLogin.Run();
                    break;
                case "CS":
                    var createShowtime = new CreateShowtime();
                    createShowtime.Run();
                    break;
                case "AA":
                    var makeAccountAdmin = new MakeAccountAdmin();
                    makeAccountAdmin.ChooseAccount();
                    break;
                case "UP":
                    var userProfile = new UserProfile();
                    userProfile.Run();
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

        // create services collection for dependency injection
        var services = new ServiceCollection();

        // add repositories (data access layers)
        // singleton makes sure that only one instance of any class exists
        services.AddSingleton<UserRepository>();
        services.AddSingleton<MovieRepository>();
        services.AddSingleton<ShowtimeRepository>();
        services.AddSingleton<SeatRepository>();
        services.AddSingleton<ReservationRepository>();
        services.AddSingleton<AuditoriumRepository>();
        services.AddSingleton<SeatReservationRepository>();
        services.AddSingleton<SearchMovieLogic>();
        services.AddSingleton<UserLogic>();
        services.AddSingleton<MovieLogic>();
        services.AddSingleton<ShowtimeLogic>();
        services.AddSingleton<SeatLogic>();
        services.AddSingleton<ReservationLogic>();
        services.AddSingleton<AuditoriumLogic>();
        services.AddSingleton<SeatReservationLogic>();
        services.AddSingleton<SearchMovieLogic>();

        // initializes all classes only once
        Services = services.BuildServiceProvider();
    }

    private static string GetMainMenuSelection()
    {
        Dictionary<string, string> menuOptions = new()
        {
            { "UM", "Upcoming Movies" },
            { "SM", "Search Movie"},
            { "AU", "About us" },
        };

        if (CurrentUser != null)
        {
            menuOptions.Add("LO", "Log out");
            menuOptions.Add("VP", "View profile");
        
            menuOptions.Add("UP", "Profile");
            
            if (CurrentUser.IsAdmin)
            {
                menuOptions.Add("CM", "Create Movie");
                menuOptions.Add("CS", "Create Showtime");
                menuOptions.Add("AA", "Give Admin Access");
            }
        }
        else
        {
            menuOptions.Add("RE", "Register");
            menuOptions.Add("LI", "Log in");
        }

        menuOptions.Add("EX", "Exit");
        
        var customerName = CurrentUser is not null ? $"back {CurrentUser.FirstName}" : "";
        List<Movie> Promotedmovie = new MovieLogic(new MovieRepository()).GetPromotedMovies();

        var heading = $"""
                       {Logo}
                       
                       This Week’s Top 3 Movies
                       1: {Promotedmovie[0]}
                       2: {Promotedmovie[1]}
                       3: {Promotedmovie[2]}
                       
                       Welcome {customerName}to the Byte Cinema!
                       Use Up & Down keys to select an option.
                       
                       """;

        var selectMenu = new Menu(heading, menuOptions);

        return selectMenu.Run();
    }

    public static void StartReservation(Movie movie)
    {
        var reservationFlow = new ReservationFlow(movie);
        reservationFlow.Run();
    }

    public static void Logout()
    {
        CurrentUser = null;
        ConsoleMethods.AnimateLoadingText("Logging out");
        ConsoleMethods.Success("You have been logged out.");
    }
}