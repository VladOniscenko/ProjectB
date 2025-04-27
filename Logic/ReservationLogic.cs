using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;
using ProjectB.Presentation;

namespace ProjectB.Logic
{
    public class ReservationLogic : IReservationService
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly ISeatService _seatService;
        private readonly ISeatReservationService _seatReservationService;

        public ReservationLogic(ReservationRepository reservationRepository, ISeatService seatService, ISeatReservationService seatReservationService)
        {
            _reservationRepository = reservationRepository;
            _seatService = seatService;
            _seatReservationService = seatReservationService;
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

        public int Create(Reservation reservation)
        {
            return _reservationRepository.AddReservation(reservation);
        }

        public ReservationError? CreateReservation(int showtimeId, IEnumerable<Seat> seats, string paymentMethod, int userId)
        {
            if (showtimeId <= 0)
                return new ReservationError("INVALID_SHOWTIME_ID", "Invalid showtime ID.");

            if (seats == null || !seats.Any())
                return new ReservationError("INVALID_SEAT_SELECTION", "At least one seat must be selected.");

            if (string.IsNullOrWhiteSpace(paymentMethod))
                return new ReservationError("INVALID_PAYMENT_METHOD", "Payment method is required.");

            if (!GetPaymentMethods().ContainsKey(paymentMethod))
                return new ReservationError("INVALID_PAYMENT_METHOD", "Invalid payment method.");

            if (userId <= 0)
                return new ReservationError("INVALID_USER_ID", "Invalid user ID.");

            var reservation = new Reservation
            {
                ShowtimeId = showtimeId,
                UserId = userId,
                CreationDate = DateTime.UtcNow,
                Status = "Pending",
                TotalPrice = _seatService.GetTotalPrice(seats),
                PaymentMethod = paymentMethod
            };

            int reservationId = Create(reservation);

            foreach (Seat seat in seats)
            {
                
                if (_seatReservationService.IsSeatTaken(showtimeId, seat.Id))
                {
                    Delete(reservationId);
                    return new ReservationError("SEAT_ALREADY_TAKEN", $"Seat {seat.Id} is already reserved for the selected showtime.");
                }
                
                var seatReservation = new SeatReservation
                {
                    SeatId = seat.Id,
                    ReservationId = reservationId,
                    ShowtimeId = showtimeId,
                    TicketType = seat.TicketType ?? "adult"
                };

                _seatReservationService.Create(seatReservation);
            }

            return new ReservationError("SUCCESS", $"Reservation created successfully. ID: {reservationId}");
        }

        private void Delete(int reservationId)
        {
            _reservationRepository.Delete(reservationId);
        }
    }

    public class ReservationError
    {
        public string ErrorCode { get; }
        public string Message { get; }

        public ReservationError(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}

