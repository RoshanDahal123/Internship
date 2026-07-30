using System;
using System.Collections.Generic;
using System.Text;

namespace BankAccoutManagementSystem.Interfaces
{
    public interface IInterestBearing
    {
        decimal InterestRate { get; }
        // Applies interest to the account's balance based on its own rate.
        // Returns the interest amount applied — useful for logging/reporting later
        decimal ApplyInterest();
    }
}
