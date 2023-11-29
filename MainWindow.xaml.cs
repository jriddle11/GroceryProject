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
//graph bits
using OxyPlot;
using OxyPlot.Series;

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

        /// <summary>
        /// hides all tabs
        /// </summary>
        public void HideAllTabs()
        {
            //add all tabs here
            ReciptCtrl.Visibility = Visibility.Collapsed;
            LoginCtrl.Visibility = Visibility.Collapsed;
            

        }

        /// <summary>
        /// Passes login info and logs in
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void LoginDisplayChange(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            ReciptCtrl.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Holder empty click event
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void NAClick(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Handles account click button
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void AccountClick(object sender, RoutedEventArgs e)
        {
            HideAllTabs();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void ReceiptClick(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            ReciptCtrl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void ThreeClick(object sender, RoutedEventArgs e)
        {
            HideAllTabs();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void FourClick(object sender, RoutedEventArgs e)
        {
            HideAllTabs();

        }

        /// <summary>
        /// Handles logout click button
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void LogoutClick(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            LoginCtrl.Visibility = Visibility.Visible;
        }
    }
}
