

using BankAccoutManagementSystem.Exceptions;
using BankAccoutManagementSystem.Interfaces;
using System;
namespace BankAccoutManagementSystem.Model
{
    public  class CurrentAccount : Account
    {
        public decimal OverDraftLimit { get; }

        public CurrentAccount(string name, int id, int accountNumber, decimal bankBalance, string bankName, decimal overDraftLimit) : base(name, id, accountNumber,bankBalance,bankName)
        {

            if (overDraftLimit < 0)
                throw new ArgumentException("Overdraft limit cannot be negative", nameof(OverDraftLimit));
            OverDraftLimit =overDraftLimit;
        }

        public override string GetAccountType()=> "Current";


       
        public override void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be positive", nameof(amount));

            }

            if (BankBalance - amount < -OverDraftLimit)
                throw new InsufficientFundsExceptions($"Overdraft limit exceeded. Balance: {BankBalance:C}, Limit: {OverDraftLimit:C}, Requested: {amount:C}");


            AdjustBalance(-amount);
        }

    }

}
