namespace ProjectB.DataModels;
public class Seat{
    //made id and seatnumber different as every auditorium has a seatnumber beginning at 1 (can change if not preffered)
    public int Id;
    public int SeatNumber;
    public bool IsTaken;
    public string Type;

    public Seat(int id, int seatNumber, bool isTaken, string type){
        Id = id;
        SeatNumber = seatNumber;
        IsTaken = isTaken;
        Type = type;
    }

}