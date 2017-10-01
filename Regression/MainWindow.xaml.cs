using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Controls.DataVisualization.Charting;
using System.Threading.Tasks;

namespace Regression
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
           
            InitializeComponent();
            B2.IsEnabled = false;
        }

        double[] yLine = new double[300];
        double[] xLine = new double[300];
        int rw;
        int cl;
        Excel.Range range;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            await Task.Run(() =>
            {
                OpenFile();
            });
        }

        private void OpenFile()
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(@"C:\Users\rbt1\Documents\Visual Studio 2013\Projects\Regression\Regression\bin\Debug\stat.csv", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            rw = range.Rows.Count;
            cl = range.Columns.Count;


            string[] values = new string[(range.Cells[1, 1] as Excel.Range).Value2.ToString().Split(',').Length];
            for (int i = 0; i < (range.Cells[1, 1] as Excel.Range).Value2.ToString().Split(',').Length; i++)
                values[i] = (range.Cells[1, 1] as Excel.Range).Value2.ToString().Split(',')[i];


                foreach (var value in values)
                {
                Dispatcher.Invoke(()=> { comboValues1.Items.Add(value); });
                Dispatcher.Invoke(() => { comboValues2.Items.Add(value); });
                Dispatcher.Invoke(() => { B2.IsEnabled = true; });
                }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int firstItem = comboValues1.SelectedIndex;
            int secondItem = comboValues2.SelectedIndex;
            for (int i = 2; i < 302; i++)
            {
                // if (Convert.ToDouble((range.Cells[i, 1] as Excel.Range).Value2.ToString().Split(',')[secondItem].CompareTo(".")))
                yLine[i - 2] = Convert.ToDouble((range.Cells[i, 1] as Excel.Range).Value2.ToString().Split(',')[secondItem]);
                // if(Convert.ToDouble((range.Cells[i, 1] as Excel.Range).Value2.ToString().Split(',')[firstItem].CompareTo(".")))
                xLine[i - 2] = Convert.ToDouble((range.Cells[i, 1] as Excel.Range).Value2.ToString().Split(',')[firstItem].Replace('.', ','));
            }
            KeyValuePair<double, double>[] valueList = new KeyValuePair<double, double>[xLine.Length];

            LinearRegression LR = new LinearRegression(xLine, yLine);
            labelEq.Content = LR.GetEquation();
            labelR.Content = LR.GetDetermination();

            for (int i = 0; i < xLine.Length; i++)
                valueList[i] = new KeyValuePair<double, double>(xLine[i], yLine[i]);

            KeyValuePair<double, double>[] regressionLine = new KeyValuePair<double, double>[2] { new KeyValuePair<double, double>(xLine.Min(), LR.Predict(xLine.Min())), new KeyValuePair<double, double>(xLine.Max(), LR.Predict(xLine.Max())) };

            ((LineSeries)Graph.Series[0]).ItemsSource = valueList;
            ((LineSeries)Graph.Series[1]).ItemsSource = regressionLine;
            Gr2.Title = "Regression Line";
            Gr1.Title = "Data points";
        }
    }
}

