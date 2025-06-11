using Microsoft.Extensions.DependencyInjection;
using ProjectB.Logic;

namespace ProjectB.Presentation;

public class CreateMovieFlow
{

    private readonly IServiceProvider _services;
    private readonly MovieLogic _movieService;
    public CreateMovieFlow()
    {
        _services = Program.Services;
        _movieService = _services.GetRequiredService<MovieLogic>();
    }

    public void Run()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════╗");
        Console.WriteLine("║     Create movie     ║");
        Console.WriteLine("╚══════════════════════╝");

        Console.SetCursorPosition(0, 24);
        Console.Write("                                                                                     \n");

        Console.WriteLine("╔══════════════════════════════════════════════╗");
        Console.WriteLine("║     Press ESC to return back to the menu     ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝");

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
            while (!MovieLogic.ValidateInput<string>(3, 50, movieTitle))
            {
                BaseUI.ShowErrorMessage("Your input has to be between 3 and 50 characters.", 5);
                movieTitle = BaseUI.DrawInputBox("Enter movie title", 20, 30, 0, 4, movieTitle);
            }

            Console.SetCursorPosition(0, 5);
            Console.Write("                                                                                     ");

            // Description
            movieDescription = DrawMovieDescriptionInputBox("Enter movie description", 25, 70, 2, 0, 6, movieDescription);
            while (!MovieLogic.ValidateInput<string>(20, 100, movieDescription))
            {
                BaseUI.ShowErrorMessage("Your input has to be between 20 and 100 characters.", 8);
                movieDescription = DrawMovieDescriptionInputBox("Enter movie description", 25, 50, 2, 0, 6, movieDescription);
            }

            Console.SetCursorPosition(0, 8);
            Console.Write("                                                                                     ");

            // Runtime
            runtime = BaseUI.DrawInputBox("Enter runtime (in minutes)", 30, 30, 0, 8, runtime);
            while (!MovieLogic.ValidateInput<int>(60, 240, runtime.ToString()))
            {
                BaseUI.ShowErrorMessage("Please enter a number in between 60 and 240 minutes.", 10);
                runtime = BaseUI.DrawInputBox("Enter runtime (in minutes)", 30, 30, 0, 9, runtime);
            }

            Console.SetCursorPosition(0, 10);
            Console.Write("                                                                                     ");
            // Actors
            actorsInput = BaseUI.DrawInputBox("Enter actor names (comma separated)", 40, 30, 0, 10, actorsInput);
            while (!MovieLogic.ValidateInput<string>(5, 200, actorsInput))
            {
                BaseUI.ShowErrorMessage("Your input has to be between 5 and 200 characters.", 12);
                actorsInput = BaseUI.DrawInputBox("Enter actor names (comma separated)", 40, 30, 0, 11, actorsInput);
            }

            Console.SetCursorPosition(0, 12);
            Console.Write("                                                                                     ");

            // Rating
            rating = BaseUI.DrawInputBox("Enter rating", 20, 30, 0, 12, rating);
            while (!MovieLogic.ValidateInput<double>(0, 10, rating))
            {
                BaseUI.ShowErrorMessage("Please enter a valid rating between 0.0 and 10.0.", 14);
                rating = BaseUI.DrawInputBox("Enter rating", 20, 30, 0, 13, rating);
            }

            Console.SetCursorPosition(0, 14);
            Console.Write("                                                                                     ");


            // Genre
            genreInput = BaseUI.DrawInputBox("Enter genre", 20, 30, 0, 14, genreInput);
            while (!MovieLogic.ValidateInput<string>(3, 50, genreInput))
            {
                BaseUI.ShowErrorMessage("Your input has to be between 3 and 50 characters.", 16);
                genreInput = BaseUI.DrawInputBox("Enter genre", 20, 30, 0, 15, genreInput);
            }

            Console.SetCursorPosition(0, 16);
            Console.Write("                                                                                     ");

            // Age Restriction
            ageInput = BaseUI.DrawInputBox("Enter age restriction", 30, 30, 0, 16, ageInput);
            while (!MovieLogic.ValidateInput<int>(0, 99, ageInput))
            {
                BaseUI.ShowErrorMessage("Please enter a valid non-negative integer for age restriction.", 18);
                ageInput = BaseUI.DrawInputBox("Enter age restriction", 30, 30, 0, 17, ageInput).Trim('+');
            }

            Console.SetCursorPosition(0, 18);
            Console.Write("                                                                                     ");

            // Release Date
            releaseDate = BaseUI.DrawInputBox("Enter release date", 30, 30, 0, 18, releaseDate);
            while (!MovieLogic.ValidateInput<DateTime>(0, 100, releaseDate))
            {
                BaseUI.ShowErrorMessage("Please enter a valid date in the format yyyy-MM-dd.", 20);
                releaseDate = BaseUI.DrawInputBox("Enter release date (yyyy-MM-dd)", 30, 30, 0, 19, releaseDate);
            }

            Console.SetCursorPosition(0, 20);
            Console.Write("                                                                                     ");
            // Country
            countryInput = BaseUI.DrawInputBox("Enter country", 25, 30, 0, 20, countryInput);
            while (!MovieLogic.ValidateInput<string>(2, 50, countryInput))
            {
                BaseUI.ShowErrorMessage("Your input has to be between 2 and 50 characters.", 22);
                countryInput = BaseUI.DrawInputBox("Enter country", 25, 30, 0, 21, countryInput);
            }

            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(@$"
            Movie title: {movieTitle}
            Movie description: {movieDescription}
            Runtime: {runtime} minutes
            Featuring: {actorsInput}
            Movie Rating: {rating}/10
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

private string DrawMovieDescriptionInputBox(string label, int BoxX = 15, int Width = 30, int Height = 2, int x = -1, int y = -1,
    string? previouslyWritten = null)
{
    Console.SetCursorPosition(x, y);
    Console.Write(label + ": ");
    
    string[] lines = new string[Height];
    for (int i = 0; i < Height; i++)
        lines[i] = string.Empty;

    if (previouslyWritten is not null)
    {
        var words = previouslyWritten.Split(' ');
        int currentLine = 0;
        foreach (var word in words)
        {
            if (lines[currentLine].Length + word.Length + 1 > Width - 1)
            {
                currentLine++;
                if (currentLine >= Height) break;
            }
            if (lines[currentLine].Length > 0)
                lines[currentLine] += " ";
            lines[currentLine] += word;
        }
    }

    for (int i = 0; i < Height; i++)
    {
        Console.SetCursorPosition(BoxX, y + i);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(new string(' ', Width));
        Console.SetCursorPosition(BoxX, y + i);
        Console.Write(lines[i]);
    }

    Console.SetCursorPosition(BoxX, y);
    int currentLineIndex = 0;
    int cursorPos = lines[currentLineIndex].Length;

    while (true)
    {
        Console.SetCursorPosition(BoxX + cursorPos, y + currentLineIndex);
        var key = Console.ReadKey(intercept: true);

        if (key.Key == ConsoleKey.Enter)
        {
            break;
        }

        if (key.Key == ConsoleKey.Backspace)
        {
            if (cursorPos > 0)
            {
                cursorPos--;
                lines[currentLineIndex] = lines[currentLineIndex].Remove(cursorPos, 1);
            }
            else if (currentLineIndex > 0)
            {
                currentLineIndex--;
                cursorPos = lines[currentLineIndex].Length;
            }
        }
        else if (!char.IsControl(key.KeyChar))
        {
            if (lines[currentLineIndex].Length + 1 < Width)
            {
                lines[currentLineIndex] = lines[currentLineIndex].Insert(cursorPos, key.KeyChar.ToString());
                cursorPos++;
            }
            else if (currentLineIndex < Height - 1)
            {
                var lastWordIndex = lines[currentLineIndex].LastIndexOf(' ');
                if (lastWordIndex != -1 && cursorPos > lastWordIndex)
                {
                    string overflow = lines[currentLineIndex].Substring(lastWordIndex + 1);
                    lines[currentLineIndex] = lines[currentLineIndex].Substring(0, lastWordIndex);
                    currentLineIndex++;
                    lines[currentLineIndex] = overflow + key.KeyChar;
                    cursorPos = overflow.Length + 1;
                }
            }
        }

        for (int i = 0; i < Height; i++)
        {
            Console.SetCursorPosition(BoxX, y + i);
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(new string(' ', Width));
            Console.SetCursorPosition(BoxX, y + i);
            Console.Write(lines[i]);
        }
    }

    Console.ResetColor();
    return string.Join(" ", lines).Trim();
}
}