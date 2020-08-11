using Business_Tier;
using Data_tier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationTier
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        BusinessServerInterface businessInterface;

        //constructor
        public Account(string userID)
        {
            InitializeComponent();
            lUserID.Content = userID;
        }

        //Proceeds to transaction interface
        private void transactionsClick(object sender, RoutedEventArgs e)
        {
            if (txtAccID.Text.Equals(""))
            {
                MessageBox.Show("Cannot proceed. Please select an account ID");
            }
            else
            {
                //moves to transaction tab
                var tTab = new Transaction(Convert.ToString(txtAccID.Text), Convert.ToString(lUserID.Content));
                tTab.Show();
                this.Hide();
            }
        }

        //Create account ID
        private void createAcc(object sender, RoutedEventArgs e)
        {
            uint accID = businessInterface.createAcc(Convert.ToUInt32(lUserID.Content));        //calls the create account method in business tier

            txtAccID.Text = accID.ToString();

            //loads to the list view
            List<uint> accounts = businessInterface.getAccountIDsByUser(Convert.ToUInt32(lUserID.Content));
            listView.ItemsSource = accounts;
        }

        //loads connection to business tier
        private void Window_Loaded_Acc(object sender, RoutedEventArgs e)
        {
            ChannelFactory<BusinessServerInterface> dataFactory; //opening a server connection

            NetTcpBinding tcpBinding = new NetTcpBinding();

            string sURL = "net.tcp://localhost:50010/BusinessTier";

            dataFactory = new ChannelFactory<BusinessServerInterface>(tcpBinding, sURL);
            businessInterface = dataFactory.CreateChannel();

            var t = Task.Run(async delegate
            {
                await Task.Delay(TimeSpan.FromSeconds(0.4));
            });
            t.Wait();

            //calls the open method in business tier
            businessInterface.open();

            //loads to the list view
            List<uint> accounts = businessInterface.getAccountIDsByUser(Convert.ToUInt32(lUserID.Content));
            listView.ItemsSource = accounts;
        }

        //deposit money method
        private void deposit(object sender, RoutedEventArgs e)
        {
            if(txtAmount.Text.Equals("") || txtAccID.Text.Equals(""))
            {
                MessageBox.Show("Fill all the fields");
            }
            else
            {
                uint accid = Convert.ToUInt32(txtAccID.Text);
                businessInterface.selectAccount(accid);             //calls the select account method in business tier

                uint amount = Convert.ToUInt32(txtAmount.Text);

                businessInterface.Deposit(amount, Convert.ToUInt32(txtAccID.Text));        //calls the deposit method in business tier

                uint a = businessInterface.GetBalance(Convert.ToUInt32(txtAccID.Text));        //calls the get balance method in business tier

                lbalance.Content = a;

                txtAmount.Text = null;
            }
        }

        //withdraw money method
        private void withdraw(object sender, RoutedEventArgs e)
        {
            if (txtAmount.Text.Equals("") || txtAccID.Text.Equals(""))
            {
                MessageBox.Show("Fill all the fields");
            }
            else
            {
                uint accid = Convert.ToUInt32(txtAccID.Text);
                businessInterface.selectAccount(accid);             //calls the select account method in business tier

                uint amount = Convert.ToUInt32(txtAmount.Text);
                uint bal = businessInterface.GetBalance(Convert.ToUInt32(txtAccID.Text));      //calls the open method in business tier

                if(bal < amount)
                {
                    MessageBox.Show("Low money! Cannot process transaction");
                }
                else
                {
                    businessInterface.Withdraw(amount, Convert.ToUInt32(txtAccID.Text), bal);      //calls the withdraw method in business tier

                    uint a = businessInterface.GetBalance(Convert.ToUInt32(txtAccID.Text));        //calls the get balance method in business tier
                    lbalance.Content = a;

                    txtAmount.Text = null;
                }
            }
        }

        //selection of account ID in the list
        private void selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            uint accid = Convert.ToUInt32(listView.SelectedItem);
            businessInterface.selectAccount(accid);                 //calls the select account method in business tier
            txtAccID.Text = accid.ToString();
            lbalance.Content = businessInterface.GetBalance(Convert.ToUInt32(listView.SelectedItem));       //calls the get balance method in business tier
        }

        //Logout
        private void usersClick(object sender, RoutedEventArgs e)
        {
            //moves to user tab(logout)
            var usr = new MainWindow();
            usr.Show();
            this.Hide();
        }

        //when closing the window
        private void onClosingAccount(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //calls the close method in business tier
            businessInterface.closing();
        }
    }
}
