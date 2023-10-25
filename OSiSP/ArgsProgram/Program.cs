
using System.Collections.Concurrent;

public class Program
{
    static void Main(string[] args)
    {

        string input = Console.ReadLine();

        int count = input.Split().Count();
        //input.Split();
        Console.WriteLine($"Count {count}");

        string str = "очень";

        for(int i = 0; i < count; i++) {
            str += "-очень";
        }

        Console.WriteLine("\n\n\n");
        Console.WriteLine(str);
    }
}