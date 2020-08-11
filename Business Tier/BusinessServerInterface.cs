using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Business_Tier
{
    [ServiceContract]
    public interface BusinessServerInterface
    {
        // --------------------- Connections to Data tier ---------------------------------
        [OperationContract]
        void ConnectBankDB();
        // --------------------- End of connections to Data tier ---------------------------------


        // --------------------- User Access ---------------------------------
        [OperationContract]
        void SelectUser(uint userID);

        [OperationContract]
        uint createUser();

        [OperationContract]
        void GetUserName(out string fname, out string lname, uint userID);

        [OperationContract]
        void SetUserName(string fname, string lname, uint userID);
        // ---------------------End of User Access ------------------------------


        // --------------------- Account Access ---------------------------------
        [OperationContract]
        void selectAccount(uint accID);

        [OperationContract]
        List<uint> getAccountIDsByUser(uint uID);

        [OperationContract]
        uint createAcc(uint ID);

        [OperationContract]
        void Deposit(uint amnt, uint accID);

        [OperationContract]
        void Withdraw(uint amnt, uint accID, uint bal);

        [OperationContract]
        uint GetBalance(uint accID);
        // ---------------------End of Account Access ---------------------------


        // --------------------- Transaction Access ---------------------------------
        [OperationContract]
        void SelectTransaction(uint transactionID);

        [OperationContract]
        List<uint> Getransactions(uint accID);

        [OperationContract]
        uint createTransaction();

        [OperationContract]
        uint GetAmount(uint transactionID);

        [OperationContract]
        uint GetSenderAccount(uint transactionID);

        [OperationContract]
        uint GetReceiverAccount(uint transactionID);

        [OperationContract]
        void setAmount(uint amount, uint transactionID, uint bal);

        [OperationContract]
        void setSender(uint accountID, uint transactionID);

        [OperationContract]
        void setReceiver(uint accountID, uint transactionID);

        [OperationContract]
        void closing();

        [OperationContract]
        void open();
        // ---------------------End of Transaction Access ---------------------------

    }
}
