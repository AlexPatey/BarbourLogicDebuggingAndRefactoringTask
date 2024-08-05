using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebuggingAndRefactoringTask1.Enums
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        TransferDeposit, //Naming here might need to be changed, would need to investigate specific banking terms
        TransferWithdrawal //Naming here might need to be changed, would need to investigate specific banking terms
    }
}
