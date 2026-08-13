
using BankAccoutManagementSystem.Interfaces;

namespace BankAccoutManagementSystem.Model;

public abstract class Account : ITrasnsactionable
{
    public string Name { get;} = String.Empty;
    public int Id { get;}
    public int AccountNumber { get;}
    public string BankName { get;} = String.Empty;
    
    
    public decimal  BankBalance { get; private set; }

    protected Account(string name,int id, int accountNumber, decimal initialBalance,string bankName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        if (id <= 0)
            throw new ArgumentException("Id must be positive", nameof(id));

        if (accountNumber <= 0)
            throw new ArgumentException("Account number must be positive", nameof(accountNumber));

        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative", nameof(initialBalance));

        if (string.IsNullOrWhiteSpace(bankName))
            throw new ArgumentException("Bank name is required", nameof(bankName));
        Name = name;
        Id = id;
        AccountNumber = accountNumber;
        BankBalance = initialBalance;
        BankName = bankName;
    }

    protected void AdjustBalance(decimal amount)
    {
        BankBalance += amount;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive", nameof(amount));
        AdjustBalance(amount);
    }
    public abstract void Withdraw(decimal amount);
    public abstract string GetAccountType();

    public override string ToString() => $"[{Id}] {Name}- {AccountNumber} ({GetAccountType()})-Balance :{BankBalance:C}";

}

