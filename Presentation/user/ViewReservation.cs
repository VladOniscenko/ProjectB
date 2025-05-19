
using Microsoft.Extensions.DependencyInjection;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class ViewReservation
{

    bool Running;
    private readonly IServiceProvider _services;
    private readonly IReservationService _reservationService;

    public ViewReservation()
{
    _services = Program.Services;
    _reservationService = _services.GetRequiredService<IReservationService>();
    Running = false;
}

    public void Run(User CurrentUser)
    {
        IEnumerable<Reservation> reservations = _reservationService.GetReservationByUserID(CurrentUser);

        if (reservations.Count() == 0)
        {
            ConsoleMethods.Error("No reservations made");
            return;
        }

        Running = true;
        while (Running == true)
        {
            Console.Clear();
            Console.WriteLine("Reservations");
            Console.WriteLine("===========");

            var reservationsToShow = reservations.ToList();

            var reservationDictionary = reservationsToShow.ToDictionary(
                r => r.Id.ToString(),
                r => $"Date: {r.CreationDate}||ID: {r.ShowtimeId}||Price: {r.TotalPrice}"

            );

            reservationDictionary.Add("M", "Back to menu");
            var selectedOption = Menu.SelectMenu("Your current reservation", reservationDictionary);
            switch (selectedOption)
            {
                case "M":
                Running = false;
                break;

                default:
                    Reservation? reservation = reservations.FirstOrDefault(r => r.Id == int.Parse(selectedOption));

                    ShowReservationInformation(reservation);
                    ShowPurchaseMenu(reservation);
                continue;
                
            }

        }
    }

    public void ShowReservationInformation(Reservation reservation)
    {
        var ReservationShowtime = _reservationService.GetShowtimeByShowtimeId(reservation);
        var ReservationMovie = _reservationService.GetMovieByShowtimeId(reservation);

        Console.Clear();
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine("");
        Console.WriteLine($"Title: {ReservationMovie.Title}");
        Console.WriteLine($"Startingtime: {ReservationShowtime.StartTime}");
        Console.WriteLine($"Ending: {ReservationShowtime.EndTime}");
        Console.WriteLine($"Reservated on: {reservation.CreationDate}");
        Console.WriteLine($"Auditorium: {ReservationShowtime.AuditoriumId}");
        Console.WriteLine($"Reservation ID: {reservation.Id}");
        Console.WriteLine($"Status: {reservation.Status}");
        Console.WriteLine("");
        
    }

    private void ShowPurchaseMenu(Reservation reservation)
    {
        int startingRow = Console.CursorTop + 2;
        List<string> options = new() { "Cancel reservation", "Back to reservation List" };
        int selected = Menu.AddMenuFromStartRow("What would you like to", options, startingRow);

        if (selected == 1)
        {
            Console.Clear();
            return;
        }
        
        Running = false;
        canceling(reservation);

    }

    private void canceling(Reservation reservation)
    {
        _reservationService.Cancel(reservation.Id);
        ConsoleMethods.AnimateLoadingText("Canceling reservatoin");
    }

}