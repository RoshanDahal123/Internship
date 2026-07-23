
using System.Reflection.Metadata.Ecma335;

namespace MathCalculations;

public class MathServices
{
    public Action<Double> MathPerformed;


    public void CalculateNumbers ( double value1 , double value2 ,Func<double,double, double> calculation)
    {
       
        MathPerformed(calculation(value1, value2));
    }
}