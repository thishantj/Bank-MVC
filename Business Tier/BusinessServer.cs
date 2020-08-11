using Data_tier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using BankDB;
using System.Threading;

namespace Business_Tier
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public class BusinessServer : BusinessServerInterface
    {
        IBankDB ibank;

        // --------------------- Connections to Data tier ---------------------------------
        public void ConnectBankDB()
        {
            ChannelFactory<IBankDB> dataFactory; //opening a server connection

            NetTcpBinding tcpBinding = new NetTcpBinding();

            string sURL = "net.tcp://localhost:50001/DataTier";

            dataFactory = new ChannelFactory<IBankDB>(tcpBinding, sURL);
            ibank = dataFactory.CreateChannel();
        }
        // --------------------- End of connections to Data tier ---------------------------------


        // --------------------- User Access ---------------------------------
        public uint createUser()
        {
            ConnectBankDB();                    //calls connectBankDB to connect to data tier

            uint usr = ibank.CreateUser();      //gets a user ID by calling createUser in data tier
            ibank.SavetoDisk();                 //calls SaveToDisk in data tier to save details
            return usr;                         //returns user ID
        }

        public void GetUserName(out string fname, out string lname, uint userID)
        {
            ConnectBankDB();                    //calls connectBankDB to connect to data tier

            ibank.SelectUser(userID);
            ibank.GetUserName(out fname, out lname);    //returns user's name
        }

        public void SelectUser(uint userID)
        {
            ConnectBankDB();                    //calls connectBankDB to connect to data tier

            ibank.SelectUser(userID);           //selects user
        }

        public void SetUserName(string fname, string lname, uint userID)
        {
            ConnectBankDB();                    //calls connectBankDB to connect to data tier

            ibank.SelectUser(userID);           //selects user
            ibank.SetUserName(fname, lname);    //saves user name
            ibank.SavetoDisk();                 //calls SaveToDisk in data tier to save details
        }
        // ---------------------End of User Access ------------------------------


        // --------------------- Account Access ---------------------------------
        public void selectAccount(uint accID)
        {
            ConnectBankDB();                    //calls connectBankDB to connect to data tier

            ibank.SelectAccount(accID);         //selects Account
        }

        public List<uint> getAccountIDsByUser(uint uID)
        {
            ConnectBankDB();                    //calls connectBankDB to connect to data tier

            return ibank.GetAccountIDsByUser(uID);  //returns a list of account IDs for a user
        }

        public uint createAcc(uint ID)
        {
            ConnectBankDB();                    //calls connectBankDB to connect to data tier

            uint acc = ibank.CreateAccount(ID); //gets a acc ID by calling createAccount in data tier
            ibank.SavetoDisk();                 //calls SaveToDisk in data tier to save details
            return acc;                         //returns created account ID
        }

        public void Deposit(uint amnt, uint accID)
        {
            ConnectBankDB();                    //calls connectBankDB to connect to data tier

            ibank.SelectAccount(accID);         //selects account ID
            ibank.Deposit(amnt);                ////gets a user ID by calling createUser in data tier

            ibank.SavetoDisk();                 //calls SaveToDisk in data tier to save details
        }

        public void Withdraw(uint amnt, uint accID, uint bal)
        {
            ConnectBankDB();                    //calls connectBankDB to connect to data tier

            ibank.SelectAccount(accID);         //selects account ID
            if (bal < amnt)
            {
                Console.WriteLine("Cannot process transaction. Low money!");
            }
            else
            {
                ibank.Withdraw(amnt);           //calls withdraw to connect to data tier

                ibank.SavetoDisk();             //calls SaveToDisk in data tier to save details
            }
        }

        public uint GetBalance(uint accID)
        {
            ConnectBankDB();                    //calls connectBankDB to connect to data tier

            ibank.SelectAccount(accID);         //selects account ID

            ibank.SavetoDisk();                 //calls SaveToDisk in data tier to save details
            return ibank.GetBalance();          //returns balance
        }
        // ---------------------End of Account Access ---------------------------


        // --------------------- Transaction Access ---------------------------------
        public void SelectTransaction(uint transactionID)
        {
            ConnectBankDB();                    //calls connectBankDB to connect to data tier

            ibank.SelectTransaction(transactionID);     //selects transaction
        }

        public List<uint> Getransactions(uint accID)
        {
            return filterTransactions(accID).Result;        //returns list of transactions
        }

        public uint createTransaction()
        {
            ConnectBankDB();                //calls connectBankDB to connect to data tier

            return ibank.CreateTransaction();       //returns created transaction ID
        }

        public uint GetAmount(uint transactionID)
        {
            ConnectBankDB();                //calls connectBankDB to connect to data tier

            ibank.SelectTransaction(transactionID);     //selects transaction
            return ibank.GetAmount();                   //returns amount
        }

        public uint GetSenderAccount(uint transactionID)
        {
            ConnectBankDB();                //calls connectBankDB to connect to data tier

            ibank.SelectTransaction(transactionID);     //selects transaction
            return ibank.GetSenderAccount();            //returns senders account ID
        }

        public uint GetReceiverAccount(uint transactionID)
        {
            ConnectBankDB();                //calls connectBankDB to connect to data tier

            ibank.SelectTransaction(transactionID);     //selects transaction
            return ibank.GetReceiverAccount();          //returns created account ID
        }

        public void setAmount(uint amount, uint transactionID, uint bal)
        {
            ConnectBankDB();                //calls connectBankDB to connect to data tier

            ibank.SelectTransaction(transactionID);     //selects transaction

            if (bal < amount)
            {
                Console.WriteLine("Cannot process transaction. Low money amount");
            }
            else
            {
                ibank.SetAmount(amount);        //sets amount
            }
        }

        public void setSender(uint accountID, uint transactionID)
        {
            ConnectBankDB();                //calls connectBankDB to connect to data tier

            ibank.SelectTransaction(transactionID);     //selects transaction
            ibank.SetSender(accountID);
        }

        public void setReceiver(uint accountID, uint transactionID)
        {
            ConnectBankDB();                //calls connectBankDB to connect to data tier

            ibank.SelectTransaction(transactionID);     //selects transaction
            ibank.SetReceiver(accountID);               //sets receiver account
        }

        public void closing()
        {
            ConnectBankDB();                //calls connectBankDB to connect to data tier

            ibank.ProcessAllTransactions();     //calls processAllTransactions in data tier to process transactions
            ibank.SavetoDisk();             //calls SaveToDisk in data tier to save details
        }

        public void open()
        {
            ConnectBankDB();                //calls connectBankDB to connect to data tier

            int startTimeSpan = 0;
            int periodTimeSpan = 15000;

            var timer = new System.Threading.Timer((e) =>
            {
                ibank.SavetoDisk();         //calls SaveToDisk in data tier to save details
            }, null, startTimeSpan, periodTimeSpan);       //repeats after x seconds
        }

        public async Task<List<uint>> filterTransactions(uint accID)
        {   
            ConnectBankDB();                //calls connectBankDB to connect to data tier

            //filters tranasctions
            List<uint> resultlist = await Task.Run(() =>
            {
                List<uint> filteredList = new List<uint>();

                foreach (uint a in ibank.GetTransactions())
                {
                    ibank.SelectTransaction(a);

                    uint sender = ibank.GetSenderAccount();

                    if (accID.Equals(sender))
                    {
                        filteredList.Add(a);
                    }
                }

                return filteredList;
            });
            return resultlist;
        }
    // ---------------------End of Transaction Access ---------------------------
    }
}
