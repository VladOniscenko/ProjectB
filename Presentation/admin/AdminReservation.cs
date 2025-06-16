using Microsoft.Extensions.DependencyInjection;
using ProjectB;
using ProjectB.Logic;
using ProjectB.Models;
using ProjectB.Presentation;
public class AdminReservation
{
    bool Running;
    private readonly IServiceProvider _services;
    private readonly ReservationLogic _reservationService;
    private readonly UserLogic _userService;

    public AdminReservation()
    {
        _services = Program.Services;
        _reservationService = _services.GetRequiredService<ReservationLogic>();
        _userService = _services.GetRequiredService<UserLogic>();
        Running = false;
    }

    public void Run()
    {
        User currentUser = Program.CurrentUser;
        IEnumerable<Reservation> reservations = _reservationService.GetAllReservation();
        List<Reservation> activeReservations = reservations.Where(r => r.Status == "Confirmed").ToList();
        List<Reservation> inactiveReservations = reservations.Where(r => r.Status == "Cancelled").ToList();

        if (reservations.Count() == 0)
        {
            ConsoleMethods.Error("No reservations made");
            return;
        }
        Running = true;
        while (Running)
        {

            int page = 0;
            int totalPages = (int)Math.Ceiling((double)activeReservations.Count() / 5);


            var reservationDictionary = activeReservations.ToDictionary(
                r => r.Id.ToString(),
                r => ShowInfo(r, true)
            );

            var inactiveReservationDictionary = inactiveReservations.ToDictionary(
                r => r.Id.ToString(),
                r => ShowInfo(r, false)
            );

            foreach (var keyValue in inactiveReservationDictionary)
            {
                if (!reservationDictionary.ContainsKey(keyValue.Key))
                {
                    reservationDictionary[keyValue.Key] = keyValue.Value;
                }
            }

            reservationDictionary.Add("M", "Back to menu");


            Console.Clear();

            if (page < totalPages - 1)
            {
                reservationDictionary.Add("N", "Next Page");
            }

            if (page > 0)
            {
                reservationDictionary.Add("P", "Previous Page");
            }

            Console.ResetColor();
            Console.Clear();

            var selectedOption = Menu.SelectMenu($"Reservations|| Total reservations:{reservations.Count()} || Total active reservations:{activeReservations.Count()} || [ Page {page + 1}/{totalPages} ]  ", reservationDictionary);

            ConsoleMethods.AnimateLoadingText("Processing data");

            switch (selectedOption)
            {
                case "N":
                    page++;
                    break;
                case "P":
                    page--;
                    break;
                case "S":
                    return;
                case "M":
                    Running = false;
                    break;
                default:
                    Reservation? reservation = reservations.FirstOrDefault(r => r.Id == int.Parse(selectedOption));
                    ViewReservation viewReservation = new();
                    viewReservation.ShowReservationInformation(reservation);
                    Running = false;
                    continue;
            }

        }
    }

    private string ShowInfo(Reservation reservation, bool confirmed)
    {
        User? user = _userService.GetUserByReservationId(reservation);
        var ReservationShowtime = _reservationService.GetShowtimeByShowtimeId(reservation);
        var ReservationMovie = _reservationService.GetMovieByShowtimeId(reservation);
        // var ReservationSeats = _reservationService.GetSeatIdByReservationId(reservation);
        return $"Reservation ID: {reservation.Id} || Movie name: {ReservationMovie.Title} || Starting time: {ReservationShowtime.StartTime:yyyy-MM-dd HH:mm} || Firstname: {user.FirstName} || Status: {reservation.Status}";
    }
}