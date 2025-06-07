using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.UnitTests;

[TestClass]
public class CreateMovieTest
{
    [DataTestMethod]
    [DataRow("AAAA", true)]
    [DataRow("", false)]
    [DataRow("AB", true)]
    [DataRow("Longer but still valid string", true)]
    public void ValidateInput_ValitdateString_ReturnTrue(string inputString, bool expectedResult)
    {
        bool res = MovieLogic.ValidateInput<string>(2, 50, inputString);
        Assert.AreEqual(res, expectedResult);
    }

    [DataTestMethod]
    [DataRow("1", true)]
    [DataRow("100", true)]
    [DataRow("", false)]
    [DataRow("AA    AAAA", false)]

    public void ValidateInput_IntInput_ReturnsExpected(string inputInt, bool expectedResult)
    {
        bool res = MovieLogic.ValidateInput<int>(0, 100, inputInt);
        Assert.AreEqual(res, expectedResult);
    }

    [DataTestMethod]
    [DataRow("0.1", true)]
    [DataRow("9.9", true)]
    [DataRow("10.5", false)]
    [DataRow("100", false)]
    [DataRow("abc", false)]
    [DataRow("", false)]
    public void ValidateInput_StringRepresentingDouble_ReturnsExpected(string input, bool expectedResult)
    {
        bool result = MovieLogic.ValidateInput<double>(0, 10, input);
        Assert.AreEqual(expectedResult, result);
    }
    
    [DataTestMethod]
    [DataRow("2020-01-01", true)]
    [DataRow("1990-12-31", false)]
    [DataRow("2050-05-15", true)]
    [DataRow("abcd", false)]
    [DataRow("", false)]
    public void ValidateInput_StringRepresentingDateTime_ReturnsExpected(string input, bool expectedResult)
    {
        DateTime minDate = new DateTime(2000, 1, 1);
        DateTime maxDate = new DateTime(2050, 12, 31);

        bool result = MovieLogic.ValidateInput<DateTime>(minDate.Year, maxDate.Year, input);
        Assert.AreEqual(expectedResult, result);
    }
}