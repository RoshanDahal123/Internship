# C# OOP Keywords — Quick Reference

## `static`
Belongs to the class, not any instance. Shared across all objects. Accessed via class name.
```csharp
static class MathHelper { public static int Square(int x) => x * x; }
MathHelper.Square(5);
```

## `readonly`
Set once — at declaration or inside constructor. Locked after that.
```csharp
public readonly string SerialNumber;
```

## `const`
Fixed at compile-time. Never changes. Implicitly static.
```csharp
public const double Pi = 3.14159;
```

## `sealed`
Prevents further inheritance (on class) or further overriding (on method).
```csharp
sealed class Dog : Animal { }
public sealed override void Speak() { }
```

## `virtual` / `override`
`virtual` = can be overridden. `override` = provides the derived class's version.
```csharp
public virtual void Speak() { }
public override void Speak() { }
```

## `new` (on members)
Hides a base member instead of overriding it. Breaks polymorphism — avoid unless intentional.
```csharp
public new void Speak() { }
```

## `abstract`
Marks a class as non-instantiable, or a method with no body (must be implemented by derived class).
```csharp
abstract class Shape { public abstract double Area(); }
```

## `partial`
Splits one class across multiple files; compiles into a single class.
```csharp
partial class Employee { public string Name; }   // File1
partial class Employee { public void Print() { } } // File2
```

## `this`
Refers to the current instance. Disambiguates field vs parameter, or chains constructors.
```csharp
public Car(string model) { this.Model = model; }
public Car() : this("Unknown") { }
```

## `base`
Refers to the parent class. Calls base constructor or base method implementation.
```csharp
public Dog(string name) : base(name) { }
base.Speak();
```

## Access Modifiers

| Modifier | Visible from |
|---|---|
| `public` | Anywhere |
| `private` | Same class only |
| `protected` | Same class + derived classes |
| `internal` | Same project/assembly only |
| `protected internal` | Same assembly OR derived classes anywhere |
| `private protected` | Same assembly AND derived classes only |

## Quick Summary Table

| Keyword | Meaning |
|---|---|
| `static` | Belongs to class, not instance |
| `readonly` | Settable once (declaration/constructor) |
| `const` | Fixed at compile-time |
| `sealed` | No further inheritance/override |
| `virtual` | Can be overridden |
| `override` | Replaces base behavior |
| `new` (member) | Hides base member |
| `abstract` | No implementation; derived class must implement |
| `partial` | Splits class across files |
| `this` | Current instance |
| `base` | Parent class |