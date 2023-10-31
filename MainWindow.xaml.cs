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
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.gif; *.bmp)| *.jpg; *.jpeg; *.png; *.gif; *.bmp";

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                TextImager ts = new();
                string text = ts.ConvertImageToText(filename);
                text = text.ToUpper();
                richText.Text = text;
                string[] doc = text.Split('\n');
                address.Text = doc.Length + "";
                store.Text = ts.FindStoreName(doc);
                decimal[] totals = ts.FindTaxAndTotals(doc);
                subtotal.Text = totals[0] + "";
                total.Text = totals[1] + "";
                tax1.Text = totals[2] + "";
                tax2.Text = totals[3] + "";
                List<string[]> items = ts.FindItems(doc);
                foreach(string[] item in items)
                {
                    itemsBox.Text += item[0] + "     " + item[1] + "    $" + item[2] + '\n';
                }
                itemsBox.Text += items.Count;
            }
        }
    }
}
