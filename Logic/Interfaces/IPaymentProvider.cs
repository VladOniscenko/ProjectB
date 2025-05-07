namespace ProjectB.Logic.Interfaces;

public interface IPaymentProvider
{
    bool ProcessPayment();
    Dictionary<string, string> GetPaymentMethods();
    bool SetPaymentMethod(string method);
    bool SetPaymentAmount(decimal amount);
}