using ProjectB.DataAccess;
using ProjectB.Models;

namespace ProjectB.Presentation;

public static class UserCreation
{
    // needs to check if account was already made and all fields are according to standards
    const int BoxX = 15;
    const int Width = 30;

    public static void CreateUser(User? user = null)
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
        while (!UserLogic.IsNameValid(newUser.FirstName))
        {
            BaseUI.ShowErrorMessage("Name must be longer than 3 characters and can only contain letters", 5);
            newUser.FirstName = BaseUI.DrawInputBox("First name", BoxX, Width, 0, 4, newUser.FirstName);
        }

        Console.SetCursorPosition(0, 5);
        Console.Write("                                                                                     ");

        newUser.LastName = BaseUI.DrawInputBox("Last name", BoxX, Width, 0, 6, newUser.LastName);
        while (!UserLogic.IsNameValid(newUser.LastName))
        {
            BaseUI.ShowErrorMessage( "Name must be longer than 3 characters and can only contain letters", 7);
            newUser.LastName = BaseUI.DrawInputBox("Last name", BoxX, Width, 0, 6, newUser.LastName);
        }

        Console.SetCursorPosition(0, 7);
        Console.Write("                                                                                     ");

        newUser.Email = BaseUI.DrawInputBox("Email", BoxX, Width, 0, 8, newUser.Email);
        while (!UserLogic.IsEmailValid(newUser.Email) || UserLogic.DoesUserExist(newUser.Email))
        {
            if (!UserLogic.IsEmailValid(newUser.Email))
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

        newUser.Password = BaseUI.DrawInputBox("Password", BoxX, Width, 0, 10, newUser.Password, true);
        while (!UserLogic.IsPasswordValid(newUser.Password))
        {
            BaseUI.ShowErrorMessage("Please enter a valid password (must contain an uppercase letter, a number and symbol)",
                                    11);
            newUser.Password = BaseUI.DrawInputBox("Password", BoxX, Width, 0, 10, newUser.Password, true);
        }

        if (CheckIfDataCorrect(newUser))
        {
            // UserLogic.CreateUser(newUser);
            Console.WriteLine("\nYour account has been made!");
            Thread.Sleep(1000);
        }
        else
        {
            CreateUser(newUser);
        }
    }

    public static bool CheckIfDataCorrect(User user)
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