using System;
using System.Collections.Generic;
using System.Text;

namespace C_Sharp_Advanced_Concepts
{
    /*
     * DELEGATES
     * ---------
     * A delegate is a type-safe function pointer: a variable that can hold a
     * reference to a method (or several) matching a specific signature, and
     * be invoked like a method call. This enables passing behavior around as
     * data (callbacks, strategies, etc.).
     *
     * EVENTS
     * ------
     * An event is built on top of a delegate. It lets a class publish
     * notifications ("something happened") that other classes can subscribe
     * to, without the publisher needing to know who's listening. This is the
     * classic publisher/subscriber (observer) pattern.
     *
     * LAMBDA EXPRESSIONS
     * -------------------
     * A lambda is a short, inline, unnamed function: `(parameters) => expression`.
     * They're commonly used to plug quick logic into delegates, LINQ, or events
     * without writing a separate named method.
     */

// Step 1: declare a delegate type - defines the method "shape" it can hold

// Step 2: declare an event based on that delegate type
    public delegate void Notify(string message);
    public class OrderProcessor
    {
        public event Notify? OrderShipped;

        public void ShipOrder(string orderId)
        {
            Console.WriteLine($" SHippeing order {orderId}....");
            // Step 3: raise the event - notifies all subscribers
            OrderShipped?.Invoke($"Order {orderId} has shippped!");
        }
    }

    public static class DelegatesEventsDemo
    {
      // a plain method that matches the Notify delegate signature

        private static void LogToConsole(string message)
        {
            Console.WriteLine($"[LOG] {message}");

        }
        public static void Run()
        {
            Console.WriteLine("=== 3. DELEGATES, EVENTS, AND LAMBDA EXPRESSIONS ===");

            // ---- Delegates ----
            Console.WriteLine("\n-- Delegate pointing to a named method --");
            Notify notifier = LogToConsole;
            notifier("Delegate invoted directly");
            Console.WriteLine("\n --delegate pointing to a lambda");
            Notify lambdaNotifier= (message) => Console.WriteLine($"  [LAMBDA] {message}");

            lambdaNotifier("Delegate invoked with a lambda body.");
            //Multicas delgate:one delegate, multiple subscribers , called in order

            Console.WriteLine("\n ==Multicas delegate (multiple handlers) --");
            Notify combined = LogToConsole;
            combined += (msg) => Console.WriteLine($"[Email SIM] Emailing= {msg}");
            combined += (msg) => Console.WriteLine($"  [SMS SIM] Texting: {msg}");
            combined("Multicas message fired");

            //----Events----
            Console.WriteLine("\n-- Event (publisher/subscriber) --");

            var processor = new OrderProcessor();

            //Subscribe using a named method;

            processor.OrderShipped += LogToConsole;
            //// Subscribe using a lamba
            processor.OrderShipped += (msg) => Console.WriteLine($"  [CUSTOMER NOTIFICATION] {msg}");
            processor.ShipOrder("A1001");

            /*
             * FUNC<> AND ACTION<>
             * --------------------
             * These are built-in generic delegate types provided by .NET, so
             * you don't have to declare a custom `delegate` (like our `Notify`
             * above) every time you need one.
             *
             *   Action<...>        - a method that returns nothing (void).
             *                        Action, Action<T>, Action<T1,T2>, ...
             *                        up to 16 parameters.
             *
             *   Func<..., TResult> - a method that RETURNS a value. The LAST
             *                        generic type parameter is always the
             *                        return type; everything before it is a
             *                        parameter.
             *                        Func<TResult>, Func<T1,TResult>,
             *                        Func<T1,T2,TResult>, ... up to 16 params.
             *
             * Rule of thumb: returns nothing -> Action. Returns something -> Func.
             *
             * Example shapes:
             *   Action                  ()      -> void
             *   Action<string>          (string) -> void
             *   Func<int>               ()      -> int
             *   Func<int, bool>         (int)   -> bool
             *   Func<int, int, int>     (int,int) -> int   <-- used below
             *
             * Why use these instead of a custom delegate? Less boilerplate -
             * no need to declare `public delegate int AddDelegate(int a, int b);`
             * just to hold a lambda; Func<int,int,int> already means exactly
             * that, and every C# developer recognizes it on sight.
             *
             * This is also what LINQ methods expect under the hood, e.g.
             * Where() wants a Func<T, bool>, Select() wants a Func<T, TResult>,
             * and List<T>.ForEach() wants an Action<T>.
             */


            //--standlone lambda uage (Func/Action)---
            Console.WriteLine("\n-- Lambdas with Func<> and Action<> --");
            Func<int, int, int> add = (a, b) => a + b;
            Action<string> shout = (text) => Console.WriteLine($"{text.ToUpper()}!!!");
            Console.WriteLine($"add(5,7) = {add(5, 7)}");
            shout("lambdas are handy");

            Console.WriteLine();
        }
    }
}
