using System;
using ProjectB.DataModels;
using ProjectB.Logic;

namespace ProjectB;

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello");


        var moviesLogic = new MoviesLogic();
        Movie createdMovie = moviesLogic.Create();

        Console.WriteLine($"Created Movie: {createdMovie.Title} ({createdMovie.ReleaseDate.Year})");

        
    }
}