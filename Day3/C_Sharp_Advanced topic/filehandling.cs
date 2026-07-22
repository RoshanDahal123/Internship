using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace C_Sharp_Advanced_Concepts
{

    /*
     * FILE HANDLING (Reading/Writing files, Streams)
     * ------------------------------------------------
     * The System.IO namespace provides classes to work with files and folders.
     *
     *   File.WriteAllText / File.ReadAllText - simplest way to write/read an
     *      entire text file in one call.
     *   StreamWriter / StreamReader - lower-level, line-by-line or buffered
     *      access; useful for large files where you don't want everything in
     *      memory at once, or when appending.
     *   FileStream - raw byte-level stream access (binary files).
     *
     * Always dispose streams when done (the `using` statement does this
     * automatically, even if an exception occurs).
     */

    public static class FileHandlingDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== 5. FILE HANDLING (Reading/Writing files, Streams) ===");
            string folder = Path.Combine(AppContext.BaseDirectory, "demo_files");
            Directory.CreateDirectory(folder);

            string filePath = Path.Combine(folder, "sample.txt");

            //---Simple write. read (FileClass ) ---
            Console.WriteLine("\n-- File.WriteAllText / File.ReadAllText --");

            File.WriteAllText(filePath, "hllo this is the line one \n");
            Console.WriteLine($"  Wrote initial content to {filePath}");

            string content = File.ReadAllText(filePath);
            Console.WriteLine($"  Read back: {content.Trim()}");

            //--Appending with streamwriter-----
            Console.WriteLine("\n --stremwriter (apend mode)--");
            using (StreamWriter writer = new StreamWriter(filePath, append: true))
            {
                writer.WriteLine("This is line two, appended...");
                writer.WriteLine("This is line three, appended...");

            }//writer is automatically flushed and closed here


            Console.WriteLine("  Appended two more lines.");

            //----Reading line by line with StreamReader----
            Console.WriteLine("\n-- StreamReader (line by line) --");

            using (StreamReader reader = new StreamReader(filePath))
            {
                string? line;
                int lineNumber = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine($"{lineNumber} {line}");
                    lineNumber++;
                }
            }

            // ---- File info / existence checks ----
            Console.WriteLine("\n-- File metadata --");

            FileInfo info = new FileInfo(filePath);
            Console.WriteLine($" Exists:{info.Exists},Size:{info.Length} bytes, Last write:{info.LastWriteTime}");
            // ---- Cleanup ----

            File.Delete(filePath);
            Directory.Delete(folder);

            Console.WriteLine("\n  Cleaned up demo file and folder.");

            Console.WriteLine();

        }
    }
}
