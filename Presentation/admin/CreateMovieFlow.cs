using Microsoft.Extensions.DependencyInjection;
using ProjectB.Logic.Interfaces;

namespace ProjectB.Presentation;

public class CreateMovieFlow
{
    
    private readonly IServiceProvider _services;
    private readonly IMovieService _movieService;
    public CreateMovieFlow()
    {
        _services = Program.Services;
        _movieService = _services.GetRequiredService<IMovieService>();
    }
    
    public void Run()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════╗");
        Console.WriteLine("║     Create movie     ║");
        Console.WriteLine("╚══════════════════════╝");

        bool completed = false;

        string movieTitle = "";
        string movieDescription = "";
        string? runtime = null;
        string actorsInput = "";
        string? rating = null;
        string genreInput = "";
        string? ageInput = null;
        string? releaseDate = null;
        string countryInput = "";

        while (!completed)
        {
            // Title
            movieTitle = BaseUI.DrawInputBox("Enter movie title", 20, 30, 0, 4, movieTitle);
            while (!_movieService.ValidateInput<string>(3, 50, movieTitle))
            {
                BaseUI.ShowErrorMessage("Your input has to be between 3 and 50 characters.", 5);
                movieTitle = BaseUI.DrawInputBox("Enter movie title", 20, 30, 0, 4, movieTitle);
            }

            Console.SetCursorPosition(0, 5);
            Console.Write("                                                                                     ");

            // Description
            movieDescription = BaseUI.DrawInputBox("Enter movie description", 25, 30, 0, 6, movieDescription);
            while (!_movieService.ValidateInput<string>(20, 100, movieDescription))
            {
                BaseUI.ShowErrorMessage("Your input has to be between 20 and 100 characters.", 7);
                movieDescription = BaseUI.DrawInputBox("Enter movie description", 25, 30, 0, 6, movieDescription);
            }

            Console.SetCursorPosition(0, 7);
            Console.Write("                                                                                     ");

            // Runtime
            runtime = BaseUI.DrawInputBox("Enter runtime (in minutes)", 30, 30, 0, 8, runtime);
            while (!_movieService.ValidateInput<int>(60, 240, runtime.ToString()))
            {
                BaseUI.ShowErrorMessage("Please enter a number in between 60 and 240 minutes.", 9);
                runtime = BaseUI.DrawInputBox("Enter runtime (in minutes)", 30, 30, 0, 8, runtime);
            }

            Console.SetCursorPosition(0, 9);
            Console.Write("                                                                                     ");
            // Actors
            actorsInput = BaseUI.DrawInputBox("Enter actor names (comma separated)", 40, 30, 0, 10, actorsInput);
            while (!_movieService.ValidateInput<string>(5, 200, actorsInput))
            {
                BaseUI.ShowErrorMessage("Your input has to be between 5 and 200 characters.", 11);
                actorsInput = BaseUI.DrawInputBox("Enter actor names (comma separated)", 40, 30, 0, 10, actorsInput);
            }

            Console.SetCursorPosition(0, 11);
            Console.Write("                                                                                     ");

            // Rating
            rating = BaseUI.DrawInputBox("Enter rating", 20, 30, 0, 12, rating);
            while (!_movieService.ValidateInput<double>(0, 10, rating))
            {
                BaseUI.ShowErrorMessage("Please enter a valid rating between 0.0 and 10.0.", 13);
                rating = BaseUI.DrawInputBox("Enter rating", 20, 30, 0, 12, rating);
            }

            Console.SetCursorPosition(0, 13);
            Console.Write("                                                                                     ");


            // Genre
            genreInput = BaseUI.DrawInputBox("Enter genre", 20, 30, 0, 14, genreInput);
            while (!_movieService.ValidateInput<string>(3, 50, genreInput))
            {
                BaseUI.ShowErrorMessage("Your input has to be between 3 and 50 characters.", 15);
                genreInput = BaseUI.DrawInputBox("Enter genre", 20, 30, 0, 14, genreInput);
            }

            Console.SetCursorPosition(0, 15);
            Console.Write("                                                                                     ");

            // Age Restriction
            ageInput = BaseUI.DrawInputBox("Enter age restriction", 30, 30, 0, 16, ageInput);
            while (!_movieService.ValidateInput<int>(0, 99, ageInput))
            {
                BaseUI.ShowErrorMessage("Please enter a valid non-negative integer for age restriction.", 17);
                ageInput = BaseUI.DrawInputBox("Enter age restriction", 30, 30, 0, 16, ageInput);
            }

            Console.SetCursorPosition(0, 17);
            Console.Write("                                                                                     ");

            // Release Date
            releaseDate = BaseUI.DrawInputBox("Enter release date", 30, 30, 0, 18, releaseDate);
            while (!_movieService.ValidateInput<DateTime>(0, 100, releaseDate))
            {
                BaseUI.ShowErrorMessage("Please enter a valid date in the format yyyy-MM-dd.", 19);
                releaseDate = BaseUI.DrawInputBox("Enter release date (yyyy-MM-dd)", 30, 30, 0, 18, releaseDate);
            }

            Console.SetCursorPosition(0, 19);
            Console.Write("                                                                                     ");
            // Country
            countryInput = BaseUI.DrawInputBox("Enter country", 25, 30, 0, 20, countryInput);
            while (!_movieService.ValidateInput<string>(2, 50, countryInput))
            {
                BaseUI.ShowErrorMessage("Your input has to be between 2 and 50 characters.", 21);
                countryInput = BaseUI.DrawInputBox("Enter country", 25, 30, 0, 20, countryInput);
            }

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(@$"
            Movie title: {movieTitle}
            Movie description: {movieDescription}
            Runtime: {runtime} minutes
            Featuring: {actorsInput}
            Rating: {rating}/10
            Genre: {genreInput}
            Age restriction: {ageInput}
            Release: {releaseDate}
            Country: {countryInput}
            ");
            Console.WriteLine("Do you confirm this movie?");

            if (BaseUI.BasicYesOrNo(12))
            {
                BaseUI.ConfirmingMessage("You successfully created a movie", 21);
                Console.ReadKey();
                
                _movieService.CreateMovie(
                    movieTitle,
                    movieDescription,
                    int.Parse(runtime),
                    actorsInput,
                    double.Parse(rating),
                    genreInput,
                    int.Parse(ageInput),
                    DateTime.Parse(releaseDate),
                    countryInput
                );

                Console.Clear();
                Console.WriteLine("Would you like to create another?");
                if (BaseUI.BasicYesOrNo())
                {
                    Run();
                }

                completed = true;
            }

            Console.Clear();
            continue;
        }
    }
}