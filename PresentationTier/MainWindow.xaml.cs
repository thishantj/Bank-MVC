using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Data_tier;
using Business_Tier;
using System.ServiceModel;
using System.Reflection;

namespace PresentationTier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BusinessServerInterface businessInterface;

        //Constructor
        public MainWindow()
        {
            InitializeComponent();
        }

        //Create a user ID
        private void onClick(object sender, RoutedEventArgs e)
        {
            uint userID = businessInterface.createUser();       //calls the create user method in business tier

            txtUserID.Text = userID.ToString();     //saves user ID to text field

            businessInterface.open();       //calls the open method in business tier
        }

        //When window loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChannelFactory<BusinessServerInterface> dataFactory; //opening a server connection

            NetTcpBinding tcpBinding = new NetTcpBinding();

            string sURL = "net.tcp://localhost:50010/BusinessTier";

            //Loads to the list view
            dataFactory = new ChannelFactory<BusinessServerInterface>(tcpBinding, sURL);
            businessInterface = dataFactory.CreateChannel();

        }

        //Save user name
        private void setUser_onClick(object sender, RoutedEventArgs e)
        {
            //saves name to a variable
            string fname = txtFname.Text;
            string lname = txtLname.Text;

            if(txtUserID.Text.Equals(""))
            {
                MessageBox.Show("Please enter a UserID!");
            }
            else
            {
                businessInterface.SetUserName(fname, lname, Convert.ToUInt32(txtUserID.Text));  //calls the set User name method in business tier
                MessageBox.Show("User's name added successfully");
                txtFname.Text = null;
                txtLname.Text = null;
            }
        }

        //Load user name
        private void getUser_onClick(object sender, RoutedEventArgs e)
        {
            string fname = null;
            string lname = null;

            if(txtUserID.Text.Equals(""))
            {
                MessageBox.Show("Please enter a UserID to search!");
            }
            else
            {
                businessInterface.GetUserName(out fname, out lname, Convert.ToUInt32(txtUserID.Text));      //calls the get user name method in business tier
                businessInterface.open();       //calls the open method in business tier

                //saves to a text field
                txtFname.Text = fname;
                txtLname.Text = lname;
            }
        }

        //Proceeds to accounts interface
        private void accountsTab(object sender, RoutedEventArgs e)
        {
            if(txtUserID.Text.Equals(""))
            {
                MessageBox.Show("Cannot proceed. Please enter/create a user ID");
            }
            else
            {
                string fname = null;
                string lname = null;

                businessInterface.GetUserName(out fname, out lname, Convert.ToUInt32(txtUserID.Text));      //calls the open method in business tier
                businessInterface.open();
                txtFname.Text = fname;
                txtLname.Text = lname;

                //opens accounts tab
                var accTab = new Account(txtUserID.Text);
                accTab.Show();
                this.Hide();
            }
        }

        private void transactionClick(object sender, RoutedEventArgs e)
        {
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //When window closing
        private void onClosingUser(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //calls the close method in business tier
            businessInterface.closing();
        }

    }
}
