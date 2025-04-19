using System.Reflection.Emit;
using ProjectB.DataAccess;
using ProjectB.Logic;
using ProjectB.Models;

namespace ProjectB.Presentation;

public static class UserLogin
{
    public static void Login()
    {
        Console.WriteLine("╔══════════════════════════╗");
        Console.WriteLine("║ Enter your login details ║");
        Console.WriteLine("╚══════════════════════════╝");
        bool succesLog = false; 
        while (!succesLog)
        {
            string UserEmail = BaseUI.DrawInputBox("Email",15,30,0,4);
            while (UserLogic.IsEmailFoundAndCorrect(UserEmail))
            {
                BaseUI.ShowErrorMessage("User not found or incorrect", 5);
                UserEmail = BaseUI.DrawInputBox("Email",15,30,0,4);
            }

            Console.SetCursorPosition(0,5);
            Console.Write("                                                                                     ");

            string UserPassword = BaseUI.DrawInputBox("Password",15,30,0,6,null,true);
            while (!UserLogic.IsPasswordFoundAndCorrect(UserEmail,UserPassword))
            {
                BaseUI.ShowErrorMessage("User not found or incorrect", 7);
                UserPassword = BaseUI.DrawInputBox("Password",15,30,0,6,null,true);
            }

            Console.SetCursorPosition(0,7);
            Console.Write("                                                                                     ");

            BaseUI.ConfirmingMessage("You succesfully logged in(press any key to continue)",9);
            Console.ReadKey();
            succesLog = true;

            
        }
        return;
    }
        
}