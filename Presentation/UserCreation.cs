using ProjectB.DataAccess;
using ProjectB.Models.Users;
using System.ComponentModel.DataAnnotations;

public static class UserCreation{
    // needs to check if account was already made and all fields are according to standards

    public static void DrawInputBox(int x, int y, int width, string label){
        Console.SetCursorPosition(x, y);
        Console.Write(label + ": ");
        Console.SetCursorPosition(x + label.Length + 2, y);
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write(new string(' ', width));
        Console.SetCursorPosition(x + label.Length + 2, y);
    }

 


    public static void CreateUser(){
        Console.Clear();
        UserRepository userRepository =  new UserRepository();
        Console.WriteLine("╔══════════════════════╗");
        Console.WriteLine("║   Create New User    ║");
        Console.WriteLine("╚══════════════════════╝");


        User newUser = new User();
        var email = new EmailAddressAttribute();

        newUser.FirstName = "";
        while(newUser.FirstName.Length < 3){
            Console.WriteLine("Please enter your first name");
            newUser.FirstName = Console.ReadLine();
        }

        newUser.LastName = "";
        while(newUser.LastName.Length < 3){
            Console.WriteLine("Please enter your last name");
            newUser.LastName = Console.ReadLine();
        }

        newUser.Email = "";
        while(!email.IsValid(newUser.Email) && CheckIfUserExists(newUser.Email, userRepository)){
            Console.WriteLine("Please enter a valid email adress");
            newUser.Email = Console.ReadLine();
            if(!CheckIfUserExists(newUser.Email, userRepository)){
                Console.WriteLine("An account with this email already exists");
            }
        }


        newUser.Password = "";
        while(newUser.Password.Length < 4 || newUser.Password.Length > 15 ||
        !newUser.Password.Any(char.IsUpper) || !newUser.Password.Any(char.IsDigit)){
            Console.WriteLine("Please enter a valid password (must contain 1 capital letter, 1 number and 1 symbol)");
            newUser.Password = Console.ReadLine();
        }

        newUser.IsAdmin = false;
        // var defaultColor1 = Console.ForegroundColor;
        // var defaultColor2 = Console.BackgroundColor;
        // Console.BackgroundColor = defaultColor1;
        // Console.ForegroundColor = defaultColor2;


        userRepository.AddUser(newUser);
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

