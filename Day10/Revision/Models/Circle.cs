

namespace Revision.Models
{
    public class Circle : Shape
    {
        private readonly double _radius;

        public Circle(double radius)
        {
            if (radius <= 0)
                throw new ArgumentException("Radius must be positive", nameof(radius));
            _radius = radius;
        }

        public override double CalculateArea()
        {
            return Math.PI * _radius * _radius;
        }


    }

}
