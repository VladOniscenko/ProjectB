namespace CinemaApp.Models;
public class MovieShowing{
    public int ID;
    public Movie Movie;
    public DateTime DateTimeShowing;
    public Auditorium Auditorium;

    public MovieShowing(int id, Movie movie, DateTime dateTimeShowing, Auditorium auditorium){
        ID = id;
        Movie = movie;
        DateTimeShowing = dateTimeShowing;
        Auditorium = auditorium;
    }
}