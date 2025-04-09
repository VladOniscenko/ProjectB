using static System.Console;
using ProjectB.Database;
using ProjectB.Models.Movies;
using ProjectB.DataAccess;

// Used this video to help me out
// https://www.youtube.com/watch?v=qAWhGEPMlS8

namespace ProjectB
{
    class Menu
    {
        private int SelectedIndex = 0;
        private string[] Options;
        private string Prompt;

        public Menu(string prompt, string[] options)
        {
            Prompt = prompt;
            Options = options;
        }

        public static string CenterText(string text, int boxWidth, bool isActive = false)
        {
          int spaces = boxWidth - text.Length;
          if (isActive)
          {
            spaces -= 6;
            text = $">> {text} <<";
          }
          int padleft = spaces / 2;
          int padright = spaces - padleft;
          return new string(' ', padleft) + new string($"{text}") + new string(' ', padright);
        }
        
        public void DisplayOptions()
        {
            Clear();
            WriteLine(Prompt);
            
            WriteLine("╔══════════════════════════════════════╗");
            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];

                if (i == SelectedIndex)
                {   
                    WriteLine($"║{CenterText(currentOption, 38, true)}║"); 
                }
                else
                {
                    WriteLine($"║{CenterText(currentOption, 38)}║"); 
                } 
            }
            WriteLine("╚══════════════════════════════════════╝");
        }

        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                DisplayOptions();

                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                // Code block to update SelectedIndex
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex = (SelectedIndex - 1 + Options.Length) % Options.Length;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex = (SelectedIndex + 1) % Options.Length;
                }
            }
            while (keyPressed != ConsoleKey.Enter);
 
            return SelectedIndex;
        }

        static public void RunMenu()
        {
            string prompt = @"
 ____             __               ____                                              __             
/\  _`\          /\ \__           /\  _`\    __                                     /\ \            
\ \ \L\ \  __  __\ \ ,_\    __    \ \ \/\_\ /\_\    ___      __    ___ ___      __  \ \/      ____  
 \ \  _ <'/\ \/\ \\ \ \/  /'__`\   \ \ \/_/_\/\ \ /' _ `\  /'__`\/' __` __`\  /'__`\ \/      /',__\ 
  \ \ \L\ \ \ \_\ \\ \ \_/\  __/    \ \ \L\ \\ \ \/\ \/\ \/\  __//\ \/\ \/\ \/\ \L\.\_      /\__, `\
   \ \____/\/`____ \\ \__\ \____\    \ \____/ \ \_\ \_\ \_\ \____\ \_\ \_\ \_\ \__/.\_\     \/\____/
    \/___/  `/___/> \\/__/\/____/     \/___/   \/_/\/_/\/_/\/____/\/_/\/_/\/_/\/__/\/_/      \/___/ 
               /\___/                                                                               
               \/__/                                                                                       

Welcome customer!
Use Up & Down keys to select an option.
                ";
            string[] options = { "Register", "Login", "Movies", "About us", "Exit" };
            Menu menu = new Menu(prompt, options);
            int SelectedIndex = menu.Run();

            // Code block for keyPressed cases
            switch(SelectedIndex)
            {
                case 0:
                    Console.Clear();
                    Register();
                    break;
                case 1:
                    Console.Clear();
                    Login();
                    break;
                case 2:
                    Console.Clear();
                    Movies();
                    break;
                case 3:
                    Console.Clear();
                    AboutUs();
                    break;
                case 4:
                    Console.Clear();
                    return;
            }
        }

        // Methods for each option in menu. Might change/remove?
        static void Register()
        {
            WriteLine("Works.");
        }

        static void Login()
        {
            WriteLine("Works.");
        }

        static void Movies()
        {
            WriteLine("Works.");
        }

        static void AboutUs()
        {
            WriteLine("Works.");
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
        public static void Create(MovieRepository movieRepository)
        {
            bool completed = false;
            string currentState = "title";

            string movieTitle = "";
            string movieDescription = "";
            int? runtime = null;
            string actorsInput = "";
            double? rating = null;
            string genreInput = "";
            int? ageInput = null;
            DateTime? releaseDate = null;
            string countryInput = "";


            while (!completed)
            {
                switch (currentState)
                {
                    case "title":
                        if (!string.IsNullOrEmpty(movieTitle))
                        {
                            movieTitle = GetAndValidateInput<string>($"Enter movie title(Currently: {movieTitle})", 3, 50);
                        }
                        else
                        {
                            movieTitle = GetAndValidateInput<string>($"Enter movie title",3, 50);
                        }
                        currentState = "description";
                        break;

                    case "description":
                        if (!string.IsNullOrEmpty(movieDescription))
                        {
                            movieDescription = GetAndValidateInput<string>($"Enter movie description(Currently: {movieDescription})", 20, 500);
                        }
                        else
                        {
                            movieDescription = GetAndValidateInput<string>("Enter movie description", 20, 500);
                        }    
                        currentState = "Runtime";
                        break;

                    case "Runtime":
                        if (runtime != null)
                        {
                            runtime = GetAndValidateInput<int>($"Enter the runtime of the movie in minutes(Currently: {runtime})", 1, 240);
                        }
                        else
                        {
                            runtime = GetAndValidateInput<int>("Enter the runtime of the movie in minutes", 1, 240);
                        }
                        currentState = "Actors";
                        break;

                    case "Actors":
                        if (!string.IsNullOrEmpty(actorsInput))
                        {
                            actorsInput = GetAndValidateInput<string>($"Enter actors featuring(Currently: {actorsInput})", 3, 50);
                        }
                        else
                        {
                            actorsInput = GetAndValidateInput<string>("Enter actors featuring", 3, 50);
                        }
                        currentState = "Rating";
                        break;

                    case "Rating":
                        if (rating != null)
                        {
                            rating = GetAndValidateInput<double>($"Enter the movie rating(Currently {rating})", 1 ,10 );
                        }
                        else
                        {
                            rating = GetAndValidateInput<double>("Enter the movie rating",1 ,10);
                        }
                        currentState = "Genre";
                        break;

                    case "Genre":
                        if (!string.IsNullOrEmpty(genreInput))
                        {
                            genreInput = GetAndValidateInput<string>($"Enter the movie's genre(s) (Currently {genreInput})", 3, 50);
                        }
                        else
                        {
                            genreInput = GetAndValidateInput<string>("Enter the movie's genre(s)", 3, 50);
                        }
                        currentState = "Age";
                        break;

                    case "Age":
                        if (ageInput != null)
                        {
                            ageInput = GetAndValidateInput<int>($"Enter the movie's age restriction (Currently {ageInput})",1,99);
                        }
                        else
                        {
                            ageInput = GetAndValidateInput<int>("Enter the movie's age restriction",1,99);
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
                        if (!string.IsNullOrEmpty(countryInput))
                        {
                            countryInput = GetAndValidateInput<string>($"Enter country of origin (Currently {countryInput})",4 , 56);
                        }
                        else
                        {
                            countryInput = GetAndValidateInput<string>("Enter country of origin",4 , 56);
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
                            Console.WriteLine("Would you like to add another movie? (y/n)");
                            string addInput = Console.ReadLine();
                            if (addInput == "y")
                            {
                                movieRepository.AddMovie(new Movie
                                {
                                    Title = movieTitle,
                                    Description = movieDescription,
                                    Runtime = (int)runtime,
                                    Actors = actorsInput,
                                    Rating = (double)rating,
                                    Genre = genreInput,
                                    AgeRestriction = (int)ageInput,
                                    ReleaseDate = (DateTime)releaseDate,
                                    Country = countryInput
                                });
                                
                                completed = false;
                                currentState = "title";

                                movieTitle = "";
                                movieDescription = "";
                                runtime = null;
                                actorsInput = "";
                                rating = null;
                                genreInput = "";
                                ageInput = null;
                                releaseDate = null;
                                countryInput = "";

                                Console.WriteLine("Comfirmed and next will be made.");
                                Thread.Sleep(1000);
                            }
                            else if  ( addInput == "n")
                            {
                                movieRepository.AddMovie(new Movie
                                {
                                    Title = movieTitle,
                                    Description = movieDescription,
                                    Runtime = (int)runtime,
                                    Actors = actorsInput,
                                    Rating = (double)rating,
                                    Genre = genreInput,
                                    AgeRestriction = (int)ageInput,
                                    ReleaseDate = (DateTime)releaseDate,
                                    
                                });

                                completed = true;
                            }
                        }
                        else if (confirmInput == "n")
                        {
                            Console.WriteLine("Movie confirmation canceled.");
                            Thread.Sleep(1000);
                            currentState = "title";
                        }
                        else
                        {
                            Console.WriteLine("Invalid input try again");
                        }
                        break;
                }
            }
        }
    }
}
