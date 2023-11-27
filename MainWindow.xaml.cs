using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            

            double[] time = new double[] {new DateTime(2023, 5, 1).ToOADate(), new DateTime(2023, 5, 8).ToOADate(), new DateTime(2023, 5, 15).ToOADate(), new DateTime(2023, 5, 22).ToOADate() };
            double[] price = new double[] { 120, 150, 222.3, 180.5 };
            CostOverTime.Plot.XAxis.DateTimeFormat(true);
            CostOverTime.Plot.AddScatter(time, price);
            CostOverTime.Refresh();
            UpdateLeaderboard(new string[] { "Monster", "Broc", "Chicken", "Eggs" }, new decimal[] { 200.32m, 119.2m, 115m, 102.23m });
        }  

        public void UpdateLeaderboard(string[] names, decimal[] totals)
        {
            leaderboard.Text = "Item Purchase Leaderboard\n";
            for(int i = 0; i < names.Length; i++)
            {
                leaderboard.Text += (i + 1) + ". " + names[i] + " $" + totals[i] + "\n";
            }
        }

        public async void SelectImage(object sender, RoutedEventArgs e)
        {

            ImageReader reader = new ImageReader();
            ClearText();
            reader.OpenImage();
            var task = Task.Run(async delegate {
                await reader.ReadImage();
            });
            await task;
            richText.Text = reader.Text;
            ReceiptReader receiptReader = new ReceiptReader(reader);
            Receipt fresh = new Receipt(receiptReader);
            string json = JsonConvert.SerializeObject(fresh);
            Receipt r = JsonConvert.DeserializeObject<Receipt>(json);
            store.Text = r.StoreName;
            subtotal.Text = r.SubTotal + "";
            total.Text = r.Total + "";
            tax1.Text = r.Tax1 + "";
            tax2.Text = r.Tax2 + "";
            List<PurchasedItem> items = r.PurchasedItems;
            foreach(PurchasedItem item in items)
            {
                itemsBox.Text += item.Name + "     " + item.Code + "    $" + item.Price + '\n';
            }
            itemsBox.Text += items.Count;
            lineCount.Text = receiptReader.ReceiptLines.Length + "";
            id.Text = r.ReceiptDate.ToString();
            box.Text = r.Street + " " + r.City + " " + r.State + " " + r.PostalCode;

        }

        private void ClearText()
        {
            richText.Text = "";
            lineCount.Text = "LineCount";
            store.Text = "StoreName";
            subtotal.Text = "subtotal";
            total.Text = "total";
            tax1.Text = "tax1";
            tax2.Text = "tax2";
            itemsBox.Text = "";
        }
    }
}
