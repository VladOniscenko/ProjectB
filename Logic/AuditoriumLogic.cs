using ProjectB.DataAccess;
using ProjectB.Models;

namespace ProjectB.Logic;

public class AuditoriumLogic
{
    public static Auditorium? Find(int id)
    {
        AuditoriumRepository auditoriumRepository = new();
        return auditoriumRepository.Find(id);
    }
}