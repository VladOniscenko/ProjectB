using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.AccessControl;
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
    
    public static T GetAndValidateInput<T>(string prompt, int min = 0, int max= 100)
    {
            Console.Clear();
            Console.WriteLine($"{prompt}:");
            string input = Console.ReadLine();

            if (typeof(T) == typeof(int))
            {
                if (int.TryParse(input,out int value) && value >= min && value <= max)
                {
                    return (T) (object) value!;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                if (!string.IsNullOrEmpty(input) && input.Length >= min && input.Length <= max)
                {
                    return (T) (object) input!;
                }
            }
            else if (typeof(T) == typeof(double))
            {
                if (double.TryParse(input,out double value))
                {
                    return (T) (object) value;
                }
            }
            else if (typeof(T) == typeof(DateTime))
            {
                if (DateTime.TryParse(input,out DateTime value))
                {
                    return (T) (object) value;
                }
            }

            Console.WriteLine("Invalid input");
            Thread.Sleep(1000);
            return GetAndValidateInput<T>(prompt,min,max);
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
            int ageInput = 0;
            DateTime? releaseDate = null;
            string countryInput = "";


            while (!completed)
        {
            switch (currentState)
            {
                case "title":
                    if (!string.IsNullOrEmpty(movieTitle))
                    {
                        movieTitle = GetAndValidateInput<string>($"Enter movie title(Currently: {movieTitle})");
                    }
                    else
                    {
                        movieTitle = GetAndValidateInput<string>($"Enter movie title");
                    }
                    currentState = "description";
                    break;

                case "description":
                    if (!string.IsNullOrEmpty(movieTitle))
                    {
                        movieDescription = GetAndValidateInput<string>($"Enter movie description(Currently: {movieDescription})");
                    }
                    else
                    {
                        movieDescription = GetAndValidateInput<string>("Enter movie description");
                    }    
                    currentState = "Runtime";
                    break;

                case "Runtime":
                    if (runtime != null)
                    {
                        runtime = GetAndValidateInput<int>($"Enter the runtime of the movie in minutes(Currently: {runtime})");
                    }
                    else
                    {
                        runtime = GetAndValidateInput<int>("Enter the runtime of the movie in minutes");
                    }
                    currentState = "Actors";
                    break;

                case "Actors":
                    if (string.IsNullOrEmpty(actorsInput))
                    {
                        actorsInput = GetAndValidateInput<string>($"Enter actors featuring(Currently: {actorsInput})");
                    }
                    else
                    {
                        actorsInput = GetAndValidateInput<string>("Enter actors featuring");
                    }
                    currentState = "Rating";
                    break;

                case "Rating":
                    if (rating != null)
                    {
                        rating = GetAndValidateInput<double>($"Enter the movie rating(Currently {rating})");
                    }
                    else
                    {
                        rating = GetAndValidateInput<double>("Enter the movie rating");
                    }
                    currentState = "Genre";
                    break;

                case "Genre":
                    if (string.IsNullOrEmpty(genreInput))
                    {
                        genreInput = GetAndValidateInput<string>($"Enter the movie's genre(s) (Currently {genreInput})");
                    }
                    else
                    {
                        genreInput = GetAndValidateInput<string>("Enter the movie's genre(s)");
                    }
                    currentState = "Age";
                    break;

                case "Age":
                    if (ageInput != null)
                    {
                        ageInput = GetAndValidateInput<int>($"Enter the movie's age restriction (Currently {ageInput})");
                    }
                    else
                    {
                        ageInput = GetAndValidateInput<int>("Enter the movie's age restriction ");
                    }
                    currentState = "Release";
                    break;

                case "Release":
                    if (releaseDate.HasValue)
                    {
                        releaseDate = GetAndValidateInput<DateTime>($"Enter release date (Currently{releaseDate})");
                    }
                    else
                    {
                        releaseDate = GetAndValidateInput<DateTime>("Enter release date");
                    }
                    currentState = "Country";
                    break;

                case "Country":
                    if (string.IsNullOrEmpty(countryInput))
                    {
                        countryInput = GetAndValidateInput<string>($"Enter country of origin (Currently {countryInput})");
                    }
                    else
                    {
                        countryInput = GetAndValidateInput<string>("Enter country of origin");
                    }
                    currentState = "Finish";
                    break;

                case "Finish":
                    Console.Clear();
                    Console.WriteLine(@$"
                    Movie title: {movieTitle}
                    Movie despriction: {movieDescription}
                    Runtime: {runtime} minutes
                    Featuring: {actorsInput}
                    Rating: {rating}/10
                    Genre: {genreInput}
                    Agerestriction: {ageInput}
                    Release: {releaseDate}
                    Country: {countryInput}
                    ");
                    Console.WriteLine("Do you confirm this movie? (y/n)");
                    string confirmInput = Console.ReadLine();
                    if (confirmInput == "y")
                    {
                        completed = true;
                    }
                    else if (confirmInput == "n")
                    {
                        currentState = "title";
                    }
                    else
                    {
                        Console.WriteLine("Invalid input try again");
                    }
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
            ReleaseDate = (DateTime)releaseDate,
            Country = countryInput
        };
    }
}