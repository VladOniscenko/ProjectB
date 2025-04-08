using ProjectB.DataAccess;
using ProjectB.Models.Users;

namespace ProjectB.Presentation;

public static class UserCreation
{
    // needs to check if account was already made and all fields are according to standards
    const int BoxX = 15;
    const int Width = 30;


    public static string DrawInputBox(int x, int y, string label, string previouslyWritten, bool isPassword = false){
        Console.SetCursorPosition(x, y);
        Console.Write(label + ": ");
        Console.SetCursorPosition(BoxX, y);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(new string(' ', Width));
        Console.SetCursorPosition(BoxX, y);
        return ReadInputBox(BoxX, y, Width, previouslyWritten, isPassword);
    }

    public static string ReadInputBox(int boxX, int y, int boxLength, string previouslyWritten, bool isPassword){
        string input = "";
        if(previouslyWritten is not null && !isPassword){
            Console.SetCursorPosition(boxX, y);
            input = previouslyWritten;
            Console.Write(input);
            Console.SetCursorPosition(boxX + input.Length, y);
        }

        while(true){
            var key = Console.ReadKey(intercept: true);

            if(key.Key == ConsoleKey.Enter){
                Console.SetCursorPosition(boxX + input.Length, y);
                break;
            }

            if (key.Key == ConsoleKey.Backspace && input.Length > 0){
                input = input.Remove(input.Length - 1);
                Console.SetCursorPosition(boxX + input.Length, y);
                Console.Write(" ");
                Console.SetCursorPosition(boxX + input.Length, y);
            }

            else if(!char.IsControl(key.KeyChar) && input.Length < boxLength - 1){
                input += key.KeyChar;
                if(!isPassword){
                    Console.Write(key.KeyChar); 
                }       
                else{
                    Console.Write("*");
                }         
            }
        }
        Console.ResetColor();
        return input;
    }

    public static void ShowErrorMessage(int y, string errorMessage){
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(0, y);
        Console.Write(errorMessage);
        Console.ResetColor();

    }

 


    public static void CreateUser(User? user = null){
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Clear();
        UserRepository userRepository =  new UserRepository();
        Console.WriteLine("╔══════════════════════╗");
        Console.WriteLine("║   Create New User    ║");
        Console.WriteLine("╚══════════════════════╝");

        User newUser = new User();
        if (user is not null){
            newUser = user;
        }

        newUser.FirstName = DrawInputBox(0, 4, "First name", newUser.FirstName);
        while(!UserLogic.IsNameValid(newUser.FirstName)){

            ShowErrorMessage( 5, "    Name must be longer than 3 characters and can only contain letters");

            newUser.FirstName = DrawInputBox(0, 4, "First name", newUser.FirstName);
        }
        Console.SetCursorPosition(0, 5);
        Console.Write("                                                                                     ");


        newUser.LastName = DrawInputBox(0, 6, "Last name", newUser.LastName);

        while(!UserLogic.IsNameValid(newUser.LastName)){

            ShowErrorMessage(7, "    Name must be longer than 3 characters and can only contain letters");

            newUser.LastName = DrawInputBox(0, 6, "Last name", newUser.LastName);
        }
        Console.SetCursorPosition(0, 7);
        Console.Write("                                                                                     ");


        newUser.Email = DrawInputBox(0, 8, "Email", newUser.Email);

        while(!UserLogic.IsEmailValid(newUser.Email) || UserLogic.DoesUserExist(newUser.Email)){

            if(!UserLogic.IsEmailValid(newUser.Email)){
                ShowErrorMessage(9, "    Please enter a valid email address       ");
            }
            else{
                ShowErrorMessage(9, "    Account with this email already exists");
            }

            newUser.Email = DrawInputBox(0, 8, "Email", newUser.Email);
        }
        Console.SetCursorPosition(0, 9);
        Console.Write("                                                                  ");


        newUser.Password = DrawInputBox(0, 10, "Password",newUser.Password, true);
        while(!UserLogic.IsPasswordValid(newUser.Password)){
            ShowErrorMessage(11, "    Please enter a valid password (must contain an uppercase letter, a number and symbol)");
            newUser.Password = DrawInputBox(0, 10, "Password",newUser.Password, true);
        }

        if(CheckIfDataCorrect(newUser)){
            userRepository.AddUser(newUser);
            Console.WriteLine("\nYour account has been made!");
        }
        else{
            CreateUser(newUser);
        }
    }

    public static bool CheckIfDataCorrect(User user){
        bool isAccepted = true;

        Console.Clear();
        Console.WriteLine("You have entered the following information:");
        Console.WriteLine($"    First name:  {user.FirstName}\n    Last name:   {user.LastName} \n    Email:       {user.Email}\n    Password:    {new string('*', user.Password.Length)}");
        Console.WriteLine("\n   Is this correct?");
        Console.Write("    ");
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(" Yes ");
        Console.ResetColor();
        Console.Write("    No ");

        while(true){
            var key = Console.ReadKey(intercept: true);

            if(key.Key == ConsoleKey.LeftArrow){
                isAccepted = true;
                Console.SetCursorPosition(0, 7);
                Console.Write("    ");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" Yes ");
                Console.ResetColor();
                Console.Write("    No   ");

            }

            if(key.Key == ConsoleKey.RightArrow){
                isAccepted = false;
                Console.SetCursorPosition(0, 7);
                Console.Write("    ");
                Console.Write(" Yes    ");
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" No ");
                Console.ResetColor();
            }

            if(key.Key == ConsoleKey.Enter){
                return isAccepted;
            }
        }
    }

}


