using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;

namespace ProjectB.Logic;

public class ReservationLogic : IReservationService
{

    private readonly ReservationRepository _reservationRepository;
    public ReservationLogic(ReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    
    
    
    
    
    
    
}