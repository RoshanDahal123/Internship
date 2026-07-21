using System;
using System.Collections.Generic;
using System.Text;

namespace C_Sharp_Advanced_Concepts
{/*
     * COLLECTIONS
     * -----------
     * C# gives you several built-in collection types, each suited to a different
     * access pattern:
     *
     *   List<T>       - resizable array. Fast random access by index. Use when
     *                    you need an ordered, indexable set of items.
     *   Dictionary<K,V> - key/value lookup table (hash map). Fast lookup by key.
     *                    Use when you need to find a value given a unique key.
     *   Queue<T>      - First-In-First-Out (FIFO). Enqueue adds to the back,
     *                    Dequeue removes from the front. Use for task/job queues,
     *                    breadth-first search, etc.
     *   Stack<T>      - Last-In-First-Out (LIFO). Push adds to the top, Pop
     *                    removes from the top. Use for undo history, backtracking,
     *                    expression evaluation, etc.
     */
    public static class Collection
    {
        public static void Run()
        {
            Console.WriteLine("=== 1. COLLECTIONS ===");
            //------List<T>---------
            List<string> fruits = new List<string> { "Apple", "Banana", "Cherry" };
            fruits.Add("Mango");
            fruits.RemoveAt(1);//Removes Banana
            Console.WriteLine("\n-- List<string> --");

            foreach (var fruit in fruits)
            {
                Console.WriteLine($"{fruit}");
            }


            //---Dictionary<K,V>

            Dictionary<string, int> ages = new Dictionary<string, int>
            {
                { "Alice", 30 },
                { "Bob", 25 }
            };

            ages["Charlie"] = 40;//add new key

            ages["Alice"] = 31;//update existing key

            Console.WriteLine("\n-- Dictionary<string,int> --");

            foreach (var kvp in ages)
            {
                Console.WriteLine($" {kvp.Key}->{kvp.Value}");

            }
            if (ages.TryGetValue("Bob", out int bobAge))
                Console.WriteLine($"Found Bob, age {bobAge}");


            //---------Oueue<T>---------

            Queue<string> printJobs = new Queue<string>();
            printJobs.Enqueue("Job1");
            printJobs.Enqueue("Job2");
            printJobs.Enqueue("job3");
            Console.WriteLine("\n-- Queue<string> (FIFO) --");
            while (printJobs.Count > 0)
                Console.WriteLine($"  Processing: {printJobs.Dequeue()}");
            //First in first out means Job1, job2 and job3


            //------------Stack<T>

            Stack<string> browserHistory = new Stack<string>();
            browserHistory.Push("page1.html");
            browserHistory.Push("Page2.html");
            browserHistory.Push("Page3.html");

            while (browserHistory.Count > 0)
            {
                Console.WriteLine($"Back to :{browserHistory.Pop()}");
            }


            Console.WriteLine();


        }
    }
}
