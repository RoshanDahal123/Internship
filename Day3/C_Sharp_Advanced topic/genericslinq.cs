using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace C_Sharp_Advanced_Concepts
{
    /*
    * GENERICS
    * --------
    * Generics let you write a class, method, or interface that works with
    * ANY type, decided at the call site, while still keeping compile-time
    * type safety (no casting, no boxing for value types).
    *
    * Example: Box<T> below can hold an int, a string, a custom object, etc.,
    * and the compiler enforces that only that type goes in/out.
    *
    * LINQ (Language-Integrated Query)
    * ---------------------------------
    * LINQ lets you query collections (lists, arrays, dictionaries...) using
    * a declarative, SQL-like syntax directly in C#. Common operators:
    * Where (filter), Select (project/transform), OrderBy (sort),
    * GroupBy, Sum/Average/Max/Min (aggregate), First/FirstOrDefault, etc.
    */
    // A simple generic class: T is a placeholder for "whatever type you give me"

    public class Box<T>
    {
        public T Content { get; set; }
        public Box(T content)
        {
            Content = content;
        }

        public void Describe()
        {
            Console.WriteLine($" box constains a {typeof(T).Name}:{Content}");
        }
    }

    //a generic method (independent of generic class)
    public static class GenericHelpers
    {
        public static T GetLast<T>(List<T> items)
        {
            return items[items.Count - 1];
        }
    }

    public record Person(string Name, int Age, string City);
    public static class GenericsLinqDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== 2. GENERICS AND LINQ ===");
            // ---- Generics ----
            Console.WriteLine("\n-- Generic class Box<T> --");

            Box<int> intBox = new Box<int>(42);
            Box<string> stringBox = new Box<string>("Hello Generics");

            intBox.Describe();
            stringBox.Describe();

            List<double> prices = new List<double> { 9.99, 19.99, 4.99 };
            Console.WriteLine($"Last price via generic method:{GenericHelpers.GetLast(prices)}");

            //---------LINQ----
            List<Person> people = new List<Person>
            {
                new("Alice",30,"London"),
                new("Bob", 25, "Paris"),
                new("Charlie", 35, "London"),
                new("Diana", 28, "New York"),
                new("Eve", 40, "Paris")
            };

            Console.WriteLine("\n ---LINQ: filetr (where) + sort (OrderBy)---");
            var over28 = people.Where(p => p.Age > 28)
                .OrderBy(p => p.Age)
                .ToList();
            foreach (var p in over28)
                Console.WriteLine($"{p.Name}({p.Age})");

            Console.WriteLine("\n-- LINQ: project (Select) --");

            var names = people.Select(p => p.Name.ToUpper()).ToList();
            Console.WriteLine(" " + string.Join(" ,", names));

            Console.WriteLine("---GroupBy----------");
            var byCity = people.GroupBy(p => p.City);
            foreach (var group in byCity)
            {
                Console.WriteLine($"{group.Key}:");
                foreach (var p in group)
                    Console.WriteLine(p.Name);
            }

            Console.WriteLine("\n-- LINQ: aggregate (Average, Max) --");
            double average = people.Average(p => p.Age);
            int maxAge = people.Max(p => p.Age);
            Console.WriteLine($"Average age:{average:F1},Max age:{maxAge}");
            Console.WriteLine();
        }
    }
}


