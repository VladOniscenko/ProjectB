namespace ProjectB.Models;

public class Seat : IEquatable<Seat>, IComparable<Seat>
{
    public int Id { get; set; }
    public int AuditoriumId { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public int Active { get; set; }
    public string Type { get; set; }
    public int Taken { get; set; } = 0;
    public bool Selected { get; set; }
    public string TicketType { get; set; }
    
    public int CompareTo(Seat? other)
    {
        return Id.CompareTo(other?.Id);
    }
    
    public bool Equals(Seat? other)
    {
        return Id == other?.Id;
    }
    
    public static bool operator ==(Seat? left, Seat? right)
    {
        if (left is null && right is null)
            return true;
        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }
    
    public static bool operator !=(Seat? left, Seat? right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj);
    }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}