using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectB.DataAccess;
using ProjectB.Logic;

namespace ProjectB.UnitTests;

[TestClass]
public class ShowtimeTest
{
    private ShowtimeLogic? _showtimeLogic;

    [TestInitialize]
    public void Initialize()
    {
        var showtimeRepository = new ShowtimeRepository();
        _showtimeLogic = new ShowtimeLogic(showtimeRepository);
    }

    // First prompt in Showtime: Enter name/keyword of a movie
    [DataTestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("  ")]
    [DataRow("   ")]
    public void IsMovieIDValid_EmptyInput(string movieName)
    {
        bool input = _showtimeLogic.IsMovieIDValid(movieName);
        Assert.AreEqual(false, input);
    }
}