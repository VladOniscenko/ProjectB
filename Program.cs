using ProjectB.DataAccess;

namespace ProjectB
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
            DbInitializer.Initialize();
        }

    }
}