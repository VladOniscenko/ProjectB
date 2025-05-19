using ProjectB.Models;

namespace ProjectB.Logic.Interfaces;

public interface IAuditoriumService
{
    Auditorium? Find(int id);
    
    IEnumerable<Auditorium> GetAllAuditoriums();
    
    bool IsAuditoriumTakenAt(int id, DateTime startTime, DateTime endTime);
}