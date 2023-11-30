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
using Newtonsoft.Json;

namespace GroceryProject
{
    /// <summary>
    /// Interaction logic for RankControl.xaml
    /// </summary>
    public partial class RankControl : UserControl
    {
        public MainWindow? Main;
        private DataCollection UserDataBoard = new DataCollection();
        private DataCollection StoreDataBoard = new DataCollection();
        private DataCollection ItemDataBoard = new DataCollection();


        public RankControl()
        {
            InitializeComponent();
            usersList.DataContext = UserDataBoard;
            storesList.DataContext = StoreDataBoard;
            itemsList.DataContext = ItemDataBoard;
        }

        private void NewUserBoard(List<string> list)
        {
            UserDataBoard.Clear();
            int rank = 1;
            foreach (string item in list)
            {
                UserDataBoard.Add("#" + rank + "   " + item);
                ++rank;
            }
        }

        private void NewItemBoard(List<string> list)
        {
            ItemDataBoard.Clear();
            int rank = 1;
            foreach (string item in list)
            {
                ItemDataBoard.Add("#" + rank + "   " + item);
                ++rank;
            }
        }

        private void NewStoreBoard(List<string> list)
        {
            StoreDataBoard.Clear();
            int rank = 1;
            foreach (string item in list)
            {
                StoreDataBoard.Add("#" + rank + "   " + item);
                ++rank;
            }
        }

        public void OpenRanksPage()
        {
            Server.Request(
                "/all_items_from_user",
                new { UserId = Main.UserId },
                (string response) => {
                    List<(string, decimal)> result = JsonConvert.DeserializeObject<List<(string, decimal)>>(response);
                    List<string> list = new List<string>();
                    foreach ((string, decimal) l in result)
                    {
                        list.Add(l.Item1 + "    $" + l.Item2.ToString());
                    }
                    NewItemBoard(list);
                });
            Server.Request(
                "/rank_stores",
                new { UserId = Main.UserId },
                (string response) => {
                    List<(string, string, string, decimal)> result = JsonConvert.DeserializeObject<List<(string, string, string, decimal)>>(response);
                    //Name street phone money
                    List<string> list = new List<string>();
                    foreach ((string, string, string, decimal) l in result)
                    {
                        list.Add(l.Item1 + "    $" + l.Item4.ToString());
                    }
                    NewStoreBoard(list);
                });
            Server.Request(
                "/rank_user",
                new { UserId = Main.UserId },
                (string response) => {
                    List<(string, decimal)> result = JsonConvert.DeserializeObject<List<(string, decimal)>>(response);
                    List<string> list = new List<string>();
                    foreach ((string, decimal) l in result)
                    {
                        list.Add(l.Item1 + "    $" + l.Item2.ToString());
                    }
                    NewUserBoard(list);
                });
        }

        public void OnClickItemsArrange(object sender, EventArgs e)
        {
            ItemDataBoard.Reverse();
        }
        public void OnClickUsersArrange(object sender, EventArgs e)
        {
            UserDataBoard.Reverse();
        }
        public void OnClickStoresArrange(object sender, EventArgs e)
        {
            StoreDataBoard.Reverse();
        }
    }
}
