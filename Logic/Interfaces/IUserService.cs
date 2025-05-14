using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IUserService
{
    bool IsNameValid(string name);
    bool IsPasswordValid(string password);
    bool IsEmailValid(string email);
    bool DoesUserExist(string email);
    int? CreateUser(User user);
    User? Authenticate(string email, string password);
    bool IsPasswordIdentical(string password, string passwordHash);
}