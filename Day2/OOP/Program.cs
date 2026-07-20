/*

using System;

namespace OOP
{
    class Program
    {
        static void Main(string[] args)
        {
            //Car myCar = new Car();
            //myCar.Model = "Toyota";
            //myCar.Year = 2020;
            //myCar.Honk();

            //Car myCar = new Car("Civic", 2022);
            //myCar.Honk();
            //Car newCar = new Car("Corolla", 2021);
            //newCar.Honk();
            //Car newCar1 = new Car();
            //newCar1.Honk();

            try
            {
                Student s = new Student();
                s.Age = -5;
            }
            catch (Exception ex)

            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.WriteLine("Finally block executed.");
            }
        }
    }

    //class Car
    //{
    //    public string Model;
    //    public int Year;

        //methods

        // constructor

        //default constructor
        //    public Car()
        //    {
        //        Model= "Unknown";
        //        Year = 0;
        //        Console.WriteLine($"Car created: {Model}, {Year}"); 
        //    }
        //    public Car(string model, int year)
        //    {
        //        Model = model;
        //        Year = year;
        //        Console.WriteLine($"Car created: {Model}, {Year}");
        //    }

        //    public void Honk()
        //    {
        //        Console.WriteLine($"{Model} says:Beep Beep");
        //    }

        //    //destrructor
        //    ~Car()
        //    {
        //        Console.WriteLine($"Car destroyed: {Model}, {Year}");
        //    }


        class Student
        {
            public string Name { get; set; }
            public string Grade { get; set; }

            private int age;
            public int Age
            {
                get { return age; }
                set
                {
                    if (value < 0)
                        throw new ArgumentException("Age cannot be negative");
                    age = value;
                }
            }
            public void DisplayInfo()
            {
                Console.WriteLine($"Name: {Name}, Grade: {Grade}, Age: {Age}");
            }
        }
    

}

*/



//using OOP;
//using System;
//using System.Xml.Serialization;
//namespace OOP
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Animal[] animals = { new Dog("Rex"), new Cat("Whiskers"), new Animal("Generic") };
//            foreach (Animal animal in animals)
//            {
//                animal.MakeSound();
//            }
//        }
//    }
//    class Animal
//    {
//        public string Name;
//       public Animal(string name)
//        {
//            Name = name;
//        }
//        public virtual void MakeSound()//virtual can be overridden in derived classes
//        {
//            Console.WriteLine($"{Name} makes a sound.");
//        }
//    }
//}

//class Dog:Animal  //Dog inherits from Animal
//{
//    public Dog(string name):base(name) { }//overide the constructor of the base class
//     public override void MakeSound() //override the method of the base class
//    {
//        Console.WriteLine($"{Name} barks: Woof! Woof!");
//    }


//}

//class Cat : Animal
//{
//    public Cat(string name) : base(name) { }

//    public override void MakeSound()
//    {
//        Console.WriteLine($"{Name} meows.");
//    }
//}


/* Inheritance and Polymorphism Example */


using System;

namespace OOP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor= ConsoleColor.Green;
            Shape[] shapes =
            {
                new Rectangle(5, 10),
                new Circle(7)
            };
            foreach (Shape shape in shapes) {

                shape.Area();
               
            }
        }
    }


    class Shape
    {
        public int x;
        public int y;

        public Shape(int x,int y)
        {
            this.x = x;
            this.y = y;
        }

        public virtual void Area()
        {
            Console.WriteLine("Area of the shape is :");
        }
    }

    class Rectangle :Shape
    {
        public Rectangle(int x, int y) : base(x, y)
        {
        }

        public override void Area()
        {
            int area = x * y;
            Console.WriteLine($"Area of the rectangle is: {area}");
        }
    }

    class Circle : Shape
    {
        public int radius;
        public Circle(int radius) :base(0,0)
        {
            this.radius = radius;
        }
       
        public override void Area()
        {
            int area = (int)(Math.PI * radius * radius);
            Console.WriteLine($"Area of the circle is: {area}");
        }
    }
}
