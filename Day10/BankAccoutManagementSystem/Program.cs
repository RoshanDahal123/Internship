using BankAccoutManagementSystem.Exceptions;
using BankAccoutManagementSystem.Model;
using BankAccoutManagementSystem.Interfaces;
using BankAccoutManagementSystem.Services;


var savings = new SavingsAccount("Roshan", 1, 1001, 500m, "NIC Asia", 0.05m);
var current = new CurrentAccount("Sita", 2, 2001, 200m, "NIC Asia", 500m);
var savings2 = new SavingsAccount("Sujit", 3, 3001, 800m, "Machapuchre", 0.05m);
Console.WriteLine(savings);
Console.WriteLine(current);

Console.WriteLine();
Console.WriteLine("--- Deposits ---");
savings.Deposit(150m);
current.Deposit(100m);
Console.WriteLine(savings);
Console.WriteLine(current);

Console.WriteLine();
Console.WriteLine("--- Withdrawals (valid) ---");
savings.Withdraw(200m);              // fine, balance stays >= 0
current.Withdraw(700m);              // fine, dips into overdraft (200+100-700 = -400, within -500 limit)
Console.WriteLine(savings);
Console.WriteLine(current);

Console.WriteLine();
Console.WriteLine("--- Withdrawals (should fail) ---");

try
{
    savings.Withdraw(10000m);        // way more than balance — no overdraft allowed
}
catch (InsufficientFundsExceptions ex)
{
    Console.WriteLine($"Savings withdrawal failed: {ex.Message}");
}

try
{
    current.Withdraw(1000m);         // would exceed overdraft limit
}
catch (InsufficientFundsExceptions ex)
{
    Console.WriteLine($"Current withdrawal failed: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("--- Interest ---");

if (savings is IInterestBearing interestBearing)
{
    decimal earned = interestBearing.ApplyInterest();
    Console.WriteLine($"Interest applied: {earned:C}");
    Console.WriteLine(savings);
}


var bank = new BankService();
bank.OpenAccount(savings);
bank.OpenAccount(savings2);
bank.OpenAccount(current);

Console.WriteLine();
Console.WriteLine("--- Bank Service ---");
Console.WriteLine($"Total assets: {bank.GetTotalBankAssets():C}");

Console.WriteLine("Accounts above $300:");
foreach (var acc in bank.GetAccountsAboveBalance(300m))
{
    Console.WriteLine($"  {acc}");
}

Console.WriteLine("Savings accounts only:");
foreach (var acc in bank.GetAccountsByType<SavingsAccount>())
{
    Console.WriteLine($"  {acc}");
}

var highest = bank.GetHighestBalanceAccount();
Console.WriteLine($"Highest balance account: {highest}");
Console.ReadKey();


