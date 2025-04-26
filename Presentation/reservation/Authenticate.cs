namespace ProjectB.Presentation;

public class Authenticate
{
    
    private readonly IServiceProvider _services;
    private Dictionary<string, string> _options;
    public Authenticate(IServiceProvider services)
    {
        _services = services;
        _options = new()
        {
            { "LI", "Login" },
            { "RE", "Register" },
            { "RETURN", "Return" }
        };
    }

    public bool Run()
    {
        string selection = Menu.SelectMenu("To make a reservation, please login or register", _options);
        if (!_options.Keys.ToList().Contains(selection))
        {
            ConsoleMethods.Error("Invalid selection");
            return Run();
        }

        switch (selection)
        {
            case "RE":
                return new UserCreation(_services).CreateUser();
            case "LI":
                return new UserLogin(_services).Run();
            default:
                return false;
        }
    }
}