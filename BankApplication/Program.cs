using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Name: Mohammad Mukahhal
/// Class- CIS 297 C# Programming
/// Date:9/10/2022
/// </summary>

namespace BankApplication
{
    /// <summary>
    /// Testing account class
    /// This class will genereate the array of accounts, featuring a system the user can use
    /// </summary>
    public class AccountTest
    {
        /// <summary>
        /// Main Method that will generate the array of accounts and user interface in console
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // create array of accounts
            Account[] accounts = new Account[4];

            // initialize array with Accounts
            accounts[0] = new SavingsAccount(25M, .03M);
            accounts[1] = new CheckingAccount(80M, 1M);
            accounts[2] = new SavingsAccount(200M, .015M);
            accounts[3] = new CheckingAccount(400M, .5M);

            // loop through array, prompting user for debit and credit amounts
            for (int i = 0; i < accounts.Length; i++)
            {
                Console.WriteLine($"Account {i + 1} balance: {accounts[i].GetBalance():C}");

                Console.Write($"\nEnter an amount to withdraw from Account {i + 1}: ");
                decimal withdrawalAmount = decimal.Parse(Console.ReadLine());

                accounts[i].Debit(withdrawalAmount); // attempt to debit

                Console.Write($"\nEnter an amount to deposit into Account {i + 1}: ");
                decimal depositAmount = decimal.Parse(Console.ReadLine());

                // credit amount to Account
                accounts[i].Credit(depositAmount);

                // if Account is a SavingsAccount, calculate and add interest
                if (accounts[i] is SavingsAccount)
                {
                    // downcast
                    SavingsAccount currentAccount = (SavingsAccount)accounts[i];

                    decimal interestEarned = currentAccount.CalculateInterest();
                    Console.WriteLine($"Adding {interestEarned:C} interest to Account {i + 1} (a SavingsAccount)");
                    currentAccount.Credit(interestEarned);
                }

                Console.WriteLine($"\nUpdated Account {i + 1} balance: {accounts[i].GetBalance():C}\n\n");
            }
            Console.WriteLine("Enter any key to exit");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// Base account class
    /// </summary>
    class Account
    {
        private decimal balance;
        /// <summary>
        /// Account class constructor
        /// </summary>
        /// <param name="initialBalance"></param>
        public Account(decimal initialBalance)
        {
            try
            {
                if(!(initialBalance >= 0))
                {
                    throw new ArgumentException("Initial balance must be greater than or equal to zero.");
                }
                balance = initialBalance;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        /// <summary>
        /// Credit method will add to balance
        /// </summary>
        /// <param name="value"></param>
        public virtual void Credit(decimal value)
        {
            balance = balance + value;
        }
        
        /// <summary>
        /// Debit method will subtract from balance
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Debit(decimal value)
        {
            if (balance >= value)
            {
                balance = balance - value;
                return true;
            }
            else
            {
                Console.WriteLine("Debit amount exceeded account balance.");
                return false;
            }
        }
        /// <summary>
        /// Get Balance will simply return the balance
        /// </summary>
        /// <returns></returns>
        public decimal GetBalance()
        {
            return balance;
        }
    }
    /// <summary>
    /// SavingsAccount will inherit from class Account
    /// </summary>
    class SavingsAccount : Account
    {
        /// <summary>
        /// SavingAccount Constructor
        /// </summary>
        decimal interestRate;
        public SavingsAccount(decimal initialBalance, decimal interestRate) : base(initialBalance)
        {
            this.interestRate = interestRate;
        }
        /// <summary>
        /// This method will return the interest rate
        /// </summary>
        /// <returns></returns>
        public decimal CalculateInterest()
        {
            return GetBalance() * interestRate;
        }
    }

    /// <summary>
    /// CheckingAccount class will inherit from class Account
    /// </summary>
    class CheckingAccount : Account
    {
        decimal fee;

        /// <summary>
        /// CheckingAccount class constructor
        /// </summary>
        /// <param name="initialBalance"></param>
        /// <param name="fee"></param>
        public CheckingAccount(decimal initialBalance, decimal fee) : base(initialBalance)
        {
            this.fee = fee;
        }
        
        /// <summary>
        /// Credit method will overide base Account class credit method to subtract fee
        /// </summary>
        /// <param name="value"></param>
        public override void Credit(decimal value)
        {
            base.Credit(value - fee);
        }

        /// <summary>
        /// Debit method will overide base Account class credit method to subtract fee
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool Debit(decimal value)
        {
            if (base.Debit(value))
            {
                base.Debit(fee); //subtacting the fee only if money was taken
                return true;
            }
            return false;
        }
    }
}
