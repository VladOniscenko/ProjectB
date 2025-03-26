public class Customer{
    public int ID;
    public string UserName;
    public string Password;
    public List<Reservation> Reservations = new();

    public Customer(int id, string userName, string password){
        ID = id;
        UserName = userName;
        Password = password;
    }
}