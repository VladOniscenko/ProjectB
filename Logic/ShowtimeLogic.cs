using ProjectB.DataAccess;
using ProjectB.Logic.Interfaces;
using ProjectB.Models;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ProjectB.Logic;

public class ShowtimeLogic : IShowtimeService
{
    public DateTime parsedStartTime { get; set; }
    private readonly ShowtimeRepository _showtimeRepository;
    public ShowtimeLogic(ShowtimeRepository showtimeRepository)
    {
        _showtimeRepository = showtimeRepository;
    }
    
    public Showtime? Find(int id)
    {
        return _showtimeRepository.Find(id);
    }

    public bool IsMovieIDValid(string movie)
    {   
        if (movie == "")
        {
            return false;
        }
        return true;
    }

    public bool IsMovieStartTimeValid(string date)
    {
        if (string.IsNullOrWhiteSpace(date))
        {
            return false;
        }

        if (!DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out DateTime result))
        {
            return false;
        }

        var now = DateTime.Now;
        if (result <= now.AddMinutes(-5))
        {
            return false;
        }

        parsedStartTime = result;
        return true;
    }

    public IEnumerable<Showtime> GetShowtimesByMovieId(int movieId, int limit = 10)
    {
        return _showtimeRepository.GetShowtimesByMovieId(movieId, limit);
    }

    public void CreateShowtime(Showtime showtime)
    {
        _showtimeRepository.AddShowtime(showtime);
    }
}