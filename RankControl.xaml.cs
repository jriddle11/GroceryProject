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
        public DataCollection UserDataBoard = new DataCollection();
        public DataCollection StoreDataBoard = new DataCollection();
        public DataCollection ItemDataBoard = new DataCollection();


        public RankControl()
        {
            InitializeComponent();
            usersList.DataContext = UserDataBoard;
            storesList.DataContext = StoreDataBoard;
            itemsList.DataContext = ItemDataBoard;
            List<string> items = new List<string>();
            items.Add("jariddle@ksu.edu");
            items.Add("avaller@ksu.edu");
            items.Add("Kaelpav@ksu.edu");
            NewUserBoard(items);
        }

        public void NewUserBoard(List<string> list)
        {
            UserDataBoard.Clear();
            int rank = 1;
            foreach (string item in list)
            {
                UserDataBoard.Add("#" + rank + "   " + item);
                ++rank;
            }
        }

        public void NewItemBoard(List<string> list)
        {
            ItemDataBoard.Clear();
            int rank = 1;
            foreach (string item in list)
            {
                ItemDataBoard.Add("#" + rank + "   " + item);
                ++rank;
            }
        }

        public void NewStoreBoard(List<string> list)
        {
            StoreDataBoard.Clear();
            int rank = 1;
            foreach (string item in list)
            {
                StoreDataBoard.Add("#" + rank + "   " + item);
                ++rank;
            }
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
