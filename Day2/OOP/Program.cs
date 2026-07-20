

using System;

namespace OOP
{
    class Program
    {
        static void Main(string[] args)
        {
            Car myCar = new Car();
            myCar.Model = "Toyota";
            myCar.Year = 2020;
            myCar.Honk();
        }
    }
    
    class Car
    {
        public string Model;
        public int Year;

        //methods

        public void Honk()
        {
            Console.WriteLine($"{Model} says:Beep Beep");
        }
    }
    
}