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

        public void SelectImage(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Scan"; // Default file name
            dialog.DefaultExt = ".jpg"; // Default file extension
            dialog.Filter = "Image Files(*.jpg; *.jpeg)| *.jpg; *.jpeg;";

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            ClearText();
            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                ReceiptReader receiptReader = new();
                receiptReader.Read(filename, 1);
                Receipt r = new Receipt(receiptReader);
                richText.Text = receiptReader.Text;
                lineCount.Text = receiptReader.ReceiptLines.Length + "";
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
                id.Text = r.ID;
            }
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
