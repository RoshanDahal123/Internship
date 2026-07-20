using System;
using System.Collections.Generic;
using System.Text;

namespace Encapsulation
{
    class BankAccount
    {
        public double balance;

        public BankAccount(double initialBalance)
        {
            balance = initialBalance;
        }

        public double Balance
        {
            get { return balance; }
            private set { balance = value; }
        }

        public void Deposit(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive");
            }
            balance += amount;
        }
        public void Withdraw(double amount)
        {
            if (amount > balance)
                throw new InvalidOperationException("Insufficient funds.");
            balance -= amount;
        }
    }

}