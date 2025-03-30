namespace CinemaApp.Models;
public class Reservation{
    public MovieShowing Movie;
    //should multiple seats be able ro be reserved in the same reservation?
    public Seat Seat;

    public Reservation(MovieShowing movie, Seat seat){
        Movie = movie;
        Seat =  seat;
    }
}