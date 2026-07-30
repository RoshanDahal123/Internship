

namespace Revision.Models;

public class Rectangle : Shape
{
    private readonly double _x;
    private readonly double _y;


    public Rectangle(double x, double y)
    {

        if(x < 0 || y < 0)
        {
            throw new ArgumentException("Width and height must be positive");
        }
        _x = x;
        _y = y;
    }

    public override double CalculateArea()
    {
        return _x * _y;
    }

}