//a program to learn about the run time polymerphism
//learning about abstraction and interface

using Revision.Models;
List<Shape> shapes = new List<Shape>
{
    new Circle(5),
    new Rectangle(4,5)
};

foreach (var shape in shapes)
{
    Console.WriteLine(shape.Describe());
}

