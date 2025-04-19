using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Logic;

public class SeatReservationLogic : ISeatReservationService
{
    private SeatReservationRepository _seatReservationRepository;

    public SeatReservationLogic(SeatReservationRepository seatReservationRepository)
    {
        _seatReservationRepository = seatReservationRepository;
    }
    
    public IEnumerable<SeatReservation> GetReservedSeatsByShowtimeId(int showtimeId)
    {
        SeatReservationRepository seatReservationRepository = new();
        return seatReservationRepository.GetReservedSeatsByShowtimeId(showtimeId);
    }
}