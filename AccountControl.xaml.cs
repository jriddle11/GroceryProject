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
    /// Interaction logic for AccountControl.xaml
    /// </summary>
    public partial class AccountControl : UserControl
    {
        public MainWindow? Main;
        public AccountControl()
        {
            InitializeComponent();
        }

        public void OpenAccountPage()
        {
            userid.Text = Main.UserId;
            useremail.Text = Main.Email;
            Server.Request(
                "/user_information",
                new { UserId = 2 },
                (string response) => {
                    (string, string, string, decimal, int) result = JsonConvert.DeserializeObject<List<(string, string, string, decimal, int)>>(response)[0];
                    //UserId, Email, JoinDate, Spent, UploadedRecipts
                    userjoin.Text = result.Item3;
                    userspent.Text = "$" + result.Item4;
                    useruploaded.Text = "" + result.Item5;
                }
           );

        }
    }
}
