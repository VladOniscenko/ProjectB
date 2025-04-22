using System.ComponentModel.DataAnnotations;
using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;


public class UserLogic : IUserService
{
    private UserRepository _userRepository;
    public UserLogic(UserRepository userRepository) {
        _userRepository = userRepository;
    }

    private EmailAddressAttribute email = new EmailAddressAttribute();

    public bool IsNameValid(string name){
        return !(name.Length < 3 || name.Any(c => !char.IsLetter(c)));
    }

    public bool IsPasswordValid(string password){
        return !(password.Length < 4 || !password.Any(c => !char.IsLetterOrDigit(c)) ||
        !password.Any(char.IsUpper) || !password.Any(char.IsDigit));
    }

    public bool IsEmailValid(string newEmail){
        return email.IsValid(newEmail);
    }

    public bool DoesUserExist(string newEmail){
        return _userRepository.CheckIfUserExistByEmail(newEmail);
    }

    public void CreateUser(User user)
    {
        _userRepository.AddUser(user);
    }
}