public class Movie{

    public string MovieName;
    public string MovieInfo;
    public int Runtime;
    public bool HasBreak;
    public string[] Actors;
    public int MinimumAge;

    public Movie(string movieName, string movieInfo, int runtime, bool hasBreak, string[] actors, int minimumAge){
        MovieName = movieName;
        MovieInfo = movieInfo;
        Runtime = runtime;
        HasBreak = hasBreak;
        Actors = actors;
        MinimumAge = minimumAge;
    }

}