﻿using OxyPlot.Series;
using OxyPlot;
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
using Newtonsoft.Json;

namespace GroceryProject
{
    /// <summary>
    /// Interaction logic for ReciptProcessor.xaml
    /// </summary>
    public partial class ReciptProcessor : UserControl
    {
        public MainWindow? Main;
        public ReciptProcessor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// creates the graph
        /// </summary>
        public void CreateGraph(LineSeries line)
        {
            // Create a PlotModel and add the LineSeries
            var plotModel = new PlotModel { Title = "Grocery Costs" };
            plotModel.Series.Add(line);

            // Set the PlotModel to the PlotView
            //lineGraph.Model = plotModel;
        }

        /// <summary>
        /// clear the graph
        /// </summary>
        public void ClearGraphClick(object sender, RoutedEventArgs e)
        {
            // Initialize and set up the OxyPlot LineSeries
            var lineSeries = new LineSeries
            {
                Title = "Random Data",
                MarkerType = MarkerType.Circle,
                Color = OxyColor.FromRgb(0, 0, 255)
            };

            CreateGraph(lineSeries);
        }

        /// <summary>
        /// clear the graph
        /// </summary>
        public void GenerateGraphClick(object sender, RoutedEventArgs e)
        {
            // Initialize and set up the OxyPlot LineSeries
            var lineSeries = new LineSeries
            {
                Title = "Random Data",
                MarkerType = MarkerType.Circle,
                Color = OxyColor.FromRgb(0, 0, 255)
            };

            // Add data points can bed one via function
            lineSeries.Points.Add(new DataPoint(1, 5));
            lineSeries.Points.Add(new DataPoint(2, 10));
            lineSeries.Points.Add(new DataPoint(3, 8));
            lineSeries.Points.Add(new DataPoint(4, 12));

            CreateGraph(lineSeries);
        }

        public async void SelectImage(object sender, RoutedEventArgs e)
        {

            ImageReader reader = new ImageReader();
            reader.OpenImage();
            var task = Task.Run(async delegate {
                await reader.ReadImage();
            });
            await task;
            ReceiptReader receiptReader = new ReceiptReader(reader);
            Receipt receipt = new Receipt(receiptReader);
            totalBox.Text = "$" + receipt.Total;
            itemCountBox.Text = receipt.PurchasedItems.Count + "";
            string json = JsonConvert.SerializeObject(receipt);
            Server.Request("/log_receipt", new { Receipt = json, UserId = Main.UserId }, (string response) => {
                MessageBox.Show("Receipt upload success!");
            });
            foreach(string s in receiptReader.ReceiptLines)
            {
                itemsBox.Text += s + "\n";
            }

        }
    }
}
