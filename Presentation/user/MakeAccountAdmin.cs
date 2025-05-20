using System.Security.Cryptography.X509Certificates;
using ProjectB.DataAccess;
using ProjectB.Models;

public static class MakeAccountAdmin{

    public static void ChooseAccount(){
        Console.Clear();
        var userLogic = new UserLogic(new UserRepository());
        List<User> AllUsers = userLogic.GetAllUsers();
        ShowUsers(AllUsers);

        int previousIndex = 0;
        int selectedIndex = 0;

        while(true){

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
            }

            UpdateLine(AllUsers[previousIndex], previousIndex, false);
            UpdateLine(AllUsers[selectedIndex], selectedIndex, true);
        }

    }

    public static void ShowUsers(List<User> Users){
        for(int i = 0; i < Users.Count(); i++){
            Console.SetCursorPosition(0,i);
            Console.WriteLine(Users[i].Email);
            Console.SetCursorPosition(18, i);
            Console.WriteLine(Users[i].FirstName + " " + Users[i].LastName);
            Thread.Sleep(1000);
        }
    }

    public static void UpdateLine(User user, int index, bool isSelected){
        if(isSelected){
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

            Console.SetCursorPosition(0,index);
            Console.WriteLine(user.Email);
            Console.SetCursorPosition(18, index);
            Console.WriteLine(user.FirstName + " " + user.LastName);

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