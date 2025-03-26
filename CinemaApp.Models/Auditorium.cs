public class Auditorium{
    public int ID;
    public int MaxSeats;
    public List<Seat> Seats;

    public Auditorium(int id, int maxSeats, List<Seat> seats){
        ID = id;
        MaxSeats = maxSeats;
        Seats = seats;
    }
}