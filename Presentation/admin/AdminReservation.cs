using Bogus.DataSets;
using Microsoft.Extensions.DependencyInjection;
using ProjectB;
using ProjectB.DataAccess;
using ProjectB.Logic;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;
using ProjectB.Presentation;
public class AdminReservation
{
    bool Running;
    private readonly IServiceProvider _services;
    private readonly ReservationLogic _reservationService;
    private readonly ShowtimeLogic _showtimeService;

    public AdminReservation()
    {
        _services = Program.Services;
        _reservationService = _services.GetRequiredService<ReservationLogic>();
        _showtimeService = _services.GetRequiredService<ShowtimeLogic>();
        Running = false;
    }

    public void Run()
    {
        User currentUser = Program.CurrentUser;
        IEnumerable<Reservation> reservations = _reservationService.GetAllReservation();
        IEnumerable<Showtime> showtimes = _showtimeService.GetAllShowtimes();

        List<Reservation> activeReservations = reservations.Where(r => r.Status == "Confirmed").ToList();

        if (reservations.Count() == 0)
        {
            ConsoleMethods.Error("No reservations made");
            return;
        }

        int page = 0;
        int totalPages = (int)Math.Ceiling((double)activeReservations.Count() / 5);


        var reservationDictionary = activeReservations.ToDictionary(
            r => r.Id.ToString(),
            r => $"{r.CreationDate}"
        );

        Running = true;
        while (Running)
        {
            Console.Clear();



            if (page < totalPages - 1)
            {
                reservationDictionary.Add("N", "Next Page");
            }

            if (page > 0)
            {
                reservationDictionary.Add("P", "Previous Page");
            }

            reservationDictionary.Add("M", "Back to menu");
            Console.ResetColor();
            Console.Clear();

            var selectedOption = Menu.SelectMenu("All the reservation", reservationDictionary);

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
                    Reservation? reservation = activeReservations.FirstOrDefault(r => r.Id == int.Parse(selectedOption));
                    continue;
            }

        }
    }
}