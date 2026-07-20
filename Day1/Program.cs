
//using System;

//namespace MyApplication
//{
//    class Program
//    {
//        static int myNum = 5;
//        static double myDoubleNum = 5.99D;
//        static char myLetter = 'D';
//        static bool myBool = true;
//        static string myText = "Hello";
//        static long myLongNumber = 15000000000L;// note it is used when integer 
//                                                // is not enought to hold the number  
//                                                //note that the end of the value must be end with L
//        static float myFloat = 19.99F;
//        static float f1 = 35e3F;  // A floating point number can
//        static double defaultDouble = 4.2e2D;
//        // also be a scientific number with an "e" to indicate the power of 10:
//        static float scientificFloat = 1.3445E-2f; 
//        static void Main(string[] args)
//        {   
//            Console.Write("Hello World! ");
//            Console.Write("I will print on the same hello looeo line.");
//            Console.WriteLine("\nEnter your name");
//            string a = Console.ReadLine();
//            Console.WriteLine($"Hello I am {a}");
//            Console.WriteLine(myNum);
//            Console.WriteLine($"this is the doublenumber {myDoubleNum} a letter{myLetter}" +
//                $" the text is boolean {myBool} and the String {myText}");
//            Console.WriteLine($"This is the LOng number{myLongNumber} and this is the float" +
//                $"NUmber {myFloat}  and the float is written with scientific{f1}");
//            Console.WriteLine(defaultDouble);
//            Console.ReadLine();
//        }
//    }
//}

/* implicit type casting*/

//int myInt = 9;
//double myDouble = myInt;

//Console.WriteLine(myInt);  
//Console.WriteLine(myDouble);
//Console.ReadLine();



/*explicit type casting*/

//double myDouble = 9.78;
//int myInt = (int)myDouble;

//Console.WriteLine(myDouble);   // Outputs 9.78
//Console.WriteLine(myInt);

//Console.ReadLine();



/*Type Conversion Methods*/

//int myInt = 10;
//double myDouble = 5.25;
//bool myBool = true;

//Console.WriteLine(Convert.ToString(myInt));//convert int to string

//Console.WriteLine(Convert.ToDouble(myInt)); //Convert int to double

//Console.WriteLine(Convert.ToInt32(myDouble));//Convert double to int

//Console.WriteLine(Convert.ToString(myBool));//Convert bool to string
//Console.ReadLine();


/*OPERATORS*/


//int x = 5;
//x <<= 3;

//Console.WriteLine(x);

//Console.ReadLine();


//String Interpolation

//string firstName = "John";
//string lastName = "Doe";
//string name = $"My full name is: {firstName} {lastName}";
//Console.WriteLine(name);

////Access of string

//string myString = "Hello World";
//Console.WriteLine(myString[0]);
////Output: H


//string b = myString.Substring(myString.IndexOf("W"));

//Console.WriteLine(b);
//Console.ReadLine();


using System;
using System.Net.Http.Headers;
using System.Security.AccessControl;
namespace MyFirstProgram
{
    class Program
    {
        static void Main(string[] args)
        {


            // ###Classic Switch Case ###
            //    int day = 3;
            //    string dayName;

            //    switch (day)
            //    {
            //        case 1:
            //            dayName = "Monday";
            //            break;
            //        case 2:
            //            dayName = "Tuesday";
            //            break;
            //        case 3:
            //            dayName = "Wednesday";
            //            break;
            //        default:
            //            dayName = "Unknown";
            //            break;
            //    }

            //    Console.WriteLine(dayName); // Wednesday
            //    Console.ReadKey();
            //}

            //Multiple cases sharing the same code

            //int month = 4;
            //string season;

            //switch (month)
            //{
            //    case 12:
            //    case 1:
            //    case 2:
            //        season = "Winter";
            //        break;
            //    case 3:
            //    case 4:
            //    case 5:
            //        season = "Spring";
            //        break;
            //    default:
            //        season = "Other";
            //        break;
            //}

            //Console.WriteLine(season);
            //Console.ReadKey();// Spring


            //Switch Expressions

            //    Console.WriteLine("Enter a number between 1 and 7");
            //    int day = Convert.ToInt32(Console.ReadLine());

            //    string dayName= day switch
            //    {
            //        1 => "Monday",
            //        2 => "Tuesday",
            //        3 => "Wednesday",
            //        4 => "Thursday",
            //        5 => "Friday",
            //        6 => "Saturday",
            //        7 => "Sunday",
            //        _ => "Unknown"
            //    };
            //    Console.WriteLine(dayName);
            //    Console.ReadKey();
            //}

            //    //pattern mathching in switch 

            //    object obj = 42;
            //    string result = obj switch
            //    {
            //        int i => $"Integer:{i}",
            //        string s => $"String:{s}",
            //        _ => "Unknown type"
            //    };
            //    Console.WriteLine(result);
            //    Console.ReadKey();

            //Switch with conditions(when)
            //int score = 90;
            //string grade = score switch
            //{
            //    int n when (n >= 90) => "A",
            //    int n when (n >= 80) => "B",
            //    int n when (n >= 60) => "C",
            //    _ when score < 60 => "F",
            //    _ => "Invalid score"

            //};
            //Console.WriteLine(grade);
            //Console.ReadKey();

            //looping in C#
            //Console.WriteLine("Enter your name");
            //string name = Console.ReadLine();

            //while (1==1)
            //{
            //    Console.WriteLine(" I am in an infinite loop");
            //    Console.ReadKey();
            //}
            //Console.WriteLine("HELLO " + name);
            //Console.ReadKey();


            //String name = "";
            // while(name == "")
            // {
            //     Console.WriteLine("Enter your name");
            //     name = Console.ReadLine();
            // }
            // Console.WriteLine("HELLO " + name);
            // Console.ReadKey();

            //for loops 

            //for(int i = 0;i < 10; i++)
            //{
            //    Console.WriteLine("I am in a for loop"+ i);
            //}
            //Console.WriteLine("Happy birthday");
            //Console.ReadKey();

            //nested for loops

            //for (int i =1; i<=5; i++)
            //{
            //    for (int j =1; j<= 5; j++)
            //    {
            //        Console.Write("* ");
            //    }
            //    Console.WriteLine();
            //}

            //Guessing number system


            //Random random = new Random();
            //bool playAgain = true;
            //int min = 1;
            //int max = 100;
            //int number = 0;
            //while (playAgain)
            //{

            //    int guess = 0;
            //    number = random.Next(min, max + 1);
            //    while (guess != number)
            //    {
            //        Console.WriteLine("Guess the secret number between 1 to 100");
            //        guess = Convert.ToInt32(Console.ReadLine());
            //        Console.WriteLine("You guessed: " + guess);
            //        if (guess == number)
            //        {
            //            Console.WriteLine("You guessed the secret number!");
            //            Console.WriteLine("Hurray!!!!!!");
            //            playAgain = false;

            //        }
            //        else if (guess < number)
            //        {
            //            Console.WriteLine("Your guess is too low.");
            //        }
            //        else
            //        {
            //            Console.WriteLine("Your guess is too high.");
            //        }
            //    }

            //    Console.WriteLine("Do you want to play again? (y/n)"); 
            //    string response= Console.ReadLine();
            //    if (response.ToLower() == "y")
            //    {
            //        playAgain = true;
            //    }
            //    else
            //    {
            //        playAgain = false;
            //    }
            //}


            //Make a calculator program

            //do{
            // Console.WriteLine("Enter your first number:a");
            // int a = Convert.ToInt32(Console.ReadLine());
            // Console.WriteLine("Enter your second number:b");
            // int b = Convert.ToInt32(Console.ReadLine());

            // Console.WriteLine("Enter the operation you want to perform (+, -, *, /)");
            // char Operator = Convert.ToChar(Console.ReadKey().KeyChar);

            //     string Result = Operator switch
            //     {
            //         '+' => (a + b).ToString(),
            //         '-' => (a - b).ToString(),
            //         '*' => (a * b).ToString(),
            //         '/' => (a / b).ToString(),
            //         _ => "Invalid operation"
            //     };
            //     Console.WriteLine();
            //     Console.WriteLine("Result: " + Result);
            //     Console.WriteLine("Do you want to perform another operation? (y/n)");

            // }while (Console.ReadLine().ToLower() == "y") ;


            //arrays

            //String[] cars = { "Volvo", "BMW", "Ford", "Mazda" };
            //for(int i=0;i<cars.Length;i++)
            //{
            //    Console.WriteLine(cars[i]);
            //}
            //Console.WriteLine(cars);


            //String[] cars = new string[3];
            //cars[0] = "Volvo";
            //   Console.WriteLine(cars[0]);

            //foreach(string car in cars)
            //{
            //    Console.WriteLine(car);
            //}

            //Console.ReadKey();

            //methods and functions overloading in c#

            //double a, b, c;
            //a=5; b=5; c=5;
            //double result1 = Multiply(a, b);
            //double result2 = Multiply(a, b,c);
            //Console.WriteLine($"{result1}, {result2}");

            //double total =Checkout(10.5, 20.75, 15.25);
            //Console.WriteLine(total);

            //double total2 = Checkout(5.99, 12.49);
            //Console.WriteLine(total2);
            //Console.ReadKey();
        }
        //static double Multiply(double a, double b)
        //{
        //    return a * b;
        //}

        //static double Multiply(double a, double b, double c)
        //{
        //    return a * b * c;
        //}
        //using params keyword to accept variable number of arguments
        //static double Checkout(params double[] prices)
        //{
        //    double total = 0;
        //    foreach (double price in prices)
        //    {
        //        total += price;

        //    }
        //    return total;
        //}
    }

}