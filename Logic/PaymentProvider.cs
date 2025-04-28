using ProjectB.Logic.Interfaces;

namespace ProjectB.Logic;

public class PaymentProvider : IPaymentProvider
{
    public decimal Amount { get; private set; }
    public string Title { get; private set; }
    public string Method { get; private set; }
    public bool PaymentProcessed { get; private set; }

    public PaymentProvider(string title)
    {
        PaymentProcessed = false;
        Title = title;
    }
    
    public bool SetPaymentAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        Amount = amount;
        return true;
    }

    public bool SetPaymentMethod(string method)
    {

        if (!GetPaymentMethods().ContainsKey(method))
        {
            throw new IndexOutOfRangeException("Payment method not found.");
        }
        
        Method = method;
        return true;
    }
    
    public bool ProcessPayment()
    {
        PaymentProcessed = true;
        return PaymentProcessed;
    }
    
    public Dictionary<string, string> GetPaymentMethods()
    {
        return new Dictionary<string, string>()
        {
            { "ideal", "iDEAL" },
            { "paypal", "PayPal" },
            { "creditcard", "Credit Card (Visa, Mastercard, Amex)" },
            { "applepay", "Apple Pay" },
            { "googlepay", "Google Pay" },
            { "klarna", "Klarna (Achteraf betalen)" },
            { "bancontact", "Bancontact" },
            { "sepa", "SEPA Bank Transfer" },
            { "sofort", "Sofort (vaak gebruikt door Nederlanders met Duitse rekeningen)" },
            { "tikkie", "Tikkie (voor informele betalingen)" },
            { "bitcoin", "Bitcoin (via BitPay en consorten)" }
        };
    }
}