using System.Text;
using ProjectB.Logic;

namespace ProjectB.Presentation;

public class PaymentProviderFlow
{
    private readonly string _title;
    private readonly decimal _priceToPay;
    private string _paymentMethod;
    private readonly PaymentProvider _paymentProvider;
    
    public PaymentProviderFlow(string title, decimal priceToPay)
    {
        _title = title;
        _priceToPay = priceToPay;
        _paymentProvider = new(title);
    }
    
    public string? Run()
    {
        try
        {
            if (!_paymentProvider.SetPaymentAmount(_priceToPay))
            {
                ConsoleMethods.Error("Failed to set payment amount. Operation aborted.");
                ConsoleMethods.AnimateLoadingText("Redirecting back to the reservation");
                return null;
            }

            Dictionary<string, string> paymentMethods = _paymentProvider.GetPaymentMethods();
            paymentMethods.Add("CR", "Cancel");
            _paymentMethod = Menu.SelectMenu($"{DisplayPaymentInformation()} \nSelect payment method", paymentMethods);
            ConsoleMethods.AnimateLoadingText("Checking if amount is received");

            if (_paymentMethod == "CR")
            {
                ConsoleMethods.AnimateLoadingText("Payment cancelled redirecting back to the reservation");
                return null;
            }

            if (!_paymentProvider.SetPaymentMethod(_paymentMethod))
            {
                ConsoleMethods.Error("Invalid payment method selected. Operation aborted.");
                ConsoleMethods.AnimateLoadingText("Redirecting back to the reservation");
                return null;
            }

            if (_paymentProvider.ProcessPayment())
            {
                ConsoleMethods.Success("Payment processed successfully!");
                ConsoleMethods.AnimateLoadingText("Redirecting back to the reservation");
                return _paymentProvider.Method;
            }
            
            ConsoleMethods.Error("Payment failed to process.");
        }
        catch (Exception ex)
        {
            ConsoleMethods.Error($"An unexpected error occurred: {ex.Message}");
        }
        
        ConsoleMethods.AnimateLoadingText("Redirecting back to the reservation");
        return null;
    }
    
    public string DisplayPaymentInformation()
    {
        var sb = new StringBuilder();
        string titleLine = $"{_title}";
        string amountLine = $"Amount to Pay: €{_priceToPay:0.00}";

        int boxWidth = 70;
        string horizontalLine = new string('═', boxWidth);

        sb.AppendLine($"╔{horizontalLine}╗");
        sb.AppendLine(ConsoleMethods.CenterTextInBox("Payment Information", boxWidth));
        sb.AppendLine($"╠{horizontalLine}╣");
        sb.AppendLine(ConsoleMethods.CenterTextInBox(titleLine, boxWidth));
        sb.AppendLine(ConsoleMethods.CenterTextInBox(amountLine, boxWidth));
        sb.AppendLine($"╚{horizontalLine}╝");

        return sb.ToString();
    }
}