


using System;
using System.Reflection.Metadata;

namespace OOP
{
    class Program
    {
        static void Main(string[] args)
        {

            //Simple exception handling
            //    try
            //    {
            //        int a = 10, b = 0;
            //        int result = a / b;//throws DivideByZeroException
            //        Console.WriteLine(result);


            //    }
            //    catch (DivideByZeroException ex)
            //    {
            //        Console.WriteLine($"Error:{ex.Message}");
            //    }
            //    finally
            //    {
            //        Console.WriteLine("This always executes,errors or not");
            //    }



            //Multiple exception handling

            //try
            //{
            //    string[] name = { "John", "Jane", "Doe" };
            //    Console.WriteLine(name[5]);//throws IndexOutOfRangeException
            //}
            //catch (IndexOutOfRangeException ex)
            //{
            //    Console.WriteLine($"Error:{ex.Message}");

            //}
            //catch (Exception ex)    // generic catch-all, always put this at the end of the catch blocks
            //{
            //    Console.WriteLine($"Error:{ex.Message}");
            //}
            //finally
            //{
            //    Console.WriteLine("Cleanup Code here");


            //try
            //{
            //    double remainingBalance = -100;
            //    double withdrawBalance = 1500;
            //    withdraw(remainingBalance, withdrawBalance);

            //}
            //catch (ArgumentException ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");

            //}
            //catch (InvalidOperationException ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($" Error:{ex.Message}");
            //}
            //finally
            //{
            //    Console.WriteLine("Cleanup code here");

            //}


            ////}

            //static void withdraw(double remainingBalance, double withdrawBalance)
            //{
            //    if (remainingBalance < 0)
            //    {
            //        throw new ArgumentException("Remaining balance cannot be negative");
            //    }

            //    else if (remainingBalance < withdrawBalance)
            //    {
            //        throw new InvalidOperationException("Insufficient funds for withdrawal");
            //    }
            //    else
            //    {
            //        remainingBalance -= withdrawBalance;
            //        Console.WriteLine($"Withdrawal successful. Remaining balance: {remainingBalance}");
            //    }
            //}


            try
            {
                withdraw(1000, 1500);
            }
            catch(InsufficientFundsException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //custom  exception handling
        class InsufficientFundsException : Exception
        {
            public InsufficientFundsException(string message) : base(message) { }
        }
        static void withdraw(double remainingBalance, double withdrawBalance)
        {
            if (withdrawBalance > remainingBalance)
                throw new InsufficientFundsException("Not enough balance to withdraw.");
        }
    }
}
