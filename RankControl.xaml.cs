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
using System.Collections.Specialized;

namespace GroceryProject
{
    /// <summary>
    /// Interaction logic for RankControl.xaml
    /// </summary>
    public partial class RankControl : UserControl
    {
        public List<string> users = new List<string>();


        public RankControl()
        {
            InitializeComponent();
            users.Add("josh");
            users.Add("kael");
            users.Add("alex");
            usersList.DataContext = users;
        }

        public void OnClickItemsArrange(object sender, EventArgs e)
        {
            users.Reverse();
        }
        public void OnClickUsersArrange(object sender, EventArgs e)
        {
            users.Reverse();
        }
        public void OnClickStoresArrange(object sender, EventArgs e)
        {
            users.Reverse();
        }
    }
}
