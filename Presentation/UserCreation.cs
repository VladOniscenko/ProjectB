using ProjectB.DataAccess;
using ProjectB.Models.Users;
using System.ComponentModel.DataAnnotations;

public static class UserCreation{
    // needs to check if account was already made and all fields are according to standards
    const int boxX = 15;
    const int width = 25;


    public static string DrawInputBox(int x, int y, string label, bool isPassword = false){
        Console.SetCursorPosition(x, y);
        Console.Write(label + ": ");
        Console.SetCursorPosition(boxX, y);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(new string(' ', width));
        Console.SetCursorPosition(boxX, y);
        return ReadInputBox(boxX, y, width, isPassword);
    }

    public static string ReadInputBox(int boxX, int Y, int boxLength, bool isPassword){
        string input = "";

        while(true){
            var key = Console.ReadKey(intercept: true);

            if(key.Key == ConsoleKey.Enter){
                Console.SetCursorPosition(boxX + input.Length, Y);
                break;
            }

            if (key.Key == ConsoleKey.Backspace && input.Length > 0){
                input = input.Remove(input.Length - 1);
                Console.SetCursorPosition(boxX + input.Length, Y);
                Console.Write(" ");
                Console.SetCursorPosition(boxX + input.Length, Y);
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

 


    public static void CreateUser(){
        Console.CursorVisible = false;
        Console.Clear();
        UserRepository userRepository =  new UserRepository();
        Console.WriteLine("╔══════════════════════╗");
        Console.WriteLine("║   Create New User    ║");
        Console.WriteLine("╚══════════════════════╝");


        User newUser = new User();
        var email = new EmailAddressAttribute();


        newUser.FirstName = DrawInputBox(0, 4, "First name");
        while(newUser.FirstName.Length < 3 || newUser.FirstName.Any(c => !char.IsLetter(c))){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 5);
            Console.Write("    Name must be longer than 3 characters and can only contain letters");
            Console.ResetColor();
            newUser.FirstName = DrawInputBox(0, 4, "First name");
        }
        Console.SetCursorPosition(0, 5);
        Console.Write("                                                                                     ");


        newUser.LastName = DrawInputBox(0, 5, "Last name");
        while(newUser.LastName.Length < 3 || newUser.LastName.Any(c => !char.IsLetter(c))){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 6);
            Console.Write("    Name must be longer than 3 characters and can only contain letters");
            Console.ResetColor();
            newUser.LastName = DrawInputBox(0, 5, "Last name");
        }
        Console.SetCursorPosition(0, 6);
        Console.Write("                                                                                     ");


        newUser.Email = DrawInputBox(0, 6, "Email");
        bool doesUserExist = CheckIfUserExists(newUser.Email, userRepository);
        while(!email.IsValid(newUser.Email) || !doesUserExist){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 7);
            if(!CheckIfUserExists(newUser.Email, userRepository)){
                Console.Write("    Account with this email already exists");
            }
            else{
                Console.Write("    Please enter a valid email address");
            }
            Console.ResetColor();
            newUser.Email = DrawInputBox(0, 6, "Email");
            doesUserExist = CheckIfUserExists(newUser.Email, userRepository);
        }
        Console.SetCursorPosition(0, 7);
        Console.Write("                                                                  ");


        newUser.Password = DrawInputBox(0, 7, "Password", true);
        while(newUser.Password.Length < 4 || !newUser.Password.Any(c => !char.IsLetterOrDigit(c)) ||
        !newUser.Password.Any(char.IsUpper) || !newUser.Password.Any(char.IsDigit)){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(0, 8);
            Console.Write("    Please enter a valid password (must contain an uppercase letter, a number and symbol)");
            Console.ResetColor();
            newUser.Password = DrawInputBox(0, 7, "Password", true);
        }

        newUser.IsAdmin = false;
        if(CheckIfDataCorrect(newUser)){
            // userRepository.AddUser(newUser);
            Console.WriteLine("\nYour account has been made!");
        }
        else{
            CreateUser();
        }
    }

    public static bool CheckIfDataCorrect(User user){
        bool isAccepted = true;

        Console.Clear();
        Console.WriteLine("You have entered the following information:");
        Console.WriteLine($"    First name: {user.FirstName}\n    Last name:  {user.LastName} \n    Email:      {user.Email}\n    Password:   {new string('*', user.Password.Length)}");
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

    public static bool CheckIfUserExists(string email, UserRepository userRepository){
        foreach(User user in userRepository.GetAllUsers()){
            if(user.Email == email){
                return false;
            }
        }
        return true;

}

}

