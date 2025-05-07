using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IReservationService
{
    public ReservationError? CreateReservation(int showtimeId, List<Seat> seats, string paymentMethod, int userId);
    // void CancelReservation();

    bool UpdateReservation(Reservation reservation);
}