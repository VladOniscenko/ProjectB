using System.Numerics;
using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Logic;

public class ReservationLogic : IReservationService
{
    private readonly ReservationRepository _reservationRepository;
    private readonly SeatLogic _seatService;
    private readonly SeatReservationLogic _seatReservationService;

    public ReservationLogic(ReservationRepository reservationRepository, SeatLogic seatService,
        SeatReservationLogic seatReservationService)
    {
        _reservationRepository = reservationRepository;
        _seatService = seatService;
        _seatReservationService = seatReservationService;
    }

    public int Create(Reservation reservation)
    {
        return _reservationRepository.AddReservation(reservation);
    }

    public bool UpdateReservation(Reservation reservation)
    {
        return _reservationRepository.UpdateReservation(reservation);
    }

    public static bool IsShowtimeValid(int showtimeId) => showtimeId > 0;
    public static bool IsSeatsValid(List<Seat> seats) => seats != null && seats.Count > 0;
    public static bool IsPaymentMethodValid(string method) => !string.IsNullOrWhiteSpace(method) && method.Length > 0;
    public static bool IsUserValid(int userId) => userId > 0;
    public static bool IsTotalPriceValid(decimal price) => price > 0;

    public ReservationError? CreateReservation(int showtimeId, List<Seat> seats, string paymentMethod, int userId)
    {
        if (!IsShowtimeValid(showtimeId))
            return new ReservationError("INVALID_SHOWTIME_ID", "Invalid showtime ID.");

        if (!IsSeatsValid(seats))
            return new ReservationError("INVALID_SEAT_SELECTION", "At least one seat must be selected.");

        if (!IsPaymentMethodValid(paymentMethod))
            return new ReservationError("INVALID_PAYMENT_METHOD", "Payment method is required.");

        if (!IsUserValid(userId))
            return new ReservationError("INVALID_USER_ID", "Invalid user ID.");

        if (!IsTotalPriceValid(_seatService.GetTotalPrice(seats)))
            return new ReservationError("INVALID_TOTAL_PRICE", "Invalid total price.");

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
        reservation.Id = reservationId;

        foreach (Seat seat in seats)
        {
            if (_seatReservationService.IsSeatTaken(showtimeId, seat.Id))
            {
                Delete(reservationId);
                return new ReservationError("SEAT_ALREADY_TAKEN",
                    $"Seat {seat.Id} is already reserved for the selected showtime.");
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

        reservation.Status = "Confirmed";
        UpdateReservation(reservation);

        return new ReservationError("SUCCESS", $"Reservation created successfully. ID: {reservationId}");
    }

    public void Delete(int reservationId)
    {
        _reservationRepository.Delete(reservationId);
    }

    public void Cancel(int Id)
    {
        _reservationRepository.Cancel(Id);
    }
    public IEnumerable<Reservation> GetReservationByUserID(User user)
    {
        return _reservationRepository.GetReservationsByUserID(user);
    }

    public Showtime GetShowtimeByShowtimeId(Reservation reservation)
    {
        return _reservationRepository.GetShowtimeByShowtimeId(reservation);
    }

    public Movie GetMovieByShowtimeId(Reservation reservation)
    {
        return _reservationRepository.GetMovieByShowtimeId(reservation);
    }

    public List<int> GetSeatIdByReservationId(Reservation reservation)
    {
        return _reservationRepository.GetSeatIdByReservationId(reservation);
    }

    public List<Tuple<int, int>> GetSeatsFromSeatReservation(List<int> seatReservationId)
    {
        return _reservationRepository.GetSeatsFromSeatReservation(seatReservationId);
    }

    public string GetAuditoriumInfoByReservationId(Showtime showtime)
    {
        return _reservationRepository.GetAuditoriumInfoByShowtime(showtime);
    }

    public IEnumerable<Reservation> GetReservationsById(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be a positive integer.", nameof(userId));
        }

        return _reservationRepository.GetReservationsById(userId);
    }

    public string GetReservationInfo(Reservation reservation)
    {
        if (reservation == null)
        {
            throw new ArgumentNullException(nameof(reservation), "Reservation cannot be null.");
        }

        return 
            $"Reservation ID: {reservation.Id}, Status: {reservation.Status}, " + 
            $"Showtime ID: {reservation.ShowtimeId}, User ID: {reservation.UserId}, " +
            $"Total Price: {reservation.TotalPrice}, Payment Method: {reservation.PaymentMethod}, " +
            $"Created On: {reservation.CreationDate}";
    }

    public IEnumerable<Reservation> GetAllReservation()
    {
        return _reservationRepository.GetAllReservations();
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