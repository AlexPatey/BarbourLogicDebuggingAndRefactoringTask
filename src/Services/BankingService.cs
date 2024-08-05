using DebuggingAndRefactoringTask1.Enums;
using DebuggingAndRefactoringTask1.Models;
using DebuggingAndRefactoringTask1.Services.Interfaces;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace DebuggingAndRefactoringTask1.Services
{
    public partial class BankingService : IBankingService
    {
        /*I am using lists here to represent an in-memory database, in a real scenario Account and Transaction would represent database tables and I would implement a data layer using the Repository pattern,
        I have omitted this here and have used the Repository pattern in my solution to "Development Task 1"*/

        private static readonly List<Account> _accounts = [];
        private static readonly List<Transaction> _transactionHistory = [];

        public bool ValidateId(string? id)
        {
            return !string.IsNullOrWhiteSpace(id);
        }
        
        public bool ValidateAccountHolderName(string? accountHolderName)
        {
            return !string.IsNullOrWhiteSpace(accountHolderName);
        }
        
        public bool ValidateMonetaryValue(decimal depositAmount)
        {
            return ValidMonetaryValueRegex().IsMatch(depositAmount.ToString());
        }

        public Account? GetAccount(string id)
        {
            return _accounts.SingleOrDefault(a => a.Id == id); //SingleOrDefault should be used as each account should have a unique identifier
        }

        public bool DoesAccountExist(string id)
        {
            return _accounts.Any(a => a.Id == id);
        }

        public bool EnsureAccountHasSufficientBalance(decimal balance, decimal withdrawAmount)
        {
            return balance >= withdrawAmount;
        }

        public bool AddAccount(Account account)
        {
            /*In a real scenario I would use an ORM like EF Core and call the SaveChangesAsync() method when modifying the Db, the result of SaveChangesAsync() would determine whether or not we return a true or false,
            in this case as I am using a list to represent a Db table, I am just returning true in all cases. Please note I have a Writings API on my GitHub if you would like to see an example of how I use EF Core, etc.*/
            _accounts.Add(account);
            return true;
        }

        public void AddTransaction(Transaction transaction)
        {
            _transactionHistory.Add(transaction);
        }

        public bool DoesAccountHaveTransactions(string id)
        {
            return _transactionHistory.Any(t => t.AccountId == id);
        }

        public List<Transaction> GetTransactionHistoryByAccountId(string id)
        {
            return _transactionHistory.Where(t => t.AccountId == id).ToList();
        }

        //Regex to ensure deposit/withdrawal value is positive and has no more than two decimal places
        [GeneratedRegex("^[0-9]*(\\.[0-9]{1,2})?$")]
        private static partial Regex ValidMonetaryValueRegex();

    }
}
