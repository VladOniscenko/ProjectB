﻿using ProjectB.Database;
using ProjectB.Models.Movies;
using ProjectB.DataAccess;
namespace ProjectB;

class Program
{
    static void Main()
    {

        string prompt = "Welcome customer!";
        string[] options = { "Register", "Login", "Movies", "About us", "Exit" };
        Menu menu = new Menu(prompt, options);
        int SelectedIndex = menu.Run();

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
}