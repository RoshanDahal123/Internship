# 🧠 .NET Core Concepts: CIL, CLR, JIT, CTS

## 📌 1. CIL (Common Intermediate Language)

CIL is a **low-level, platform-independent language** used in the .NET framework.

When you write code in languages like **C# or VB.NET**, the compiler does **not** directly convert it into machine code. Instead, it converts the code into **CIL (bytecode)**.

### 🔁 Flow:

```
High-Level Code (C#, VB.NET)
        ↓
Compiled into CIL
        ↓
Stored in .dll / .exe
```

* CIL acts as a **middle layer** between your source code and machine code.
* It is **platform-independent**, meaning it can run on any system that supports the .NET runtime.

> 🔎 Note: CIL was previously called **MSIL (Microsoft Intermediate Language)**.

---

## ⚙️ 2. CLR (Common Language Runtime)

The **CLR is the execution engine** of the .NET framework.

### 🎯 Responsibilities:

* Executes CIL code
* Converts CIL → Machine Code using JIT
* Memory management (Garbage Collection)// you donot need to worry about cleaning up the unncecessary file
     it automatically cleans up
* Exception handling
* Security enforcement
* Thread management

### 🔁 Execution Flow:

```
CIL Code (.dll / .exe)
        ↓
CLR loads the code
        ↓
JIT Compiler converts to Machine Code
        ↓
CPU executes it
```

👉 **In simple terms:**
CLR is the **brain of .NET** that runs your application.

---

## ⚡ 3. JIT (Just-In-Time Compiler)

JIT is a component of CLR that converts **CIL into native machine code at runtime**.

### 🚀 Key Points:

* Compilation happens **on-demand** (when code is executed)
* Only required methods are compiled → improves performance
* Output is **CPU-specific machine code** 

Why use JIT compilation?
*  Performance: By compiling code just before it’s needed,
    the JIT compiler can optimize the code
    based on the specific hardware it’s running on.
* Portability: Since CIL is platform-independent, the JIT compiler adapts it to run on different hardware.

### 🔁 Example:

```
CIL Method → JIT compiles → Native Machine Code → Executes
```

---

## 📦 4. CTS (Common Type System)

CTS defines how **data types are declared and used** in .NET environment.

### 🎯 Purpose:

* Ensures **language interoperability** (C#, VB.NET, F#, etc.)
* Standardizes data types across all .NET languages
* Prevents type mismatch issues

### 🧩 Example:

* `int` in C# = `Integer` in VB.NET
  👉 Both are treated the same under CTS

---
### Why is CTS important?
* Interoperabiltiy:
     You can mix and match languages in your .NET projects necause they all share the
     same type of the system
* Consistency: It ensures that all. NET langauges treat data types consistently, reducing
   errors and confusion

## 🧱 Complete Execution Flow

```
1. Write Code (C#, VB.NET)
2. Compiler → Converts to CIL
3. CIL stored in .dll / .exe
4. CLR loads the assembly
5. JIT compiles CIL → Machine Code
6. CPU executes the code
```

---

###  Ouick Recap
1. You write the code in a .NET language like C#.
2. The compiler translates it to CIL .
3. Then the before an application execution, the CLR common language runtime will 
   take part and manage the execution of your application.
4. when the application runs, the JIT compiler translates the CIL into machine code 
   just before execution.
5. The CTS ensures that it handles all the types running in the different 
   languages. such as like C# or in VB.net.


---

# C# Programming Basics: Type Casting, Operators, and Math

This repository covers the fundamental concepts of **Type Casting**, **Operators**, and the built-in **Math Class** in C#.

---

## 1. Type Casting

Type casting occurs when you assign a value of one data type to another data type. In C#, there are two types of casting:

### Implicit Casting (Automatically)
Implicit casting converts a **smaller type to a larger type size**. This is done automatically by the C# compiler because there is no risk of data loss.

char -> int -> long -> float -> double

* **Example:**
  ```csharp
  int myInt = 9;
  double myDouble = myInt; // Automatic casting: int to double

  Explicit Casting (Manually)
Explicit casting converts a larger type to a smaller type size. This must be done manually by placing the type in parentheses () in front of the value, 
as it can result in data loss.

double -> float -> long -> int -> char

double myDouble = 9.78;
int myInt = (int)myDouble; // Manual casting: double to int (myInt will be 9)

# C# Operators and Mathematics

Operators are special symbols or keywords used to perform operations on variables and values. In C#, operators are used for arithmetic calculations, comparisons, logical decisions, assignments, and bit-level operations.

---

# 1. Arithmetic Operators

Arithmetic operators are used to perform mathematical calculations.

| Operator | Description | Example | Result |
|----------|-------------|---------|--------|
| `+` | Addition | `10 + 5` | `15` |
| `-` | Subtraction | `10 - 5` | `5` |
| `*` | Multiplication | `10 * 5` | `50` |
| `/` | Division | `10 / 5` | `2` |
| `%` | Modulus (remainder) | `10 % 3` | `1` |

### Example

```csharp
using System;

class Program
{
    static void Main()
    {
        int a = 10;
        int b = 3;

        Console.WriteLine(a + b); // 13
        Console.WriteLine(a - b); // 7
        Console.WriteLine(a * b); // 30
        Console.WriteLine(a / b); // 3
        Console.WriteLine(a % b); // 1
    }
}
```

---

# 2. Increment and Decrement Operators

Used to increase or decrease a value by 1.

| Operator | Description |
|----------|-------------|
| `++` | Increment |
| `--` | Decrement |

Example:

```csharp
int count = 10;

count++;

Console.WriteLine(count); // 11

count--;

Console.WriteLine(count); // 10
```

---

# Prefix vs Postfix

## Prefix Increment

Value is increased first, then used.

```csharp
int a = 5;

Console.WriteLine(++a);
```

Output:

```
6
```

---

## Postfix Increment

Value is used first, then increased.

```csharp
int a = 5;

Console.WriteLine(a++);
Console.WriteLine(a);
```

Output:

```
5
6
```

---

# 3. Assignment Operators

Used to assign values to variables.

| Operator | Example | Equivalent |
|----------|---------|------------|
| `=` | `x = 5` | Assign |
| `+=` | `x += 5` | `x = x + 5` |
| `-=` | `x -= 5` | `x = x - 5` |
| `*=` | `x *= 5` | `x = x * 5` |
| `/=` | `x /= 5` | `x = x / 5` |
| `%=` | `x %= 5` | `x = x % 5` |

Example:

```csharp
int salary = 50000;

salary += 5000;

Console.WriteLine(salary);
```

Output:

```
55000
```

---

# 4. Relational (Comparison) Operators

Used to compare two values.

| Operator | Meaning |
|----------|---------|
| `==` | Equal |
| `!=` | Not Equal |
| `>` | Greater than |
| `<` | Less than |
| `>=` | Greater than or equal |
| `<=` | Less than or equal |

Example:

```csharp
int age = 20;

Console.WriteLine(age >= 18);
```

Output:

```
True
```

---

# 5. Logical Operators

Used to combine multiple conditions.

| Operator | Description |
|----------|-------------|
| `&&` | Logical AND |
| `||` | Logical OR |
| `!` | Logical NOT |

---

## AND Operator (`&&`)

Returns true only when both conditions are true.

```csharp
int age = 25;
bool hasLicense = true;

Console.WriteLine(age >=18 && hasLicense);
```

Output:

```
True
```

Truth Table:

| A | B | A && B |
|-|-|-|
| T | T | T |
| T | F | F |
| F | T | F |
| F | F | F |

---

## OR Operator (`||`)

Returns true if at least one condition is true.

```csharp
bool isAdmin = false;
bool isManager = true;

Console.WriteLine(isAdmin || isManager);
```

Output:

```
True
```

---

## NOT Operator (`!`)

Reverses the boolean value.

```csharp
bool isActive = true;

Console.WriteLine(!isActive);
```

Output:

```
False
```

---

# 6. Bitwise Operators

Operate directly on binary representation of numbers.

| Operator | Name |
|-|-|
| `&` | AND |
| `|` | OR |
| `^` | XOR |
| `~` | NOT |
| `<<` | Left Shift |
| `>>` | Right Shift |

Example:

```csharp
int a = 5;   // 0101
int b = 3;   // 0011

Console.WriteLine(a & b);
```

Binary operation:

```
0101
0011
----
0001
```

Output:

```
1
```

---

# 7. Conditional Operator (Ternary)

Short form of if-else.

Syntax:

```csharp
condition ? trueValue : falseValue;
```

Example:

```csharp
int age = 20;

string result = age >= 18 
                ? "Adult" 
                : "Minor";

Console.WriteLine(result);
```

Output:

```
Adult
```

---

# 8. Null Operators

C# provides operators to safely handle null values.

---

## Null Coalescing Operator (`??`)

Returns a default value if the variable is null.

```csharp
string name = null;

string result = name ?? "Guest";

Console.WriteLine(result);
```

Output:

```
Guest
```

---

## Null Conditional Operator (`?.`)

Prevents NullReferenceException.

```csharp
string name = null;

Console.WriteLine(name?.Length);
```

Output:

```
null
```

---

# Mathematics in C#

C# provides the `Math` class for mathematical operations.

Namespace:

```csharp
using System;
```

---

# 1. Math Methods

## Absolute Value

Returns positive value.

```csharp
Console.WriteLine(Math.Abs(-10));
```

Output:

```
10
```

---

## Power

```csharp
Console.WriteLine(Math.Pow(2,3));
```

Output:

```
8
```

---

## Square Root

```csharp
Console.WriteLine(Math.Sqrt(25));
```

Output:

```
5
```

---

## Maximum Value

```csharp
Console.WriteLine(Math.Max(10,20));
```

Output:

```
20
```

---

## Minimum Value

```csharp
Console.WriteLine(Math.Min(10,20));
```

Output:

```
10
```

---

## Rounding Numbers

### Round

```csharp
Console.WriteLine(Math.Round(4.6));
```

Output:

```
5
```

---

### Ceiling

Rounds upward.

```csharp
Console.WriteLine(Math.Ceiling(4.1));
```

Output:

```
5
```

---

### Floor

Rounds downward.

```csharp
Console.WriteLine(Math.Floor(4.9));
```

Output:

```
4
```

---

# 2. Random Numbers

C# provides the `Random` class.

Example:

```csharp
Random random = new Random();

int number = random.Next(1,100);

Console.WriteLine(number);
```

Generates:

```
1 - 99
```

---

# 3. Integer Division

When two integers are divided, the decimal part is removed.

Example:

```csharp
int result = 10 / 3;

Console.WriteLine(result);
```

Output:

```
3
```

To get decimal result:

```csharp
double result = 10.0 / 3;

Console.WriteLine(result);
```

Output:

```
3.333333
```

---

# Operator Precedence

C# follows mathematical precedence rules.

Order:

1. Parentheses `()`
2. Unary operators `++ -- !`
3. Multiplication `* / %`
4. Addition `+ -`
5. Comparison `< > <= >=`
6. Equality `== !=`
7. Logical AND `&&`
8. Logical OR `||`
9. Assignment `=`

Example:

```csharp
int result = 10 + 5 * 2;

Console.WriteLine(result);
```

Calculation:

```
5 * 2 = 10
10 + 10 = 20
```

Output:

```
20
```

---

# Important Interview Concepts

## Difference between `==` and `.Equals()`

`==` checks equality based on type implementation.

```csharp
string a = "Hello";
string b = "Hello";

Console.WriteLine(a == b);
```

Output:

```
True
```

`.Equals()` checks object equality.

```csharp
Console.WriteLine(a.Equals(b));
```

Output:

```
True
```

---

## Short Circuit Evaluation

C# stops evaluating logical expressions when the result is already known.

Example:

```csharp
bool result = false && SomeMethod();
```

`SomeMethod()` will never execute because the first condition is already false.

---

# Summary

| Category | Operators |
|-|-|
| Arithmetic | `+ - * / %` |
| Assignment | `= += -= *= /=` |
| Comparison | `== != > < >= <=` |
| Logical | `&& || !` |
| Bitwise | `& | ^ ~ << >>` |
| Conditional | `?:` |
| Null Handling | `?? ?. ` |

Operators are fundamental building blocks in C# and are heavily used in conditions, loops, algorithms, LINQ queries, and application logic.



## String Interpolation
  Another option of string concatenation, is string interpolation, which substitutes values of variables into placeholders in a string.
  Note that you do not have to worry about spaces, like with concatenation:

Example
```` 
string firstName = "John";
string lastName = "Doe";
string name = $"My full name is: {firstName} {lastName}";
Console.WriteLine(name);
````

## Access of string

````
string myString = "Hello";
Console.WriteLine(myString[0]);
//Output: H
Console.ReadLine();
````

You can also find the index position of a specific character in a string, by using the IndexOf() method:

````
string myString = "Hello";
Console.WriteLine(myString.IndexOf("e"));  // Outputs "1"
````

Another useful method is Substring(), which extracts the characters from a string, starting from the specified character position/index, and returns a new string.
This method is often used together with IndexOf() to get the specific character position:

````

````
