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
            totalUsers.Text = "";
            totalItems.Text = "";
            totalReceipts.Text = "";
            totalSpent.Text = "";
        }
    }
}