using System.Security.Cryptography.X509Certificates;
using ProjectB.DataAccess;
using ProjectB.Models;
using ProjectB.Presentation;
using Microsoft.Extensions.DependencyInjection;

public class MakeAccountAdmin{

    private readonly IServiceProvider _services;
    private readonly UserLogic _userLogicService;

    public MakeAccountAdmin(){
        _services = Program.Services;
        _userLogicService = _services.GetRequiredService<UserLogic>();
    }


    public void ChooseAccount(){
        Console.Clear();
        List<User> AllUsers = _userLogicService.GetAllNonAdminUsers();


        if(_userLogicService.CheckIfUserListIsEmpty(AllUsers)){
            ConsoleMethods.Error("No non admin users exist");
            return;
        }
        Console.WriteLine("Select which user you would like to give admin access:");
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
                    Console.Clear();
                    Console.WriteLine($"are you sure you wish to make {AllUsers[selectedIndex].FirstName} {AllUsers[selectedIndex].LastName} an admin?");
                    if(BaseUI.BasicYesOrNo()){
                        _userLogicService.MakeUserAdmin(AllUsers[selectedIndex].Email);
                        ConsoleMethods.AnimateLoadingText("Making user admin", 2000);
                        Console.WriteLine($"{AllUsers[selectedIndex].FirstName} {AllUsers[selectedIndex].LastName} has been given admin rights!");
                        ConsoleMethods.AwaitUser();
                        return;
                    }
                    Console.Clear();
                    Console.WriteLine("Select which user you would like to give admin access:");
                    ShowUsers(AllUsers);
                    break;



            }


        }

    }

    public void ShowUsers(List<User> Users){
        for(int i = 0; i < Users.Count(); i++){
            string fullName = Users[i].FirstName + " " + Users[i].LastName;
            string email = Users[i].Email;
            string row = fullName.PadRight(20) + email.PadRight(18);

            Console.SetCursorPosition(4, i + 2);
            Console.Write(row);
        }
    }

    public void UpdateLine(User user, int index, bool isSelected){
        int consoleWidth = Console.WindowWidth;

        if(isSelected){
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        string fullName = user.FirstName + " " + user.LastName;
        string email = user.Email;
        string row = fullName.PadRight(20) + email.PadRight(18);

        Console.SetCursorPosition(4, index + 2);
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