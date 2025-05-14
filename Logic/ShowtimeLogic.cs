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
        // DateTime.TryParseExact(input, date format, ?, ?, bool output)
        // CultureInfo uses a fixed format (so not the system's local date format)
        // DateTimeStyles refers to different ways of parsing input. .None means parse the exact output
        // result is the variable where the successfully parsed date gets stored
        bool dateCheck = DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);

        if (string.IsNullOrWhiteSpace(date))
        {
            return false;
        }

        if (!dateCheck)
        {
            return false;
        }

        if (result < DateTime.Now)
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