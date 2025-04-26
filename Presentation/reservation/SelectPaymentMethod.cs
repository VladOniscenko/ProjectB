using Microsoft.Extensions.DependencyInjection;
using ProjectB.Logic.Interfaces;

namespace ProjectB.Presentation;

public class SelectPaymentMethod
{
    private readonly IServiceProvider _services;
    private readonly IReservationService _reservationService;
    public SelectPaymentMethod(IServiceProvider services)
    {
        _services = services;
        _reservationService = _services.GetRequiredService<IReservationService>();
    }
    
    public string? Run()
    {
        Dictionary<string, string> paymentMethods = _reservationService.GetPaymentMethods();
        paymentMethods.Add("return", "Previous step");
        
        string selection = Menu.SelectMenu("Select payment method", paymentMethods);
        if (selection == "return") return null;
        
        return selection;        
    }
}