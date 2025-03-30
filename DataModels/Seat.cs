namespace CinemaApp.Models;
public class Seat{
    //made id and seatnumber different as every auditorium has a seatnumber beginning at 1 (can change if not preffered)
    public int ID;
    public int SeatNumber;
    public bool IsTaken;
    public string Type;

    public Seat(int id, int seatNumber, bool isTaken, string type){
        ID = id;
        SeatNumber = seatNumber;
        IsTaken = isTaken;
        Type = type;
    }

}