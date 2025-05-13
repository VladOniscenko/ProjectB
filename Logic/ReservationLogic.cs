using System.Text;
using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Logic;

public class ReservationLogic : IReservationService
{
    private readonly ReservationRepository _reservationRepository;
    
    public ReservationLogic(ReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public IEnumerable<Reservation> GetReservationsById(int userId)
    {
        return _reservationRepository.GetReservationsById(userId);
    }

    public string GetReservationInfo(Reservation reservation)
    {
        // Replace this with actual formatting you want
        var sb = new StringBuilder();
        sb.AppendLine($"Movie: {reservation.UserId}");
        sb.AppendLine($"Showtime: {reservation.ShowtimeId}");
        sb.AppendLine($"Created: {reservation.CreationDate}");
        return sb.ToString();
    }
    
}