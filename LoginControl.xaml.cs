using System;
using System.Collections.Generic;
using System.Linq;
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
using Newtonsoft.Json;

namespace GroceryProject
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public MainWindow? Main;
        public LoginControl()
        {
            InitializeComponent();
        }

        private void GenerateData()
        {
            Random random = new Random();
            List<string> emails = ReceiptGenerator.GenerateEmails();
            List<Receipt> receipts;
            foreach(string genEmail in emails)
            {
                int count = random.Next(2, 40);
                receipts = ReceiptGenerator.GenerateReceipts(count);
                Server.Request(
                "/insert_user",
                new { email = genEmail, password = "password" },
                (string response) => { UploadGeneratedReceipts(response, receipts); }
                );
            }
        }

        private void UploadGeneratedReceipts(string id, List<Receipt> list)
        {
            if(id != "-1")
            {
                foreach(Receipt receipt in list)
                {
                    string json = JsonConvert.SerializeObject(receipt);
                    Server.Request("/log_receipt", new { Receipt = json, UserId = id }, (string response) => {
                        
                    });
                }
            }
        }

        /// <summary>
        /// Passes login info and logs in
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void LoginClick(object sender, RoutedEventArgs e)
        {
            if (Main != null)
            {
                string userEmail = user.Text;
                string userPassword = pass.Text;
                Server.Request(
                "/insert_user",
                new { email = userEmail, password = userPassword },
                (string response) => { SetId(response, sender, e); }
                );
                Main.Email = userEmail;
                user.Text = "";
                pass.Text = "";
                Main.LoginDisplayChange(sender, e); //remove when server is connected
            }

        }

        private void SetId(string message, object sender, RoutedEventArgs e)
        {
            if (message != "-1")
            {
                Main.UserId = message;
                //Main.LoginDisplayChange(sender, e);    Add back in when server is connected
            }
            else
            {
                Main.LogoutClick(sender, e);
            }
        }
    }
}
