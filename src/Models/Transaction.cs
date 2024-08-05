using DebuggingAndRefactoringTask1.Enums;

namespace DebuggingAndRefactoringTask1.Models
{
    public class Transaction
    {
        public required Guid Id { get; set; }
        public required string AccountId { get; set; }
        public required decimal Amount { get; set; }
        public required TransactionType TransactionType { get; set; }
        public required DateTimeOffset TransactionDate { get; set; }
    }
}
