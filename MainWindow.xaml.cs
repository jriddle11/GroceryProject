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
            id.Text = r.Date;
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
