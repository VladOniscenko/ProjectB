using System.ComponentModel.DataAnnotations;
using ProjectB.DataAccess;
using ProjectB.Models;


public static class UserLogic{

    private static EmailAddressAttribute email = new EmailAddressAttribute();

    public static bool IsNameValid(string name){
        return !(name.Length < 3 || name.Any(c => !char.IsLetter(c)));
    }

    public static bool IsPasswordValid(string password){
        return !(password.Length < 4 || !password.Any(c => !char.IsLetterOrDigit(c)) ||
        !password.Any(char.IsUpper) || !password.Any(char.IsDigit));
    }

    public static bool IsEmailValid(string newEmail){
        return email.IsValid(newEmail);
    }

    public static bool DoesUserExist(string newEmail){
        return UserRepository.CheckIfUserExistByEmail(newEmail);
    }

    public static void CreateUser(User user)
    {
        UserRepository userRepository = new UserRepository();
        userRepository.AddUser(user);
    }
}