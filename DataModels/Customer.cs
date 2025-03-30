namespace ProjectB.DataModels;
public class Customer{
    public int Id;
    public string UserName;
    public string Password;
    public List<Reservation> Reservations = new();

    public Customer(int id, string userName, string password){
        Id = id;
        UserName = userName;
        Password = password;
    }
}