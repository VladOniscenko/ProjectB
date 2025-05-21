using Microsoft.Extensions.DependencyInjection;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class UserLogin
{
    private readonly IServiceProvider _services;
    private readonly UserLogic _userService;
    public UserLogin()
    {
        _services = Program.Services;
        _userService = _services.GetRequiredService<UserLogic>();
    }
    
    public bool Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════╗");
            Console.WriteLine("║ Enter your login details ║");
            Console.WriteLine("╚══════════════════════════╝");
            
            string email = BaseUI.DrawInputBox("Email",15,30,0,4);
            Console.SetCursorPosition(0,5);
            Console.Write("                                                                                     ");

            string password = BaseUI.DrawInputBox("Password",15,30,0,6,null,true);
            ConsoleMethods.AnimateLoadingText("Logging in");
            User? user = _userService.Authenticate(email, password);
            if (user == null)
            {
                ConsoleMethods.Error("Email or password incorrect!");
                continue;
            }
            
            ConsoleMethods.Success("You successfully logged in");
            Program.CurrentUser = user;
            return true;
        }
    }
        
}