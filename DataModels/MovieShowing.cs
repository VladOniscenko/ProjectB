namespace ProjectB.DataModels;
public class MovieShowing{
    public int Id;
    public Movie Movie;
    public DateTime DateTimeShowing;
    public Auditorium Auditorium;

    public MovieShowing(int id, Movie movie, DateTime dateTimeShowing, Auditorium auditorium){
        Id = id;
        Movie = movie;
        DateTimeShowing = dateTimeShowing;
        Auditorium = auditorium;
    }
}