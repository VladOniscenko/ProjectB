using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class ReservationFlow
{
    private ReservationLogic Reservation;
    private Showtime SelectedShowtime;
    
    private readonly IServiceProvider _services;
    
    private readonly Movie _movie;
    
    public ReservationFlow(IServiceProvider services, Movie movie)
    {
        _services = services;
        _movie = movie;
    }
    

    public void Run()
    {
        // 1. select showtime
        Showtime? showtime = GetShowtime();
        if (showtime == null)
        {
            return;
        }
        
        // 2. select seats
        SeatSelection? seatSelection = new SeatSelection(_services, _movie, showtime);
        IEnumerable<Seat>? seats = seatSelection.Run();
        if (seats == null || seats.Count() == 0)
        {
            return;
        }
        
        Console.WriteLine("Selected seats:");
        foreach (var seat in seats)
        {
            Console.WriteLine($"Row: {seat.Row}, Seat: {seat.Number}");
        }
        Console.ReadKey();

        // 3. select tickets type (childrent, adults, seniors)

        // 4. logged in? go to step 6

        // 5. Ask to login or to register

        // 6. confirmation (total price seats etc.)

        // 7. pay
    }

    public Showtime? GetShowtime()
    {
        // 1. select showtime
        SelectShowtime selectShowtime = new SelectShowtime(_services, _movie);
        return selectShowtime.Run();
    }
    
}