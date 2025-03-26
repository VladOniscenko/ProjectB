using CinemaApp.Models;

namespace CinemaApp.Tests
{
    [TestClass]
    public class MovieModelTests
    {
        [TestMethod]
        public void MovieModel_Creation_ShouldSetPropertiesCorrectly()
        {
            string title = "Harry Potter and the Prisoner of Azkaban";
            string description = "Harry Potter's third year at Hogwarts is filled with new dangers as Sirius Black escapes from Azkaban.";
            int runtime = 142;
            string actors = "Daniel Radcliffe, Emma Watson, Rupert Grint, Gary Oldman";
            double rating = 7.9;
            string genre = "Fantasy";
            int ageRestriction = 12;
            DateTime releaseDate = new DateTime(2004, 5, 31);
            string country = "UK / USA";

            var movie = new Movie(title, description, runtime, actors, rating, genre, ageRestriction, releaseDate, country);

            // Assert
            Assert.AreEqual(title, movie.Title);
            Assert.AreEqual(description, movie.Description);
            Assert.AreEqual(runtime, movie.Runtime);
            Assert.AreEqual(actors, movie.Actors);
            Assert.AreEqual(rating, movie.Rating);
            Assert.AreEqual(genre, movie.Genre);
            Assert.AreEqual(ageRestriction, movie.AgeRestriction);
            Assert.AreEqual(releaseDate, movie.ReleaseDate);
            Assert.AreEqual(country, movie.Country);
        }

        [TestMethod]
        public void MovieModel_DefaultConstructor_ShouldCreateEmptyObject()
        {
            var movie = new Movie();
            Assert.IsNotNull(movie);
        }
    }
}