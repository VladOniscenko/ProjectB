using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;

namespace ProjectB.Logic;

public class AuditoriumLogic : IAuditoriumService
{
    private AuditoriumRepository _auditoriumRepository;

    public AuditoriumLogic(AuditoriumRepository auditoriumRepository)
    {
        _auditoriumRepository = auditoriumRepository;
    }
    
    public Auditorium? Find(int id)
    {
        return _auditoriumRepository.Find(id);
    }
}