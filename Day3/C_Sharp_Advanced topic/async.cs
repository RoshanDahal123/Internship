using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace C_Sharp_Advanced_Concepts

{
    /*
     * ASYNCHRONOUS PROGRAMMING (async/await)
     * ----------------------------------------
     * async/await lets you write code that performs long-running operations
     * (network calls, file I/O, delays) WITHOUT blocking the calling thread.
     *
     *   - A method marked `async` can use `await` inside it.
     *   - `await` pauses the method (returns control to the caller) until the
     *     awaited Task completes, then resumes - without blocking a thread
     *     while it waits.
     *   - Task represents an operation that may not have finished yet.
     *     Task<T> is the same but produces a result of type T when done.
     *
     * This is different from just running things in parallel on multiple
     * threads - it's about not wasting a thread sitting idle while waiting.
     */


    public static class AsyncDemo
    {
        //SImulate a slow network/database call

        private static async Task<string> FetchDataAsync(string source, int delayMs)
        {
            Console.WriteLine($" fetch from {source}....");
            await Task.Delay(delayMs);
            Console.WriteLine($" FInished fetch from {source}");
            return $"Data fetch from{source}";
        }



        public static async Task Run()
        {

            Console.WriteLine("=== 4. ASYNCHRONOUS PROGRAMMING (async/await) ===");
            // ---- Sequential await: each call waits for the previous one ----
            // ---- Sequential await: each call waits for the previous one ----
            Console.WriteLine("\n-- Sequential awaits --");

            var sw = Stopwatch.StartNew();
            string result1 = await FetchDataAsync("ServoceA", 800);
            string result2 = await FetchDataAsync("ServoceB", 800);

            sw.Stop();

            Console.WriteLine($"  Got: {result1}, {result2}");
            Console.WriteLine($"  Sequential time: ~{sw.ElapsedMilliseconds} ms (roughly 800+800)");

            // ---- Concurrent await: run tasks at the same time, wait for all ----
            Console.WriteLine("\n-- Concurrent awaits with Task.WhenAll --");
            sw.Restart();
            Task<string> taskA = FetchDataAsync("ServiceC", 800);
            Task<string> taskB = FetchDataAsync("ServiceD", 800);
            string[] results = await Task.WhenAll(taskA, taskB);

            sw.Stop();

            Console.WriteLine($"  Got: {string.Join(", ", results)}");
            Console.WriteLine($"  Concurrent time: ~{sw.ElapsedMilliseconds} ms (roughly max(800,800), not the sum)");
            //---Basic error handling with async-----

            Console.WriteLine("\n -- Handling exceptions in async code --");
            try
            {
                await FailingOperationAsync();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($" Caught expected error:{ex.Message}");

            }
            Console.WriteLine();
        }


        private static async Task FailingOperationAsync()
        {
            await Task.Delay(200);
            throw new InvalidOperationException("Something went wrong during the async operation");

        }


    }
}
