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
            Receipt r = new Receipt(receiptReader);
            store.Text = r.StoreName;
            subtotal.Text = r.SubTotal + "";
            total.Text = r.Total + "";
            tax1.Text = r.Tax1 + "";
            tax2.Text = r.Tax2 + "";
            List<string[]> items = r.PurchasedItems;
            foreach(string[] item in items)
            {
                itemsBox.Text += item[0] + "     " + item[1] + "    $" + item[2] + '\n';
            }
            itemsBox.Text += items.Count;
            lineCount.Text = receiptReader.ReceiptLines.Length + "";
            id.Text = r.datetemp;
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
