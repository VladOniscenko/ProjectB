using System;
using ProjectB.DataModels;
using ProjectB.DataAccess;

namespace ProjectB.Logic
{
    public class MoviesLogic
    {
        public MoviesLogic() { }

        public Movie Create()
        {
            Console.Write("Enter Movie Title: ");
            string title = Console.ReadLine() ?? "Untitled";

            Console.Write("Enter Description: ");
            string description = Console.ReadLine() ?? "No description available.";

            Console.Write("Enter Runtime (minutes): ");
            int runtime = int.TryParse(Console.ReadLine(), out int parsedRuntime) ? parsedRuntime : 90;

            Console.Write("Enter Actors (comma separated): ");
            string actors = Console.ReadLine() ?? "Unknown actors";

            Console.Write("Enter Rating (0-10): ");
            double rating = double.TryParse(Console.ReadLine(), out double parsedRating) ? parsedRating : 5.0;

            Console.Write("Enter Genre: ");
            string genre = Console.ReadLine() ?? "Unknown";

            Console.Write("Enter Age Restriction (e.g. 13, 18): ");
            int ageRestriction = int.TryParse(Console.ReadLine(), out int parsedAge) ? parsedAge : 0;

            Console.Write("Enter Release Date (YYYY-MM-DD): ");
            DateTime releaseDate = DateTime.TryParse(Console.ReadLine(), out DateTime parsedDate) ? parsedDate : DateTime.Now;

            Console.Write("Enter Country: ");
            string country = Console.ReadLine() ?? "Unknown";

            var newMovie = new Movie(title, description, runtime, actors, rating, genre, ageRestriction, releaseDate, country);

            MoviesAccess.Write(newMovie);

            Console.WriteLine($"Movie '{title}' created successfully!");
            return newMovie;
        }
    }
}