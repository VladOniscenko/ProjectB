using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IReservationService
{
    // bool SelectShowtime(Showtime? showtime);
    // void AddOrRemoveSeat(Seat seat);
    // bool ConfirmReservation();
    // void CancelReservation();

    public IEnumerable<Reservation> GetReservationsById(int userId);
    string GetReservationInfo(Reservation reservation);
}