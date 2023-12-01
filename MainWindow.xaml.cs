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
        public string UserId;
        public string Email;
        
        public MainWindow()
        {
            InitializeComponent();
            LoginCtrl.Main = this;
            ReciptCtrl.Main = this;
            AccountCtrl.Main = this;
            RankCtrl.Main = this;
            GraphsCtrl.Main = this;
        }

        /// <summary>
        /// hides all tabs
        /// </summary>
        public void HideAllTabs()
        {
            //add all tabs here
            ReciptCtrl.Visibility = Visibility.Collapsed;
            LoginCtrl.Visibility = Visibility.Collapsed;
            AccountCtrl.Visibility = Visibility.Collapsed;
            RankCtrl.Visibility = Visibility.Collapsed;
            AdminCtrl.Visibility = Visibility.Collapsed;
            GraphsCtrl.Visibility = Visibility.Collapsed;
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
        /// Passes login info and logs in
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void RankDisplayChange(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            RankCtrl.OpenRanksPage();
            RankCtrl.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Switches to admin
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void AdminDisplayChange(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            AdminCtrl.OpenAdminPage();
            AdminCtrl.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Moves user to Account page
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void AccountDisplayChange(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            AccountCtrl.OpenAccountPage();
            AccountCtrl.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Moves user to Account page
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void GraphDisplayChange(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            GraphsCtrl.Visibility = Visibility.Visible;

        }

        /// <summary>
        /// Handles account click button
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void AccountClick(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            AccountDisplayChange(sender, e);
        }

        /// <summary>
        /// Handles graph click button
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void GraphClick(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            GraphDisplayChange(sender, e);
        }

        /// <summary>
        /// Handles admin click button
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void AdminClick(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            AdminDisplayChange(sender, e);
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
        public void RankClick(object sender, RoutedEventArgs e)
        {
            HideAllTabs();
            RankDisplayChange(sender, e);

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
