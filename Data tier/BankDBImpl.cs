using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Data_tier
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public class BankDBImpl : IBankDB
    {
        //---------------------object declaration------------------------------------
        static BankDB.BankDB bankDB = new BankDB.BankDB();
        BankDB.UserAccessInterface iUserAccess = bankDB.GetUserAccess();
        BankDB.AccountAccessInterface iAccountAccess = bankDB.GetAccountInterface();
        BankDB.TransactionAccessInterface iTransactionAccess = bankDB.GetTransactionInterface();
        //---------------------end of object declaration------------------------------------

        // --------------------- BankDB ---------------------------------
        public void ProcessAllTransactions()
        {
            bankDB.ProcessAllTransactions();    //call ProcessAllTransaction in dll
        }

        public void SavetoDisk()
        {
            bankDB.SaveToDisk();    //call SaveToDisk in dll
        }
        // ---------------------End of BankDB ------------------------------


        // --------------------- User Access ---------------------------------
        public uint CreateUser()
        {
            return iUserAccess.CreateUser();    //call CreateUser in dll
        }

        public void GetUserName(out string fname, out string lname)
        {
            iUserAccess.GetUserName(out fname, out lname);      //call GetUserName in dll
        }

        public List<uint> GetUsers()
        {
            return iUserAccess.GetUsers();      //call GetUsers in dll
        }

        public void SelectUser(uint userID)
        {
            iUserAccess.SelectUser(userID);     //call SelectUser in dll
        }

        public void SetUserName(string fname, string lname)
        {
            iUserAccess.SetUserName(fname, lname);      //call SetUserName in dll
        }
        // ---------------------End of User Access ------------------------------


        // --------------------- Account Access ---------------------------------
        public uint CreateAccount(uint userID)
        {
            return iAccountAccess.CreateAccount(userID);        //call CreateAccount in dll
        }

        public void Deposit(uint amount)
        {
            iAccountAccess.Deposit(amount);     //call Deposit in dll
        }

        public List<uint> GetAccountIDsByUser(uint userID)
        {
            return iAccountAccess.GetAccountIDsByUser(userID);      //call GetAccountIDsByUser in dll
        }

        public uint GetBalance()
        {
            return iAccountAccess.GetBalance();     //call GetBalance in dll
        }

        public uint GetOwner()
        {
            return iAccountAccess.GetOwner();       //call GetOwner in dll
        }

        public void SelectAccount(uint accID)
        {
            iAccountAccess.SelectAccount(accID);    //call SelectAccount in dll
        }

        public void Withdraw(uint amount)
        {
            iAccountAccess.Withdraw(amount);        //call Withdraw in dll
        }
        // ---------------------End of Account Access ---------------------------


        // --------------------- Transaction Access ---------------------------------
        public uint CreateTransaction()
        {
            return iTransactionAccess.CreateTransaction();      //call CreateTransaction in dll
        }

        public uint GetAmount()
        {
            return iTransactionAccess.GetAmount();      //call GetAmount in dll
        }

        public List<uint> GetTransactions()
        {
            return iTransactionAccess.GetTransactions();        //call GetTransactions in dll
        }

        public uint GetReceiverAccount()
        {
            return iTransactionAccess.GetRecvrAcct();       //call GetRecvrAcct in dll
        }

        public uint GetSenderAccount()
        {
            return iTransactionAccess.GetSendrAcct();       //call GetSendrAcct in dll
        }

        public void SelectTransaction(uint transactionID)
        {
            iTransactionAccess.SelectTransaction(transactionID);        //call SelectTransaction in dll
        }

        public void SetAmount(uint amount)
        {
            iTransactionAccess.SetAmount(amount);       //call SetAmount in dll
        }

        public void SetReceiver(uint accountID)
        {
            iTransactionAccess.SetRecvr(accountID);     //call SetRecvr in dll
        }

        public void SetSender(uint accountID)
        {
            iTransactionAccess.SetSendr(accountID);     //call SetSendr in dll
        }
        // ---------------------End of Transaction Access ------------------------------
    }
}
