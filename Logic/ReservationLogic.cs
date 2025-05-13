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
        throw new NotImplementedException();
    }
}