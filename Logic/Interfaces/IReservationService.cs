using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IReservationService
{
    public Dictionary<string, string> GetPaymentMethods();
    // bool SelectShowtime(Showtime? showtime);
    // void AddOrRemoveSeat(Seat seat);
    // bool ConfirmReservation();
    // void CancelReservation();
}