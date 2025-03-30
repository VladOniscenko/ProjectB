namespace ProjectB.DataModels;
public class Auditorium{
    public int Id;
    public int MaxSeats;
    public List<Seat> Seats;

    public Auditorium(int id, int maxSeats, List<Seat> seats){
        Id = id;
        MaxSeats = maxSeats;
        Seats = seats;
    }
}