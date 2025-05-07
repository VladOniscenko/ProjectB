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
    
    public void Create(SeatReservation seatReservation)
    {
        _seatReservationRepository.AddSeatReservation(seatReservation);
    }

    public bool IsSeatTaken(int showtimeId, int seatId)
    {
        return _seatReservationRepository.IsSeatTaken(showtimeId, seatId);
    }
}