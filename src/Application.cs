using DebuggingAndRefactoringTask1.Enums;
using DebuggingAndRefactoringTask1.Models;
using DebuggingAndRefactoringTask1.Services.Interfaces;
namespace DebuggingAndRefactoringTask1
{
    public class Application(IBankingService _bankingService)
    {
        public void Run()
        {
            while (true)
            {
                Console.WriteLine("1. Add Account");
                Console.WriteLine("2. Deposit Money");
                Console.WriteLine("3. Withdraw Money");
                Console.WriteLine("4. Display Account Details");
                Console.WriteLine("5. Display Account Transaction History");
                Console.WriteLine("6. Account Transfer");
                Console.WriteLine("7. Exit");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddAccount();
                        break;
                    case "2":
                        DepositMoney();
                        break;
                    case "3":
                        WithdrawMoney();
                        break;
                    case "4":
                        DisplayAccountDetails();
                        break;
                    case "5":
                        DisplayAccountTransactionHistory();
                        break;
                    case "6":
                        AccountTransfer();
                        break;
                    case "7":
                        return; //Exit the application
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        public void AddAccount()
        {
            //Get and validate account Id
            string? id;
            bool isValidId;
            bool idAlreadyExists;
            do //Using a do while to handle input errors
            {
                Console.WriteLine("Enter Account ID:");
                id = Console.ReadLine();

                isValidId = _bankingService.ValidateId(id); //Basic Id validation

                if (!isValidId)
                {
                    Console.WriteLine("Please enter a valid Account ID.");
                }

                idAlreadyExists = _bankingService.DoesAccountExist(id!);  //Check that account with same Id does not already exist

                if (idAlreadyExists)
                {
                    Console.WriteLine("Please choose another Account ID.");
                }
            }
            while (!isValidId || idAlreadyExists);

            //Get account holder name
            string? accountHolderName;
            bool isValidAccountHolderName;
            do
            {
                Console.WriteLine("Enter Account Holder Name:");
                accountHolderName = Console.ReadLine(); //Corrected line to use Console.Readline()

                isValidAccountHolderName = _bankingService.ValidateAccountHolderName(accountHolderName); //Basic account holder name validation

                if (!isValidAccountHolderName)
                {
                    Console.WriteLine("Please enter a valid Account Holder Name.");
                }

            } while (!isValidAccountHolderName);

            //Create and add account
            var account = new Account { Id = id!, AccountHolderName = accountHolderName!, Balance = 0m };

            var isAccountAdded = _bankingService.AddAccount(account);

            if (isAccountAdded)
            {
                Console.WriteLine("Account added successfully.");
            }
            else 
            {
                Console.WriteLine("Failed to add account.");
            }
        }

        public void DepositMoney()
        {
            //Get account Id
            var id = GetAccountId();

            //Get amount to deposit
            decimal depositAmount;
            bool isValidDeposit = false;
            bool isValidDecimalInput;
            do
            {
                Console.WriteLine("Enter Amount to Deposit:");
                isValidDecimalInput = decimal.TryParse(Console.ReadLine(), out depositAmount); //Validate input

                if (!isValidDecimalInput)
                {
                    Console.WriteLine("Please enter a valid deposit amount.");
                }
                else
                {
                    isValidDeposit = _bankingService.ValidateMonetaryValue(depositAmount); //Basic deposit validation

                    if (!isValidDeposit)
                    {
                        Console.WriteLine("Please enter a valid deposit amount.");
                    }
                }

            } while (!isValidDecimalInput || !isValidDeposit);

            var account = _bankingService.GetAccount(id);

            if (account is null)
            {
                Console.WriteLine("Account not found."); //These validation/error messages could be moved to a static class to avoid repeated use of "magic strings"
            }
            else
            {
                account.Balance += depositAmount;

                //Add transaction to transaction history
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    AccountId = account.Id,
                    Amount = depositAmount,
                    TransactionType = TransactionType.Deposit,
                    TransactionDate = DateTimeOffset.Now
                };

                _bankingService.AddTransaction(transaction);

                Console.WriteLine("Deposit successful.");
            }
        }

        public void WithdrawMoney()
        {
            //Get account Id
            var id = GetAccountId();

            //Get amount to withdraw
            decimal withdrawAmount;
            bool isValidWithdraw = false;
            bool isValidDecimalInput;
            do
            {
                Console.WriteLine("Enter Amount to Withdraw:");
                isValidDecimalInput = decimal.TryParse(Console.ReadLine(), out withdrawAmount); //Validate input

                if (!isValidDecimalInput)
                {
                    Console.WriteLine("Please enter a valid withdraw amount.");
                }
                else
                {
                    isValidWithdraw = _bankingService.ValidateMonetaryValue(withdrawAmount); //Basic withdraw validation

                    if (!isValidWithdraw)
                    {
                        Console.WriteLine("Please enter a valid withdraw amount.");
                    }
                }

            } while (!isValidDecimalInput || !isValidWithdraw);

            var account = _bankingService.GetAccount(id);

            if (account is null)
            {
                Console.WriteLine("Account not found.");
            }
            else
            {
                if (_bankingService.EnsureAccountHasSufficientBalance(account.Balance, withdrawAmount)) //Ensure account balance is greater than amount being withdrawn
                {
                    account.Balance -= withdrawAmount;

                    //Add transaction to transaction history

                    var transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        AccountId = account.Id,
                        Amount = withdrawAmount,
                        TransactionType = TransactionType.Withdrawal,
                        TransactionDate = DateTimeOffset.Now
                    };

                    _bankingService.AddTransaction(transaction);

                    Console.WriteLine("Withdrawal successful.");
                }
                else
                {
                    Console.WriteLine("Insufficient balance.");
                }
            }
        }

        public void DisplayAccountDetails()
        {
            //Get account Id
            var id = GetAccountId();

            var account = _bankingService.GetAccount(id);

            if (account is null)
            {
                Console.WriteLine("Account not found.");
            }
            else
            {
                Console.WriteLine($"Account ID: {account.Id}");
                Console.WriteLine($"Account Holder Name: {account.AccountHolderName}");
                Console.WriteLine($"Balance: {string.Format("{0:C}", account.Balance)}");
            }
        }

        public void DisplayAccountTransactionHistory()
        {
            //Get account Id
            var id = GetAccountId();

            var account = _bankingService.GetAccount(id);

            if (account is null)
            {
                Console.WriteLine("Account not found.");
            }
            else
            {
                if (!_bankingService.DoesAccountHaveTransactions(account.Id)) //Ensure there is at least one transaction associated with the account
                {
                    Console.WriteLine($"No Account Transaction History Found.");
                }
                else
                {
                    var accountTransactionHistory = _bankingService.GetTransactionHistoryByAccountId(account.Id); 

                    Console.WriteLine($"Account ID: {account.Id}");
                    Console.WriteLine($"Account Holder Name: {account.AccountHolderName}");
                    Console.WriteLine($"Account Transaction History:");
                    foreach (var transaction in accountTransactionHistory)
                    {
                        Console.WriteLine($"{transaction.Id} - {transaction.TransactionType} - {string.Format("{0:C}", transaction.Amount)} - {transaction.TransactionDate.DateTime.ToShortDateString()} {transaction.TransactionDate.DateTime.ToShortTimeString()}");
                    }
                }
            }
        }

        public void AccountTransfer()
        {
            //Get account Id that is initiating the transfer
            var id = GetAccountId();

            string? accountIdToReceiveTransfer;
            bool isValidId;
            do
            {
                Console.WriteLine("Enter Account ID that will receive the transfer:");
                accountIdToReceiveTransfer = Console.ReadLine();

                isValidId = !string.IsNullOrWhiteSpace(accountIdToReceiveTransfer);
                if (!isValidId) //Validate Id
                {
                    Console.WriteLine("Please enter a valid Account ID.");
                }
            }
            while (!isValidId);

            //Get amount to transfer
            decimal transferAmount;
            bool isValidDecimalInput;
            bool isValidTransfer = false;
            do
            {
                Console.WriteLine("Enter Amount to Transfer:");
                isValidDecimalInput = decimal.TryParse(Console.ReadLine(), out transferAmount); //Validate input

                if (!isValidDecimalInput)
                {
                    Console.WriteLine("Please enter a valid transfer amount.");
                }
                else
                {
                    isValidTransfer = _bankingService.ValidateMonetaryValue(transferAmount); //Basic withdraw validation

                    if (!isValidTransfer)
                    {
                        Console.WriteLine("Please enter a valid transfer amount.");
                    }
                }
                
            } while (!isValidDecimalInput || !isValidTransfer);

            var account = _bankingService.GetAccount(id);

            if (account is null)
            {
                Console.WriteLine("Account not found.");
            }
            else
            {
                var accountToReceiveTransfer = _bankingService.GetAccount(accountIdToReceiveTransfer!);

                if (accountToReceiveTransfer is null)
                {
                    Console.WriteLine("Account to receive transfer not found.");
                }
                else
                {
                    if (account.Balance >= transferAmount)
                    {
                        account.Balance -= transferAmount;
                        accountToReceiveTransfer.Balance += transferAmount;

                        //Add transfer transactions to transaction history

                        var transferWithdrawalTransaction = new Transaction
                        {
                            Id = Guid.NewGuid(),
                            AccountId = account.Id,
                            Amount = transferAmount,
                            TransactionType = TransactionType.TransferWithdrawal,
                            TransactionDate = DateTimeOffset.Now
                        };

                        _bankingService.AddTransaction(transferWithdrawalTransaction);

                        var transferDeposit = new Transaction
                        {
                            Id = Guid.NewGuid(),
                            AccountId = accountToReceiveTransfer.Id,
                            Amount = transferAmount,
                            TransactionType = TransactionType.TransferDeposit,
                            TransactionDate = DateTimeOffset.Now
                        };

                        _bankingService.AddTransaction(transferDeposit);

                        Console.WriteLine("Account transfer successful.");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient balance.");
                    }
                }
            }
        }

        public string GetAccountId()
        {
            string? id;
            bool isValidId;
            do
            {
                Console.WriteLine("Enter Account ID:");
                id = Console.ReadLine();

                isValidId = !string.IsNullOrWhiteSpace(id);
                if (!isValidId) //Validate Id
                {
                    Console.WriteLine("Please enter a valid Account ID.");
                }
            }
            while (!isValidId);

            return id!;
        }
    }
}
