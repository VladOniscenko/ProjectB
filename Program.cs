using System.Diagnostics;
using ProjectB.Database;
using ProjectB.Models.Movies;
namespace ProjectB;

class Program
{
    static void Main()
    {

        DbFactory.InitializeDatabase();
        
        var movieRepo = new MovieRepository();
        movieRepo.AddMovie(Create());
        
        // movieRepo.AddMovie(new Movie 
        // { 
        //     Title = "Inception", 
        //     Description = "A thief who enters the dreams of others to steal secrets.",
        //     Runtime = 148,
        //     Actors = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page",
        //     Rating = 8.8,
        //     Genre = "Sci-Fi",
        //     AgeRestriction = "PG-13",
        //     ReleaseDate = new DateTime(2010, 7, 16),
        //     Country = "USA"
        // });
        
        
    }
    
    public static bool validateString(int length, string input)
    {
        if (input.Length < length || input.Contains("PG") && input.Length < length)
            {
                Console.WriteLine("Invalid title, try again");
                Thread.Sleep(1000);
                return false;
            }
        else{return true;}
    }
    public static Movie Create()
    {
            bool completed = false;
            string currentState = "title";

            string movieTitle = "";
            string movieDescription = "";
            int runtime = 0;
            string actorsInput = "";
            double rating = 0;
            string genreInput = "";
            string ageInput = "";
            DateTime releaseDate = DateTime.MinValue;
            string countryInput = "";


            while (!completed)
        {
            switch (currentState)
            {
                case "title":
                    Console.Clear();
                    Console.WriteLine("Enter title:");
                    movieTitle = Console.ReadLine();
                    if (!validateString(3, movieTitle))
                    {
                        continue;
                    }
                    currentState = "description";
                    break;

                case "description":
                    Console.Clear();
                    Console.WriteLine("Enter description:");
                    movieDescription = Console.ReadLine();
                    if (!validateString(10, movieDescription))
                    {
                        continue;
                    }
                    currentState = "Runtime";
                    break;

                case "Runtime":
                    Console.Clear();
                    Console.WriteLine("Enter the runtime of the movie in minutes:");
                    string runtimeInput = Console.ReadLine();
                    if (!int.TryParse(runtimeInput, out runtime) || runtime < 30)
                    {
                        Console.WriteLine("Invalid runtime, try again");
                        Thread.Sleep(1000);
                        continue;
                    }
                    currentState = "Actors";
                    break;

                case "Actors":
                    Console.Clear();
                    Console.WriteLine("Enter the featuring actors:");
                    actorsInput = Console.ReadLine();
                    if (!validateString(10, actorsInput))
                    {
                        continue;
                    }
                    currentState = "Rating";
                    break;

                case "Rating":
                    Console.Clear();
                    Console.WriteLine("Enter Rating (0 - 10):");
                    string ratingInput = Console.ReadLine();
                    rating = double.Parse(ratingInput);
                    if (rating > 10.0 && rating < 0.0)
                    {
                        Console.WriteLine("Invalid rating, try again");
                        Thread.Sleep(1000);
                        continue;
                    }
                    currentState = "Genre";
                    break;

                case "Genre":
                    Console.Clear();
                    Console.WriteLine("Enter Genre:");
                    genreInput = Console.ReadLine();
                    if (!validateString(3, genreInput))
                    {
                        continue;
                    }
                    currentState = "Age";
                    break;

                case "Age":
                    Console.Clear();
                    Console.WriteLine("Enter age restriction:");
                    ageInput = Console.ReadLine();
                    if (!validateString(1, ageInput))
                    {
                        continue;
                    }
                    currentState = "Release";
                    break;

                case "Release":
                    Console.Clear();
                    Console.WriteLine("Enter release date:");
                    string releaseInput = Console.ReadLine();
                    if (!DateTime.TryParse(releaseInput, out releaseDate))
                    {
                        Console.WriteLine("Invalid release date, try again");
                        Thread.Sleep(1000);
                        continue;
                    }
                    currentState = "Country";
                    break;

                case "Country":
                    Console.Clear();
                    Console.WriteLine("Enter country of origin:");
                    countryInput = Console.ReadLine();
                    if (!validateString(3, countryInput))
                    {
                        continue;
                    }
                    completed = true;
                    break;
            }
        }
        return new Movie
        {
            Title = movieTitle,
            Description = movieDescription,
            Runtime = runtime,
            Actors = actorsInput,
            Rating = rating,
            Genre = genreInput,
            AgeRestriction = ageInput,
            ReleaseDate = releaseDate,
            Country = countryInput
        };
    }
}