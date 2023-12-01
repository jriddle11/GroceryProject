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
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Globalization;
using OxyPlot.Wpf;
using Newtonsoft.Json;

namespace GroceryProject
{
    /// <summary>
    /// Interaction logic for GraphsControl.xaml
    /// </summary>
    public partial class GraphsControl : UserControl
    {
        public MainWindow? Main;
        public GraphsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Holder empty click event
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void PurchasesClick(object sender, RoutedEventArgs e)
        {
            // Create a PlotModel and add the LineSeries
            var plotModel = new PlotModel { };

            // Initialize and set up the OxyPlot LineSeries
            var lineSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                Color = OxyColor.FromRgb(0, 0, 255),
                MarkerStroke = OxyColors.Blue, //Set marker stroke color
                MarkerFill = OxyColors.LightBlue //Set marker fill color
            };

            // Create the LinearAxis (Y-axis)
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Cost (Dollars)", //Set Y-axis title
                //StringFormat = "${0:N2}" //Format the axis labels as currency --not wanting to work
            };
            plotModel.Axes.Add(yAxis);

            // Create the LinearAxis (X-axis)
            var xAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Date"
            };
            plotModel.Axes.Add(xAxis);

            // Add data points can bed one via function
            Server.Request(
                "/user_purchases",
                new { UserId = Main.UserId },
                (string response) => {
                    List<(string, decimal, DateTime)> result = JsonConvert.DeserializeObject<List<(string, decimal, DateTime)>>(response);
                    foreach ((string, decimal, DateTime) l in result)
                    {
                        lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(l.Item3), (double)l.Item2));
                    }
                    plotModel.Series.Add(lineSeries);
                    GraphSpot.Model = plotModel; // Set the PlotModel to the PlotView
                    graphTitle.Text = "Purchases over Time";
                }
            );
        }

        /// <summary>
        /// Holder empty click event
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void StoresClick(object sender, RoutedEventArgs e)
        {
            var plotModel = new PlotModel { };

            // Create a PieSeries
            var series = new PieSeries
            {
                FontSize = 10,
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };
            Random random = new Random();
            // Add data to the PieSeries
            Server.Request(
                "/rank_stores",
                new { UserId = Main.UserId },
                (string response) => {
                List<(string, string, string, decimal)> result = JsonConvert.DeserializeObject<List<(string, string, string, decimal)>>(response);
                //Name street phone money
                List<(string, decimal)> list = new List<(string, decimal)>();
                decimal total = 0;
                foreach ((string, string, string, decimal) l in result)
                {
                    total += l.Item4;
                    list.Add(new(l.Item1, l.Item4));
                }
                double totalpercentage = 0;
                foreach (var store in list)
                {
                        double percentage;
                        double.TryParse(""+ ((store.Item2 / total) * 100), out percentage);
                        totalpercentage += percentage;
                        series.Slices.Add(new PieSlice(store.Item1, percentage) { Fill = OxyColor.FromRgb(150,150, (byte)random.Next(150,255)) });
                }
                    // Set the label position to Outside

                    // Add the PieSeries to the PlotModel
                    plotModel.Series.Add(series);

                 // Set the PlotModel to the PlotView
                 GraphSpot.Model = plotModel;

                 graphTitle.Text = "Purchases per Store";
                });
        }

        private OxyColor GetRandomColor()
        {
            Random random = new Random();
            var fields = typeof(OxyColors).GetFields();
            return (OxyColor)fields[random.Next(0, fields.Length - 1)].GetValue(null);
        }

        /// <summary>
        /// Holder empty click event
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void BarClick(object sender, RoutedEventArgs e)
        {
            // Create the PlotModel
            var plotModel = new PlotModel { Title = "Grocery Item Costs" };

            // Create the CategoryAxis (Y-axis)
            var yAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Title = "Grocery Item"
            };

            // Add the CategoryAxis to the PlotModel
            plotModel.Axes.Add(yAxis);

            // Create the LinearAxis (X-axis)
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Cost (Dollars)",
                //StringFormat = "${0:N2}"
            };

            // Add the LinearAxis to the PlotModel
            plotModel.Axes.Add(xAxis);

            // Create a BarSeries
            var series = new BarSeries
            {
                StrokeColor = OxyColors.LightBlue,
                StrokeThickness = 2,
                FillColor = OxyColor.FromRgb(100, 100, 255) // Light Blue color
            };

            // Example data with unique item names
            var items = new List<string>();

            // Add data points for grocery items and their costs
            Server.Request(
                "/all_items_from_user",
                new { UserId = Main.UserId },
                (string response) => {
                    List<(string, decimal)> result = JsonConvert.DeserializeObject<List<(string, decimal)>>(response);
                    List<string> list = new List<string>();
                    foreach ((string, decimal) l in result)
                    {
                        series.Items.Add(new BarItem { Value = (double)l.Item2, CategoryIndex = 0 });
                        items.Add(l.Item1);

                    }
                    // Add the BarSeries to the PlotModel
                    plotModel.Series.Add(series);

                    // Set the Y-axis labels to the unique item names
                    yAxis.Labels.AddRange(items);

                    // Set the PlotModel to the PlotView
                    GraphSpot.Model = plotModel;
                    graphTitle.Text = "Item Costs";
                });
        }

        /// <summary>
        /// Holder empty click event
        /// </summary>
        /// <param name="sender">The button being pressed</param>
        /// <param name="e">Metadata for this event</param>
        public void HLCClick(object sender, RoutedEventArgs e)
        {
            // Create the PlotModel
            var plotModel = new PlotModel { };

            // Create the X-axis
            var xAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Date",
                StringFormat = "yyyy-MM-dd",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
            };

            // Add the X-axis to the PlotModel
            plotModel.Axes.Add(xAxis);

            // Create the Y-axis
            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Price",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
            };

            // Add the Y-axis to the PlotModel
            plotModel.Axes.Add(yAxis);

            // Create a HighLowSeries
            var series = new HighLowSeries
            {
                Color = OxyColors.Blue, // Line color
                TrackerFormatString = "Date: {2:yyyy-MM-dd}\nHigh: {4}\nLow: {5}",
                StrokeThickness = 2
            };

            // Add data points for the high-low series
            var random = new Random();
            var startDate = new DateTime(2023, 1, 1);

            for (int i = 0; i < 30; i++)
            {
                var date = startDate.AddDays(i);
                var high = random.Next(50, 100);
                var low = random.Next(20, 50);

                series.Items.Add(new HighLowItem(date.ToOADate(), high, low));
            }

            // Add the HighLowSeries to the PlotModel
            plotModel.Series.Add(series);

            // Set the PlotModel to the PlotView
            GraphSpot.Model = plotModel;
            graphTitle.Text = "High-Low Sale tracker";
        }
    }
}
