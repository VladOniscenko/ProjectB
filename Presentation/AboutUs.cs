namespace ProjectB.Presentation;

public class AboutUs
{
    public AboutUs()
    {
        
    }

    public void Run()
    {
        Console.Clear();
        Console.WriteLine("=== About Us ===");

        Console.WriteLine("Welcome to Byte Cinemas, where every visit is more than just a trip to" + 
                          "\nthe movies—it’s an immersive cinematic experience. Founded by Jake Darcy,"+
                           "\na passionate entrepreneur and film lover, our mission is to provide the most comfortable" + 
                           "\nmovie-going experience possible. \n \n");

        Console.WriteLine("Byte Cinemas, currently located at Wijnhaven 107 3011 WN, features state-of-the-art technology" + 
                          "\nwith Auro 3D sound and IMAX 3D digital prjectors. Along with comfortable seating options" +
                          "\ntailored to your comfort, you can choose between standard, premium and VIP. Furthermore it" +
                          "\nhas a stylish lounge area and bar, perfect for unwinding before or after your film. \n\n");

        Console.WriteLine("Whether you're a casual moviegoer or a certified cinephile, Byte Cinemas aims \n" +
                          "to let you enjoy movies fully, the right way.");
        
        Console.ReadKey();
    }
}
