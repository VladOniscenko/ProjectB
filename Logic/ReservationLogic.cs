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

    public Dictionary<string, string> GetPaymentMethods()
    {
        return new Dictionary<string, string>()
        {
            { "ideal", "iDEAL" },
            { "paypal", "PayPal" },
            { "creditcard", "Credit Card (Visa, Mastercard, Amex)" },
            { "applepay", "Apple Pay" },
            { "googlepay", "Google Pay" },
            { "klarna", "Klarna (Achteraf betalen)" },
            { "bancontact", "Bancontact" },
            { "sepa", "SEPA Bank Transfer" },
            { "sofort", "Sofort (vaak gebruikt door Nederlanders met Duitse rekeningen)" },
            { "tikkie", "Tikkie (voor informele betalingen)" },
            { "bitcoin", "Bitcoin (via BitPay en consorten)" }
        };

    }
    
    
    
    
    
    
    
}