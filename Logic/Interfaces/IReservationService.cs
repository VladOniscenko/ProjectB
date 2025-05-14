using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IReservationService
{
    public ReservationError? CreateReservation(int showtimeId, List<Seat> seats, string paymentMethod, int userId);
    IEnumerable<Reservation> GetReservationByUserID(User user);
    bool UpdateReservation(Reservation reservation);
    void Cancel(int Id);
}