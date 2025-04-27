using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IReservationService
{
    public Dictionary<string, string> GetPaymentMethods();

    public ReservationError? CreateReservation(int showtimeId, IEnumerable<Seat> seats, string paymentMethod, int userId);
    // void CancelReservation();

    bool UpdateReservation(Reservation reservation);
}