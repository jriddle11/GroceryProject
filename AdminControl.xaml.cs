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
    /// Interaction logic for AdminControl.xaml
    /// </summary>
    public partial class AdminControl : UserControl
    {
        private DataCollection UserEmailsAndPasswords = new DataCollection();
        public AdminControl()
        {
            InitializeComponent();
            usersList.DataContext = UserEmailsAndPasswords;
        }
        public void OpenAdminPage()
        {
            UserEmailsAndPasswords = new DataCollection();
            usersList.DataContext = UserEmailsAndPasswords;
            Server.Request(
                "/all_users",
                new { },
                (string response) => {
                    List<(string, string)> result = JsonConvert.DeserializeObject<List<(string, string)>>(response);

                    foreach ((string, string) l in result)
                    {
                        UserEmailsAndPasswords.Add(l.Item1 + "   " + l.Item2);
                    }
                });
            Server.Request(
                "/count_users",
                new { },
                (string response) => {
                    int result = JsonConvert.DeserializeObject<int>(response);

                    totalUsers.Text = result + "";
                });
            Server.Request(
                "/count_items",
                new { },
                (string response) => {
                    int result = JsonConvert.DeserializeObject<int>(response);

                    totalItems.Text = result + "";
                });
            Server.Request(
                "/count_receipts",
                new { },
                (string response) => {
                    int result = JsonConvert.DeserializeObject<int>(response);

                    totalReceipts.Text = result + "";
                });
            Server.Request(
                "/count_items",
                new { },
                (string response) => {
                    int result = JsonConvert.DeserializeObject<int>(response);

                    totalItems.Text = result + "";
                });
            Server.Request(
                "/count_spent",
                new { },
                (string response) => {
                    decimal result = JsonConvert.DeserializeObject<decimal>(response);

                    totalSpent.Text = "$" + result + "";
                });
        }
    }
}