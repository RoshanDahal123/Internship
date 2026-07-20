using System;
using System.Collections.Generic;
using System.Text;

namespace interfaces

{
    public abstract class Animal
    {
        public string Name;

        public Animal(string name)
        {
            Name = name;
        }
        public abstract void MakeSound();
    }

    interface ISwimmable
    {
        void Swim();
    }

    interface IFlyable
        {
        void Fly();
        }


      class Duck :Animal, ISwimmable, IFlyable
    {
        public Duck(string name) : base(name) { }
        public override void MakeSound() => Console.WriteLine($"{Name} quacks.");

        public void Swim() => Console.WriteLine($"{Name} is swimming.");

        public void Fly() => Console.WriteLine($"{Name} is flying.");
    }

    class Cat : Animal, ISwimmable, IFlyable
    {
        public Cat(string name) : base(name) { }
        public override void MakeSound() => Console.WriteLine($"{Name} meows.");

        public void Swim() => Console.WriteLine($"{Name} is not swimming.");

        public void Fly() => Console.WriteLine($"{Name} is cannot fly");

    }
}
    
