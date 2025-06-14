namespace ProjectB.Models;

public class User : BaseModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
    
    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}