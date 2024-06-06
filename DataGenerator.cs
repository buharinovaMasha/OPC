namespace OPC.Models;

public class DataGenerator
{
    private static Random _random = new Random();

    public static double GenerateRandomNumber(double minimum, double maximum)
    {
        return _random.NextDouble() * (maximum - minimum) + minimum;
    }
}