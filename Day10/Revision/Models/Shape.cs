
namespace Revision.Models;

//Abstract class - cannot be instantiated directly(no new Shape()" allowed).

//it exist purely to define shared behaviour + a contract for subclasses

public abstract class Shape
{
    //virtual = subclasses CAN override this , but arent forced to 
    public virtual double CalculateArea()
    {
        return 0;
    }

    public virtual string Describe()
    {
        return $"This Shape has an area of {CalculateArea():F2}";
    }
}

