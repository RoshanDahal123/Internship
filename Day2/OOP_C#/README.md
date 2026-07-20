 ### Exception Handling in C# 

 syntax for exception handling in C# is as follows:
````
 try{
	 // Code that may throw an exception
 }
 catch(ExceptionType1 ex1){
	 // Code to handle ExceptionType1
 }
 catch(ExceptionType2 ex2){
	 // Code to handle ExceptionType2
 }
 finally{
	 // Code that will always execute, regardless of whether an exception was thrown or not
 }

 ````

 Orders Matters: Put the more specific exception types before the more general ones.

** Always put the generic exception type (Exception) at the end of the catch blocks to 
 avoid catching exceptions that could be  handled by more specific catch blocks.


 ### Throwing your own exceptions
````
 void Withdraw(double balance, double amount)
{
    if (amount > balance)
        throw new InvalidOperationException("Insufficient funds.");

    Console.WriteLine($"Withdrew {amount}. New balance: {balance - amount}");
}

try
{
    Withdraw(100, 150);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message); // Insufficient funds.
}
````