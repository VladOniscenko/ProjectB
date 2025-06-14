namespace ProjectB.Models;

public abstract class BaseModel
{
    public int Id { get; set; }
    public abstract override string ToString();
}