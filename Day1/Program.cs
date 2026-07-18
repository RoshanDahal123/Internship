
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
            int score = 90;
            string grade = score switch
            {
                int n when (n >= 90) => "A",
                int n when (n >= 80) => "B",
                int n when (n >= 60) => "C",
                _ when score < 60 => "F",
                _ => "Invalid score"

            };
            Console.WriteLine(grade);
            Console.ReadKey();
        }
    }
}