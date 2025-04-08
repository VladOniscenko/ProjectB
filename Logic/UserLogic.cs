using System.ComponentModel.DataAnnotations;
using ProjectB.DataAccess;


public static class UserLogic{

    private static EmailAddressAttribute email = new EmailAddressAttribute();

    public static bool ValidateName(string name){
        return name.Length < 3 || name.Any(c => !char.IsLetter(c));
    }

    public static bool ValidatePassword(string password){
        return password.Length < 4 || !password.Any(c => !char.IsLetterOrDigit(c)) ||
        !password.Any(char.IsUpper) || !password.Any(char.IsDigit);
    }

    public static bool VerifyEmailFormat(string newEmail){
        return !email.IsValid(newEmail);
    }

    public static bool VerifyThatUserDoesNotExist(string newEmail){
        return UserRepository.CheckIfUserExists(newEmail);
    }


}