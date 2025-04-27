using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IReservationService
{
    public Dictionary<string, string> GetPaymentMethods();

    public ReservationError? CreateReservation(int showtimeId, IEnumerable<Seat> seats, string paymentMethod, int userId);
    // bool SelectShowtime(Showtime? showtime);
    // void AddOrRemoveSeat(Seat seat);
    // bool ConfirmReservation();
    // void CancelReservation();
}