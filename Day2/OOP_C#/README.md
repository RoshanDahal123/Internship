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


### Debugging in C#

# C# Debugging in Visual Studio — Quick Reference

## Breakpoints
- **F9** — toggle breakpoint on current line
- Click left margin — same effect

## Execution Controls (while paused)
| Key | Action |
|---|---|
| F5 | Continue to next breakpoint |
| F10 | Step Over (skip into method calls) |
| F11 | Step Into (enter method calls) |
| Shift+F11 | Step Out (finish current method) |
| Ctrl+F10 | Run to Cursor |
| Shift+F5 | Stop debugging |

## Inspecting Values
- **Hover** over a variable — instant tooltip with value
- **Locals** (`Debug → Windows → Locals`) — all variables in current scope
- **Watch** (`Debug → Windows → Watch`) — track specific expressions manually
- **Immediate Window** — run/evaluate code live, e.g. `? myVar`
- **Pin a DataTip** — click pin icon on hover to keep value visible while stepping

## Conditional / Advanced Breakpoints
- Right-click breakpoint → **Conditions** — e.g. only break when `i == 50`
- Right-click breakpoint → **Hit Count** — break only on Nth hit
- **Tracepoints** — log a message to Output without pausing execution

## Exception Settings
`Debug → Windows → Exception Settings` (Ctrl+Alt+E)
- Check **CLR Exceptions** to break the instant *any* exception is thrown (even caught ones)
- Leave unchecked to only break on **unhandled** exceptions (default)

## Call Stack
`Debug → Windows → Call Stack` — shows the chain of method calls leading to current point. Double-click a frame to jump to that code.

## Just My Code
`Tools → Options → Debugging → General`
- **On** (default) — skips framework/library internals
- **Off** — lets you step into .NET runtime source

## Output
```csharp
Console.WriteLine("Always visible");
System.Diagnostics.Debug.WriteLine("Only visible in Output window while debugging");
```
`Debug.WriteLine` is stripped from Release builds automatically.

## Assertions
```csharp
System.Diagnostics.Debug.Assert(balance >= 0, "Balance should never be negative!");
```
Breaks execution with a dialog if the condition is false.

## Edit and Continue
Edit code while paused at a breakpoint — VS applies changes live in most cases (method signature changes usually force a restart).

## Typical Workflow
1. Set breakpoint at the suspicious line
2. F5 to run, hover/Locals to check incoming values
3. F10 to step line by line, watch state change
4. If an exception fires, check Call Stack to trace its origin