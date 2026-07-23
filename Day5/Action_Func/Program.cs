
using MathCalculations;
 class Program
{
    static void Main(string[] args)
    {
        MathServices mathserv = new MathServices();
        mathserv.MathPerformed += (result) =>
        Console.WriteLine("Calculation Result :" + result);

        mathserv.CalculateNumbers(57.85, 783.76, (value1, value2) => value1 * value2 );
    }
}