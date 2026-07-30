


using BankAccoutManagementSystem.Exceptions;
using BankAccoutManagementSystem.Interfaces;

namespace BankAccoutManagementSystem.Model;


    public class SavingsAccount : Account,IInterestBearing
{
        public decimal InterestRate { get; }

        public SavingsAccount(string name, int id, int accountNumber, decimal bankBalance, string bankName, decimal interestRate) : base(name, id, accountNumber, bankBalance, bankName)
        {
            if (interestRate < 0)
                throw new ArgumentException("Interest rate cannot be negative", nameof(interestRate));
            InterestRate = interestRate;
        }

        public override string GetAccountType() => "Saving";

    

    public override void   Withdraw(decimal amount)
    {
        if(amount<= 0)
        {
            throw new ArgumentException("Withdrawal amount must be positive", nameof(amount));
        }

        if (BankBalance - amount < 0)
            throw new InsufficientFundsExceptions($"Insufficient funds. Balance: {BankBalance:C}, Requested: {amount:C}");

        AdjustBalance(-amount);
    }

    public decimal ApplyInterest()
    {
        decimal interest = BankBalance * InterestRate;
        AdjustBalance(interest);
        return interest;

    }

}

