using Microsoft.Extensions.DependencyInjection;
using ProjectB.Logic;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class TicketSelection
{
    private IServiceProvider _services;
    private SeatLogic _seatService;
    private List<Seat> _seats;
    public TicketSelection(List<Seat> seats)
    {
        _services = Program.Services;
        _seats = seats;
        _seatService = _services.GetRequiredService<SeatLogic>();
    }
    
    public List<Seat>? Run()
    {
        bool adult = false;
        
        foreach (Seat seat in _seats)
        {
            var ticketOptions = _seatService.GetTicketOptionsForSeat(seat);
            ticketOptions.Add("all_adult", "Set all next seats to adult");
            ticketOptions.Add("return", "Previous step");

            string selection = "adult";
            if (!adult)
            {
                selection = Menu.SelectMenu(
                    $"Select ticket type for Seat (Row: {seat.Row}, Seat number: {seat.Number}, {seat.Type})",
                    ticketOptions
                );
                if (selection == "all_adult")
                {
                    selection = "adult";
                    adult = true;
                }
                if (selection == "return") return null;
            }
            
            seat.TicketType = selection;
            ConsoleMethods.AnimateLoadingText("Saving seat type", 500);
        }

        return _seats;
    }

}