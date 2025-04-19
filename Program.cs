using ProjectB;
using ProjectB.DataAccess;
using ProjectB.Database;
using ProjectB.Logic;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;
using ProjectB.Presentation;

class Program
{
    private static IUserService _userService;
    private static IMovieService _movieService;
    private static IShowtimeService _showtimeService;
    private static ISeatService _seatService;
    private static IReservationService _reservationService;

    public static User? CurrentUser = null;

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

Welcome customer!
Use Up & Down keys to select an option.
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
                    var userCreation = new UserCreation(_userService);
                    userCreation.CreateUser();
                    break;
                case "RF":
                    var reservationFlow = new ReservationFlow(_movieService, _showtimeService, _seatService, _reservationService);
                    reservationFlow.Run();
                    break;
                case "CM":
                    var createMovieFlow = new CreateMovieFlow(_movieService);
                    createMovieFlow.Run();
                    break;
                case "AU":
                    new AboutUs().Run();
                    break;
                case "EX":
                    return;
                default:
                    ConsoleMethods.Error("Invalid option.");
                    break;
            }
        }
    }

    private static void InitializeServices()
    {
        DbFactory.InitializeDatabase();

        var userRepository = new UserRepository();
        var movieRepository = new MovieRepository();
        var showtimeRepository = new ShowtimeRepository();
        var seatRepository = new SeatRepository();
        var reservationRepository = new ReservationRepository();

        _userService = new UserLogic(userRepository);
        _movieService = new MovieLogic(movieRepository);
        _showtimeService = new ShowtimeLogic(showtimeRepository);
        _seatService = new SeatLogic(seatRepository);
        _reservationService = new ReservationLogic(reservationRepository);
    }

    private static string GetMainMenuSelection()
    {
        Dictionary<string, string> menuOptions = new()
        {
            { "RF", "Upcoming Movies" },
            { "AU", "About us" },
            { "LI", "Login" },
            { "RE", "Register" },
            { "EX", "Exit" },
        };

        if (CurrentUser != null)
        {
            menuOptions.Add("LO", "Log out");

            if (CurrentUser.IsAdmin)
            {
                menuOptions.Add("CM", "Create Movie");
            }
        }
        
        Menu selectMenu = new Menu(Logo, menuOptions);
        return selectMenu.Run();
    }
}