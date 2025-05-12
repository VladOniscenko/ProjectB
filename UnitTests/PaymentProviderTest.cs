using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectB.Logic;

namespace ProjectB.UnitTests;

[TestClass]
public class PaymentProviderTest
{
    [DataTestMethod]
    [DataRow("ideal", true)]
    [DataRow("alibaba", false)]
    public void SetPaymentMethod_ValidatesMethod_CorrectlyHandles(string method, bool expectedResult)
    {
        // arrange
        var paymentProvider = new PaymentProvider("Test payment");

        // act
        var result = paymentProvider.SetPaymentMethod(method);

        // assert
        Assert.AreEqual(result, expectedResult);
    }

    [DataTestMethod]
    [DataRow(100.00, true)]
    [DataRow(0.01, true)]
    [DataRow(0.00, false)]
    [DataRow(-10.00, false)]
    public void SetPaymentAmount_ValidatesAmount_CorrectlyHandles(object amountObj, bool expectedResult)
    {
        // arrange
        var paymentProvider = new PaymentProvider("Test payment");
        decimal amount = Convert.ToDecimal(amountObj);

        // act
        var result = paymentProvider.SetPaymentAmount(amount);

        // assert
        Assert.AreEqual(result, expectedResult);
    }

    [TestMethod]
    public void ProcessPayment_AmountAndMethodSet_ReturnsTrue()
    {
        // arrange
        var provider = new PaymentProvider("Test");
        provider.SetPaymentAmount(50.00m);
        provider.SetPaymentMethod("ideal");

        // act
        var result = provider.ProcessPayment();

        // assert
        Assert.IsTrue(result);
        Assert.IsTrue(provider.PaymentProcessed);
    }

    [TestMethod]
    public void ProcessPayment_OnlyAmountSet_ReturnsFalse()
    {
        // arrange
        var provider = new PaymentProvider("Test");
        provider.SetPaymentAmount(25.00m);

        // act
        var result = provider.ProcessPayment();

        // assert
        Assert.IsFalse(result);
        Assert.IsFalse(provider.PaymentProcessed);
    }

    [TestMethod]
    public void ProcessPayment_NeitherSet_ReturnsFalse()
    {
        // arrange
        var provider = new PaymentProvider("Test");

        // act
        var result = provider.ProcessPayment();

        // assert
        Assert.IsFalse(result);
        Assert.IsFalse(provider.PaymentProcessed);
    }

}