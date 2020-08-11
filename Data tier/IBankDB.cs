using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Data_tier
{
    [ServiceContract]
    public interface IBankDB
    {
        // --------------------- User Access ---------------------------------
        [OperationContract]
        void SavetoDisk();

        [OperationContract]
        void ProcessAllTransactions();
        // ---------------------End of User Access ------------------------------


        // --------------------- User Access ---------------------------------
        [OperationContract]
        void SelectUser(uint userID);

        [OperationContract]
        List<uint> GetUsers();

        [OperationContract]
        uint CreateUser();

        [OperationContract]
        void GetUserName(out string firstName, out string lastName);

        [OperationContract]
        void SetUserName(string fname, string lname);
        // ---------------------End of User Access ------------------------------


        // --------------------- Account Access ---------------------------------
        [OperationContract]
        void SelectAccount(uint accID);

        [OperationContract]
        List<uint> GetAccountIDsByUser(uint userID);

        [OperationContract]
        uint CreateAccount(uint userID);

        [OperationContract]
        void Deposit(uint amount);

        [OperationContract]
        void Withdraw(uint amount);

        [OperationContract]
        uint GetBalance();

        [OperationContract]
        uint GetOwner();
        // ---------------------End of Account Access ---------------------------


        // --------------------- Transaction Access ---------------------------------
        [OperationContract]
        void SelectTransaction(uint transactionID);

        [OperationContract]
        List<uint> GetTransactions();

        [OperationContract]
        uint CreateTransaction();

        [OperationContract]
        uint GetAmount();

        [OperationContract]
        uint GetSenderAccount();

        [OperationContract]
        uint GetReceiverAccount();

        [OperationContract]
        void SetAmount(uint amount);

        [OperationContract]
        void SetSender(uint accountID);

        [OperationContract]
        void SetReceiver(uint accountID);
        // ---------------------End of Transaction Access ------------------------------
    }
}

