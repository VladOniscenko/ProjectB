using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IAuditoriumService
{
    Auditorium? Find(int id);
}