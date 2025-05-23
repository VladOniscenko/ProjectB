
using Microsoft.Extensions.DependencyInjection;
using ProjectB.DataAccess;
using ProjectB.Logic;
using ProjectB.Logic.Interfaces;
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
                r => $"Date: {r.CreationDate.ToShortDateString()}||Reservation ID: {r.Id}||Price: {r.TotalPrice}"

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
                    Showtime ReservationShowtime = _reservationService.GetShowtimeByShowtimeId(reservation);
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
        Console.WriteLine($"Startingtime: {ReservationShowtime.StartTime}");
        Console.WriteLine($"Ending: {ReservationShowtime.EndTime}");
        Console.WriteLine($"Reservated on: {reservation.CreationDate}");
        Console.WriteLine($"{AuditoriumInfo}");
        Console.WriteLine($"Reservation ID: {reservation.Id}");
        Console.WriteLine($"Status: {reservation.Status}");
        if (reservation.Status == "Confirmed")
        {
            Console.WriteLine($"Reservated seats:\n{SeatInfo}");
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

    private void ShowPurchaseMenu(Reservation reservation , Showtime showtime)
    {
        int startingRow = Console.CursorTop + 2;
        List<string> options = new() { "Cancel reservation", "Back to reservation list" };

        if (reservation.Status == "Cancelled" || DateTime.Now > showtime.StartTime)
        {
            options.Remove("Cancel reservation");
        }

        int selected = Menu.AddMenuFromStartRow("What would you like to", options, startingRow);

        if (selected == 0 && reservation.Status != "Cancelled")
        {
            Running = false;
            canceling(reservation);  
        }

        Console.Clear();
        return;
        

    }

    private void canceling(Reservation reservation)
    {
        _reservationService.Cancel(reservation.Id);
        ConsoleMethods.AnimateLoadingText("Canceling reservatoin");
        ConsoleMethods.Success("Succesfully canceled movie");
    }
    


}