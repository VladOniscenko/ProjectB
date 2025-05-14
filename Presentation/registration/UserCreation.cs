using Microsoft.Extensions.DependencyInjection;
using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Presentation;

public class UserCreation
{
    // needs to check if account was already made and all fields are according to standards

    const int BoxX = 20;
    const int Width = 35;

    private readonly IServiceProvider _services;
    private readonly IUserService _userService;

    public UserCreation()
    {
        _services = Program.Services;
        _userService = _services.GetRequiredService<IUserService>();
    }

    public bool CreateUser(User? user = null)
    {
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine("╔══════════════════════╗");
        Console.WriteLine("║   Create New User    ║");
        Console.WriteLine("╚══════════════════════╝");

        User newUser = new User();
        if (user is not null)
        {
            newUser = user;
        }

        newUser.FirstName = BaseUI.DrawInputBox("First name", BoxX, Width, 0, 4, newUser.FirstName);
        while (!_userService.IsNameValid(newUser.FirstName))
        {
            BaseUI.ShowErrorMessage("Name must be longer than 3 characters and can only contain letters", 5);
            newUser.FirstName = BaseUI.DrawInputBox("First name", BoxX, Width, 0, 4, newUser.FirstName);
        }

        Console.SetCursorPosition(0, 5);
        Console.Write("                                                                                     ");

        newUser.LastName = BaseUI.DrawInputBox("Last name", BoxX, Width, 0, 6, newUser.LastName);
        while (!_userService.IsNameValid(newUser.LastName))
        {
            BaseUI.ShowErrorMessage("Name must be longer than 3 characters and can only contain letters", 7);
            newUser.LastName = BaseUI.DrawInputBox("Last name", BoxX, Width, 0, 6, newUser.LastName);
        }

        Console.SetCursorPosition(0, 7);
        Console.Write("                                                                                     ");

        newUser.Email = BaseUI.DrawInputBox("Email", BoxX, Width, 0, 8, newUser.Email);
        while (!_userService.IsEmailValid(newUser.Email) || _userService.DoesUserExist(newUser.Email))
        {
            if (!_userService.IsEmailValid(newUser.Email))
            {
                BaseUI.ShowErrorMessage("Please enter a valid email address", 9);
            }
            else
            {
                BaseUI.ShowErrorMessage("Account with this email already exists", 9);
            }

            newUser.Email = BaseUI.DrawInputBox("Email", BoxX, Width, 0, 8, newUser.Email);
        }

        Console.SetCursorPosition(0, 9);
        Console.Write("                                                                  ");
        string secondPassword = "-";

        while (!_userService.IsPasswordIdentical(newUser.Password, secondPassword))
        {
            newUser.Password = BaseUI.DrawInputBox("Password", BoxX, Width, 0, 10, newUser.Password, true);

            while (!_userService.IsPasswordValid(newUser.Password))
            {
                BaseUI.ShowErrorMessage("Please enter a valid password (must be at least 8 characters long)",
                    11);
                newUser.Password = BaseUI.DrawInputBox("Password", BoxX, Width, 0, 10, newUser.Password, true);
            }

            Console.SetCursorPosition(0, 11);
            Console.Write(
                "                                                                                                   ");

            secondPassword = BaseUI.DrawInputBox("Re-enter password", BoxX, Width, 0, 12, newUser.Password, true);
            if (!_userService.IsPasswordIdentical(newUser.Password, secondPassword))
            {
                BaseUI.ShowErrorMessage("Passwords are not identical",
                    13);
                Console.SetCursorPosition(BoxX, 12);
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine(new string(' ', Width));
                Console.ResetColor();
            }
        }

        if (CheckIfDataCorrect(newUser))
        {
            int? userId = _userService.CreateUser(newUser);
            if (userId is null)
            {
                ConsoleMethods.Error("Something went wrong! Please try again.");
                return false;
            }
            
            newUser.Id = userId.Value;
            Program.CurrentUser = newUser;
            Console.WriteLine("\nYour account has been made!");
            Thread.Sleep(1000);
            return true;
        }
        else
        {
            CreateUser(newUser);
        }

        return false;
    }

    public bool CheckIfDataCorrect(User user)
    {
        Console.Clear();
        Console.WriteLine("You have entered the following information:");
        Console.WriteLine(
            $"    First name:  {user.FirstName}\n    Last name:   {user.LastName} \n    Email:       {user.Email}\n    Password:    {new string('*', user.Password.Length)}");
        Console.WriteLine("\n   Is this correct?");
        Console.Write("    ");
        return BaseUI.BasicYesOrNo();
    }
}