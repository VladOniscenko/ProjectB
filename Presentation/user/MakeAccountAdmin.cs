using System.Security.Cryptography.X509Certificates;
using ProjectB.DataAccess;
using ProjectB.Models;

public static class MakeAccountAdmin{

    public static void ChooseAccount(){
        Console.Clear();
        Console.WriteLine("Select which user you would like to give admin access:");
        var userLogic = new UserLogic(new UserRepository());
        List<User> AllUsers = userLogic.GetAllNonAdminUsers();
        ShowUsers(AllUsers);

        int previousIndex = 0;
        int selectedIndex = 0;

        while(true){
            UpdateLine(AllUsers[previousIndex], previousIndex, false);
            UpdateLine(AllUsers[selectedIndex], selectedIndex, true);

            previousIndex = selectedIndex;
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex == 0) ? AllUsers.Count - 1 : selectedIndex - 1;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex == AllUsers.Count - 1) ? 0 : selectedIndex + 1;
                    break;
                case ConsoleKey.Enter:
                    userLogic.MakeUserAdmin(AllUsers[selectedIndex].Email);
                    Console.Clear();
                    Console.WriteLine($"{AllUsers[selectedIndex].FirstName} + {AllUsers[selectedIndex].LastName} has been given admin rights!");
                    Thread.Sleep(1000);
                    return;
            }


        }

    }

    public static void ShowUsers(List<User> Users){
        for(int i = 0; i < Users.Count(); i++){
            Console.SetCursorPosition(0,i + 1);
            Console.WriteLine(Users[i].FirstName + " " + Users[i].LastName);
            Console.SetCursorPosition(20, i + 1);
            Console.WriteLine(Users[i].Email);
        }
    }

    public static void UpdateLine(User user, int index, bool isSelected){
        int consoleWidth = Console.WindowWidth;

        if(isSelected){
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        string fullName = user.FirstName + " " + user.LastName;
        string email = user.Email;
        string row = fullName.PadRight(20) + email.PadRight(20);

        Console.SetCursorPosition(0, index + 1);
        Console.Write(row);

        Console.ResetColor();

    }


        // key = Console.ReadKey(true).Key;

        //     switch (key)
        //     {
        //         case ConsoleKey.UpArrow:
        //             selected = (selected == 0) ? options.Count - 1 : selected - 1;
        //             break;
        //         case ConsoleKey.DownArrow:
        //             selected = (selected == options.Count - 1) ? 0 : selected + 1;
        //             break;
        //     }

        // // Run while the user has not pressed Enter.
        // } while (key != ConsoleKey.Enter);
    }