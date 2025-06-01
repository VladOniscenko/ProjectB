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

    // Tests empty movie input
    [DataTestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow("  ")]
    [DataRow("   ")]
    public void IsMovieIDValid_IncorrectInput(string movieName)
    {
        bool input = _showtimeLogic.IsMovieIDValid(movieName);
        Assert.AreEqual(false, input);
    }

    // Tests correct movie input
    [DataTestMethod]
    [DataRow("G")]
    [DataRow("Gu")]
    [DataRow("Guw")]
    [DataRow("Guwn")]
    [DataRow("Guwns")]
    public void IsMovieIDValid_CorrectInput(string movieName)
    {
        bool input = _showtimeLogic.IsMovieIDValid(movieName);
        Assert.AreEqual(true, input);
    }

    // "yyyy-MM-dd HH:mm"
    // Tests incorrect movie start time input
    [DataTestMethod]
    [DataRow("")]
    [DataRow("16-06-2004 30:15")]
    [DataRow("06-16-2004 20:40")]
    [DataRow("16-2004-06 10:00")]
    [DataRow("06-2004-16 100:100")]
    [DataRow("2004-06-16 25:10")]
    [DataRow("2004-06-16 23:59")]
    public void IsMovieStartTimeValid_IncorrectInput(string startTime)
    {
        bool input = _showtimeLogic.IsMovieIDValid(startTime);
        Assert.AreEqual(false, input);
    }

    // Tests correct movie start time input
    [DataTestMethod]
    [DataRow("2025-06-16 12:00")]
    [DataRow("2026-07-17 13:00")]
    [DataRow("2030-08-18 14:00")]
    [DataRow("2059-12-31 23:59")]
    [DataRow("2100-01-01 00:00")]
    public void IsMovieStartTimeValid_CorrectInput(string startTime)
    {
        bool input = _showtimeLogic.IsMovieIDValid(startTime);
        Assert.AreEqual(true, input);
    }
}