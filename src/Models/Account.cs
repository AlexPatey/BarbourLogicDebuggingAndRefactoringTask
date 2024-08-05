namespace DebuggingAndRefactoringTask1.Models
{
    public class Account
    {
        public required string Id { get; set; }
        public required string AccountHolderName { get; set; } //Changed variable name to be more clear
        public required decimal Balance { get; set; } //Changed double to decimal as double is unsuitable type for storing values representing money
    }
}
