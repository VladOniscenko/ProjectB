using Microsoft.Extensions.DependencyInjection;
using ProjectB.Logic;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class TicketSelection
{
    private IServiceProvider _services;
    private ISeatService _seatService;
    private IEnumerable<Seat> _seats;
    public TicketSelection(IServiceProvider serviceProvider, IEnumerable<Seat> seats)
    {
        _services = serviceProvider;
        _seats = seats;
        _seatService = _services.GetRequiredService<ISeatService>();
    }
    
    public IEnumerable<Seat> Run()
    {
        foreach (Seat seat in _seats)
        {
            var ticketOptions = _seatService.GetTicketOptionsForSeat(seat);

            seat.TicketType = Menu.SelectMenu(
                $"Select ticket type for Seat (Row: {seat.Row}, Seat number: {seat.Number}, {seat.Type})",
                ticketOptions
            );

            ConsoleMethods.AnimateLoadingText("Saving seat type", 500);
        }

        return _seats;
    }

}