
using Microsoft.Extensions.DependencyInjection;
using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class ViewReservation
{

    bool Running;
    private readonly IServiceProvider _services;
    private readonly ReservationLogic _reservationService;

    public ViewReservation()
    {
        _services = Program.Services;
        _reservationService = _services.GetRequiredService<ReservationLogic>();
        Running = false;
    }

    public void Run()
    {
        User CurrentUser = Program.CurrentUser;
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
                r => $"Reservation ID: {r.Id} || Movie name: {_reservationService.GetMovieByShowtimeId(r).Title} || Start time movie: {_reservationService.GetShowtimeByShowtimeId(r).StartTime:yyyy-MM-dd HH:mm} || Price: €{r.TotalPrice}"

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
                    continue;

            }

        }
    }

    public void ShowReservationInformation(Reservation reservation)
    {
        var ReservationShowtime = _reservationService.GetShowtimeByShowtimeId(reservation);
        var ReservationMovie = _reservationService.GetMovieByShowtimeId(reservation);
        var ReservationSeats = _reservationService.GetSeatIdByReservationId(reservation);
        string SeatInfo = ShowSeats(_reservationService.GetSeatsFromSeatReservation(ReservationSeats));
        string AuditoriumInfo = _reservationService.GetAuditoriumInfoByReservationId(ReservationShowtime);

        Console.Clear();
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine("");
        Console.WriteLine($"Title: {ReservationMovie.Title}");
        Console.WriteLine($"Startingtime: {ReservationShowtime.StartTime:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"Ending: {ReservationShowtime.EndTime:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"Reservated on: {reservation.CreationDate:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"{AuditoriumInfo}");
        Console.WriteLine($"Reservation ID: {reservation.Id}");
        Console.WriteLine($"Status: {reservation.Status}");
        if (reservation.Status == "Confirmed")
        {
            Console.WriteLine($"Reserved seats:\n{SeatInfo}");
        }
        Console.WriteLine("");
        ShowPurchaseMenu(reservation, ReservationShowtime);

    }

    private string ShowSeats(List<Tuple<int, int>> seatInfo)
    {
        string result = "";
        foreach (Tuple<int, int> seat in seatInfo)
        {
            result += $"Row:{seat.Item1} Number:{seat.Item2} |\n";
        }

        return result;
    }

    private void ShowPurchaseMenu(Reservation reservation, Showtime showtime)
    {
        int startingRow = Console.CursorTop + 2;
        List<string> options = new() { "Back to reservation list" };

        if (reservation.Status == "Confirmed" && showtime.StartTime >= DateTime.Now)
        {
            options.Add("Cancel reservation");
        }

        if (Menu.AddMenuFromStartRow("What would you like to", options, startingRow) == 1 && reservation.Status == "Confirmed" && options.Count > 1) {
            Running = false;
            canceling(reservation);
        }

        Console.Clear();
    }

    private void canceling(Reservation reservation)
    {
        _reservationService.Cancel(reservation.Id);
        ConsoleMethods.AnimateLoadingText("Canceling reservation");
        ConsoleMethods.Success("Succesfully canceled movie");
    }
}