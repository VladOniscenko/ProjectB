using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectB.Logic;

namespace ProjectB.UnitTests;

public class ReservationTest
{
    [DataTestMethod]
    [DataRow(-1, false)]
    [DataRow(0, false)]
    [DataRow(1, true)]
    public void ValidatesShowtimeId(int showtimeId, bool expectedResult)
    {
        bool res = ReservationLogic.IsShowtimeValid(showtimeId);
        Assert.AreEqual(res, expectedResult);
    }
    
    [DataTestMethod]
    [DataRow("ideal", true)]
    [DataRow("", false)]
    [DataRow(null, false)]
    public void ValidatesMethod(string method, bool expectedResult)
    {
        bool res = ReservationLogic.IsPaymentMethodValid(method);
        Assert.AreEqual(res, expectedResult);
    }
    
    [DataTestMethod]
    [DataRow(100.00, true)]
    [DataRow(0.01, true)]
    [DataRow(0.00, false)]
    [DataRow(-10.00, false)]
    public void ValidatesTotalPrice(decimal price, bool expectedResult)
    {
        bool res = ReservationLogic.IsTotalPriceValid(price);
        Assert.AreEqual(res, expectedResult);
    }
    
    [DataTestMethod]
    [DataRow(1, true)]
    [DataRow(0, false)]
    [DataRow(-1, false)]
    public void ValidatesUserId(int id, bool expectedResult)
    {
        bool res = ReservationLogic.IsUserValid(id);
        Assert.AreEqual(res, expectedResult);
    }
}


