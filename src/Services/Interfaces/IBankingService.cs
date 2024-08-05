using DebuggingAndRefactoringTask1.Models;

namespace DebuggingAndRefactoringTask1.Services.Interfaces
{
    public interface IBankingService
    {
        bool ValidateId(string? id);
        bool ValidateAccountHolderName(string? accountHolderName);
        bool ValidateMonetaryValue(decimal depositAmount);
        bool EnsureAccountHasSufficientBalance(decimal balance, decimal withdrawAmount);
        bool DoesAccountHaveTransactions(string id);
        List<Transaction> GetTransactionHistoryByAccountId(string id);
        Account? GetAccount(string id);
        void AddTransaction(Transaction transaction);
        bool DoesAccountExist(string id);
        bool AddAccount(Account account);
    }
}
