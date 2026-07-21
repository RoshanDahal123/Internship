
using System;

using System.Threading.Tasks;

namespace C_Sharp_Advanced_Concepts
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("##########################################");
            Console.WriteLine("Welcome to C# Advanced Topics");
            Console.WriteLine("##########################################\n");

            bool keepRunning = true;
            while (keepRunning)
            {
                PrintMenu();
                string input = Console.ReadLine() ?? "";

                if(!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("That's not a valid number.Please try again.\n");
                    continue;//Skip the rest of the loop iteratiion,m show menu again
                }
               
                    switch (choice)
                    {
                    case 1:
                        Collection.Run();
                        break;
                    case 2:
                        GenericsLinqDemo.Run();
                        break;
                    case 3:
                        DelegatesEventsDemo.Run();
                        break;
                    case 4:
                        await AsyncDemo.Run();
                        break;
                    case 5:
                        FileHandlingDemo.Run();
                        break;
                    case 6:
                        Collection.Run();
                        GenericsLinqDemo.Run();
                        DelegatesEventsDemo.Run();
                        await AsyncDemo.Run();
                        //FileHandlingDemo.Run();
                        //Console.WriteLine("All demos finished.\n");
                        break;
                    case 0:
                        keepRunning = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Please enter a number between 0 and 6.\n");
                        break;

                }
               

                Console.WriteLine("AllDemos finished");
            }


        }
        private static void PrintMenu()
        {
            Console.WriteLine("Choose a demo to run:");
            Console.WriteLine("  1. Collections (List, Dictionary, Queue, Stack)");
            Console.WriteLine("  2. Generics and LINQ");
            Console.WriteLine("  3. Delegates, Events, and Lambda Expressions");
            Console.WriteLine("  4. Asynchronous Programming (async/await)");
            Console.WriteLine("  5. File Handling (Reading/Writing files, Streams)");
            Console.WriteLine("  6. Run ALL demos in order");
            Console.WriteLine("  0. Exit");
            Console.Write("Your Choice:");
        }
    }
}