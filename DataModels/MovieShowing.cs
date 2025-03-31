namespace ProjectB.DataModels;
public class MovieShowing{
    public int Id;
    public int Movie;
    public DateTime DateTimeShowing;
    public Auditorium Auditorium;

    public MovieShowing(int id, int movieId, DateTime dateTimeShowing, Auditorium auditorium){
        Id = id;
        Movie = movieId;
        DateTimeShowing = dateTimeShowing;
        Auditorium = auditorium;
    }
}