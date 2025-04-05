﻿using ProjectB.Database;
using ProjectB.Models.Movies;
using ProjectB.DataAccess;
namespace ProjectB;

class Program
{
    static void Main()
    {   
        Start();

        DbFactory.InitializeDatabase();
        
        var movieRepo = new MovieRepository();
        
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
        
        // foreach (Movie movie in movieRepo.GetBestAndNewestMovies())
        // {
        //     Console.WriteLine($"{movie}");
        // }
    }

    static public void Start()
    {
        RunMenu();
    }

    static private void RunMenu()
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
    }
}