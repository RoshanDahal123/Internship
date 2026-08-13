
using System.Linq;
using BankAccoutManagementSystem.Interfaces;
using BankAccoutManagementSystem.Model;

namespace BankAccoutManagementSystem.Services;

public class BankService
{
    private readonly List<Account> _accounts = new();

    public Account OpenAccount(Account account)
    {
        if(account == null)
            throw new ArgumentNullException(nameof(account));

        if(_accounts.Any(a => a.AccountNumber == account.AccountNumber))
            throw new InvalidOperationException($"Account number {account.AccountNumber} already exists");

        _accounts.Add(account);
        return account;
    }

    public Account? FindByAccountNumber(int accountNumber)
    {
        return _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
    }

    public IReadOnlyList<Account> GetAllAccounts()
    {
        return _accounts.AsReadOnly();
    }
    public decimal GetTotalBankAssets()
    {
        return _accounts.Sum(a => a.BankBalance);
    }

    public IEnumerable<Account> GetAccountsAboveBalance(decimal minBalance)
    {
        return _accounts.Where(a => a.BankBalance >= minBalance);
    }

    public IEnumerable<T> GetAccountsByType<T>() where T : Account
    {
        return _accounts.OfType<T>();
    }
    public IEnumerable<Account> GetInterestBearingAccounts()
    {
        return _accounts.OfType<IInterestBearing>().Cast<Account>();
    }

    public Account? GetHighestBalanceAccount()
    {
        return _accounts.OrderByDescending(a => a.BankBalance).FirstOrDefault();
    }

    public decimal ApplyInterestToAllEligibleAccounts()
    {
        decimal totalInterestPaid = 0;
        foreach (var account in _accounts.OfType<IInterestBearing>())
        {
            totalInterestPaid += account.ApplyInterest();
        }
        return totalInterestPaid;
    }
}