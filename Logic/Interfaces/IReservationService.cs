using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IReservationService
{
    public ReservationError? CreateReservation(int showtimeId, List<Seat> seats, string paymentMethod, int userId);
    IEnumerable<Reservation> GetReservationByUserID(User user);

    Showtime GetShowtimeByShowtimeId(Reservation reservation);
    Movie GetMovieByShowtimeId(Reservation reservation);
    List<int> GetSeatIdByReservationId(Reservation reservation);
    List<Tuple<int, int>> GetSeatsFromSeatReservation(List<int> seatReservationId);
    bool UpdateReservation(Reservation reservation);
    string GetAuditoriumInfoByReservationId(Showtime showtime);
    void Cancel(int Id);

    public IEnumerable<Reservation> GetReservationsById(int userId);
    string GetReservationInfo(Reservation reservation);
}