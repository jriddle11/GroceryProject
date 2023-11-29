﻿using System;
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
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public MainWindow? Main;
        public LoginControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Passes login info and logs in
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void LoginClick(object sender, RoutedEventArgs e)
        {
            if (Main != null)
            {
                Main.LoginDisplayChange(sender, e);
            }

        }
    }
}